using System;
using System.Collections.Generic;

public class SimulationDefinitionLoader
{
	private string[] lines;
	private int nextLineIndex;
	private SimulationDefinition definition;
	
	private Dictionary<string, SimRule> rules = new Dictionary<string, SimRule>();
	
	public SimulationDefinition LoadDefinitionFromString(string strDefinition)
	{
		rules.Clear();
		
		LoadLines (strDefinition);
		
		definition = new SimulationDefinition();
		
		ParseDefinition();
		
		return definition;
	}
	
	private void ParseDefinition()
	{
		string line = GetNextLine();
		
		while(line != null)
		{
			switch(line)
			{
				case "resources":
					ParseResources();
					break;
				
				case "rules":
					ParseRules();
					break;
				
				case "maps":
					ParseMaps();
					break;
					
				case "paths":
					ParsePaths();
					break;
				
				case "segments":
					ParseSegments();
					break;
				
				case "agents":
					ParseAgents();
					break;
				
				case "units":
					ParseUnits();
					break;
				
				default:
					ThrowInvalidLine("ParseDefinition()");
					break;
			}
			
			line = GetNextLine();
		}
	}
	

	private void ParseRules()
	{
		string line = GetNextLine();
		
		while(line != null)
		{
			string[] lineSplit = SplitLine(line);
			
			switch(lineSplit[0])
			{
				case "end":
					return;
				
				case "mapRule":
					ReturnCurrentLine();
					ParseRuleMap();
					break;
				
				case "unitRule":
					ReturnCurrentLine();
					ParseRuleUnit();
					break;
				
				default:
					ThrowInvalidLine("ParseRules()");
					break;
			}
			
			line = GetNextLine();
		}
	}	
	

	private void ParseRuleMap()
	{
		string line = GetNextLine();
		
		string[] lineSplit = SplitLine(line);
		
		if (lineSplit.Length == 1)
			ThrowInvalidLine("ParseRuleMap()");
		
		SimRuleMap ruleMap = new SimRuleMap();
		
		ruleMap.id = lineSplit[1];

		line = GetNextLine();
		
		List<SimRuleCommand> commands = new List<SimRuleCommand>();
		
		while(line != "end")
		{
			lineSplit = SplitLine(line);
			
			switch(lineSplit[0])
			{
				case "rate":
					ruleMap.rate = ParseInt(lineSplit[1]);					
					break;
				
				case "randomTiles":
					ruleMap.randomTiles = ParseBool(lineSplit[1]);					
					break;
				
				case "randomTilesPercent":
					ruleMap.randomTilesPercent = ParseInt(lineSplit[1]);					
					break;
				
				default:
					SimRuleCommand command = ParseCommand(line);
					if (command == null)
						ThrowInvalidLine("ParseRuleMap()");
					commands.Add(command);
					break;
			}
			
			line = GetNextLine();
		}
		
		ruleMap.commands = commands.ToArray();
		
		this.rules.Add(ruleMap.id, ruleMap);
	}
	
	private void ParseRuleUnit()
	{
		string line = GetNextLine();
		
		string[] lineSplit = SplitLine(line);
		
		if (lineSplit.Length == 1)
			ThrowInvalidLine("ParseRuleUnit()");
		
		SimRuleUnit ruleUnit = new SimRuleUnit();
		
		ruleUnit.id = lineSplit[1];

		line = GetNextLine();
		
		List<SimRuleCommand> commands = new List<SimRuleCommand>();
		
		while(line != "end")
		{
			lineSplit = SplitLine(line);
			
			switch(lineSplit[0])
			{
				case "rate":
					ruleUnit.rate = ParseInt(lineSplit[1]);					
					break;
				
				case "onFail":
					ruleUnit.onFail = FindRule<SimRuleUnit>(lineSplit[1]);
					break;
				
				default:
					SimRuleCommand command = ParseCommand(line);
					if (command == null)
						ThrowInvalidLine("ParseRuleUnit()");
					commands.Add(command);
					break;
			}
			
			line = GetNextLine();
		}
		
		ruleUnit.commands = commands.ToArray();
		
		this.rules.Add(ruleUnit.id, ruleUnit);
	}	
	
	private SimRuleCommand ParseCommand(string line)
	{
		string[] lineSplit = SplitLine(line);
		int lineSplitOffset = 0;
		
		SimRuleCommand command = null;
		
		//Find target
		SimRuleValue target = null;
		
		switch(lineSplit[lineSplitOffset])
		{
			case "local":
				target = new SimRuleValueLocal();
				((SimRuleValueLocal) target).resource = definition.GetResource(lineSplit[1]);
				lineSplitOffset += 2;
				break;
			
			case "global":
				target = new SimRuleValueGlobal();
				((SimRuleValueGlobal) target).resource = definition.GetResource(lineSplit[1]);
				lineSplitOffset += 2;
				break;
			
			case "map":
				target = new SimRuleValueMap();
				((SimRuleValueMap) target).mapId = lineSplit[1];
				lineSplitOffset += 2;
				break;
			
			case "agent":
				command = new SimRuleCommandAgent();
				((SimRuleCommandAgent) command).agentType = definition.GetAgentType(lineSplit[1]);
				lineSplitOffset += 2;
			
				while(lineSplitOffset < lineSplit.Length)
				{
					switch(lineSplit[lineSplitOffset++])
					{
						case "to":
							((SimRuleCommandAgent) command).searchTarget = lineSplit[lineSplitOffset++];
							break;
							
						case "add":
							((SimRuleCommandAgent) command).resources = ParseResourcesArray(lineSplit, ref lineSplitOffset);
							break;
					
						default:
							ThrowInvalidLine("ParseCommand() - Invalid agent parameter");
							break;
					}
				}
				break;
				
		}
		
		if (target != null)
		{
			switch(lineSplit[lineSplitOffset])
			{
				case "add":
					command = new SimRuleCommandAdd();
					((SimRuleCommandAdd) command).target = target;
					((SimRuleCommandAdd) command).amount = ParseInt(lineSplit[lineSplitOffset + 1]);
					break;
				
				case "remove":
					command = new SimRuleCommandRemove();
					((SimRuleCommandRemove) command).target = target;
					((SimRuleCommandRemove) command).amount = ParseInt(lineSplit[lineSplitOffset + 1]);
					break;
				
				case "greater":
					command = new SimRuleCommandTest();
					((SimRuleCommandTest) command).target = target;
					((SimRuleCommandTest) command).comparison = SimRuleCommandTest.Comparison.Greater;
					((SimRuleCommandTest) command).amount = ParseInt(lineSplit[lineSplitOffset + 1]);
					break;
				
				case "less":
					command = new SimRuleCommandTest();
					((SimRuleCommandTest) command).target = target;
					((SimRuleCommandTest) command).comparison = SimRuleCommandTest.Comparison.Less;
					((SimRuleCommandTest) command).amount = ParseInt(lineSplit[lineSplitOffset + 1]);
					break;
				
				case "equals":
					command = new SimRuleCommandTest();
					((SimRuleCommandTest) command).target = target;
					((SimRuleCommandTest) command).comparison = SimRuleCommandTest.Comparison.Equals;
					((SimRuleCommandTest) command).amount = ParseInt(lineSplit[lineSplitOffset + 1]);
					break;
			}
		}
		
		return command;
	}
	
	private void ThrowInvalidLine()
	{
		ThrowInvalidLine("");
	}
		
	private void ThrowInvalidLine(string context)
	{
		string line = "";
		
		if (nextLineIndex - 1 >= 0 && lines.Length > 0)
			line = lines[nextLineIndex - 1];
		
		string message;
		
		if (!string.IsNullOrEmpty(context))
			message = string.Format("{0}: Invalid content in line number {1}, '{2}'", context, nextLineIndex - 1, line); 
		else
			message = string.Format("Invalid content in line number {0}, '{1}'", nextLineIndex - 1, line); 
		
		throw new Exception(message);
	}
	
	private void ParseMaps()
	{
		string line = GetNextLine();
		
		while(line != null)
		{
			string[] lineSplit = SplitLine(line);
			
			switch(lineSplit[0])
			{
				case "end":
					return;
				
				case "map":
					ReturnCurrentLine();
					ParseMap();
					break;
				
				default:
					ThrowInvalidLine("ParseMaps()");
					break;
			}
			
			line = GetNextLine();
		}
	}
	
	private void ParseMap()
	{
		string line = GetNextLine();
		
		string[] lineSplit = SplitLine(line);
		
		if (lineSplit.Length == 1)
			ThrowInvalidLine("ParseMap()");
		
		SimMapType mapType = new SimMapType();
		mapType.id = lineSplit[1];
		mapType.rules = new SimRuleMap[0];
		
		int splitOffset = 2;
		
		while(splitOffset < lineSplit.Length)
		{
			switch(lineSplit[splitOffset++])
			{
				case "color":
					mapType.color = ParseUint(lineSplit[splitOffset++]);
					break;
				
				case "capacity":
					mapType.capacity = ParseInt(lineSplit[splitOffset++]);
					break;
				
				case "rules":
				{
					string[] rulesIds = ParseStringArray(lineSplit, ref splitOffset);
					mapType.rules = FindRules<SimRuleMap>(rulesIds);
					break;
				}
				
				default:
					ThrowInvalidLine("ParseMap()");
					break;
			}
		}
		
		definition.mapTypes.Add(mapType.id, mapType);
	}
	
	private void ParsePaths()
	{
		string line = GetNextLine();
		
		while(line != null)
		{
			string[] lineSplit = SplitLine(line);
			
			switch(lineSplit[0])
			{
				case "end":
					return;
				
				case "path":
					ReturnCurrentLine();
					ParsePath();
					break;
				
				default:
					ThrowInvalidLine("ParsePaths()");
					break;
			}
			
			line = GetNextLine();
		}
	}
	
	private void ParsePath()
	{
		string line = GetNextLine();
		
		string[] lineSplit = SplitLine(line);
		
		if (lineSplit.Length == 1)
			ThrowInvalidLine("ParsePath()");
		
		SimPathType pathType = new SimPathType();
		pathType.id = lineSplit[1];
		
		int splitOffset = 2;
		
		while(splitOffset < lineSplit.Length)
		{
			switch(lineSplit[splitOffset++])
			{
				case "color":
					pathType.color = ParseUint(lineSplit[splitOffset++]);
					break;
								
				default:
					ThrowInvalidLine("ParsePath()");
					break;
			}
		}
		
		definition.pathTypes.Add(pathType.id, pathType);
	}
	
	private void ParseSegments()
	{
		string line = GetNextLine();
		
		while(line != null)
		{
			string[] lineSplit = SplitLine(line);
			
			switch(lineSplit[0])
			{
				case "end":
					return;
				
				case "segment":
					ReturnCurrentLine();
					ParseSegment();
					break;
				
				default:
					ThrowInvalidLine("ParseSegments()");
					break;
			}
			
			line = GetNextLine();
		}
	}
	
	private void ParseSegment()
	{
		string line = GetNextLine();
		
		string[] lineSplit = SplitLine(line);
		
		if (lineSplit.Length == 1)
			ThrowInvalidLine("ParseSegment()");
		
		SimSegmentType segmentType = new SimSegmentType();
		segmentType.id = lineSplit[1];
		
		int splitOffset = 2;
		
		while(splitOffset < lineSplit.Length)
		{
			switch(lineSplit[splitOffset++])
			{
				case "color":
					segmentType.color = ParseUint(lineSplit[splitOffset++]);
					break;
								
				default:
					ThrowInvalidLine("ParseSegment()");
					break;
			}
		}
		
		definition.segmentTypes.Add(segmentType.id, segmentType);
	}
		
	private void ParseUnits()
	{
		string line = GetNextLine();
		
		while(line != null)
		{
			string[] lineSplit = SplitLine(line);
			
			switch(lineSplit[0])
			{
				case "end":
					return;
				
				case "unit":
					ReturnCurrentLine();
					ParseUnit();
					break;
				
				default:
					ThrowInvalidLine("ParseUnits()");
					break;
			}
			
			line = GetNextLine();
		}
	}
	
	private void ParseUnit()
	{
		string line = GetNextLine();
		
		string[] lineSplit = SplitLine(line);
		
		if (lineSplit.Length == 1)
			ThrowInvalidLine("ParseUnit()");
		
		SimUnitType unitType = new SimUnitType();
		unitType.id = lineSplit[1];
		unitType.rules = new SimRuleUnit[0];
		unitType.resources = new SimResourceBinCollection();
		unitType.caps = new SimResourceBinCollection();
		unitType.targets = new string[0];
		
		int splitOffset = 2;
		
		while(splitOffset < lineSplit.Length)
		{
			switch(lineSplit[splitOffset++])
			{
				case "color":
					unitType.color = ParseUint(lineSplit[splitOffset++]);
					break;
				
				case "mapRadius":
					unitType.mapRadius = ParseInt(lineSplit[splitOffset++]);
					break;
				
				case "caps":
					unitType.caps = ParseResourcesArray(lineSplit, ref splitOffset);
					break;
					
				case "resources":
					unitType.resources = ParseResourcesArray(lineSplit, ref splitOffset);
					break;
				
				case "rules":
				{
					string[] rulesIds = ParseStringArray(lineSplit, ref splitOffset);
					unitType.rules = FindRules<SimRuleUnit>(rulesIds);
					break;
				}
				
				case "targets":
				{
					string[] targets = ParseStringArray(lineSplit, ref splitOffset);
					unitType.targets = targets;
					break;
				}
				
				default:
					ThrowInvalidLine("ParseUnit()");
					break;
			}
		}
		
		definition.unitTypes.Add(unitType.id, unitType);
	}	
	
	private void ParseAgents()
	{
		string line = GetNextLine();
		
		while(line != null)
		{
			string[] lineSplit = SplitLine(line);
			
			switch(lineSplit[0])
			{
				case "end":
					return;
				
				case "agent":
					ReturnCurrentLine();
					ParseAgent();
					break;
				
				default:
					ThrowInvalidLine("ParseAgents()");
					break;
			}
			
			line = GetNextLine();
		}
	}
	
	private void ParseAgent()
	{
		string line = GetNextLine();
		
		string[] lineSplit = SplitLine(line);
		
		if (lineSplit.Length == 1)
			ThrowInvalidLine("ParseAgent()");
		
		SimAgentType agentType = new SimAgentType();
		agentType.id = lineSplit[1];
		
		int splitOffset = 2;
		
		while(splitOffset < lineSplit.Length)
		{
			switch(lineSplit[splitOffset++])
			{
				case "color":
					agentType.color = ParseUint(lineSplit[splitOffset++]);
					break;
				
				case "speed":
					agentType.speed = ParseFloat(lineSplit[splitOffset++]);
					break;
				
				default:
					ThrowInvalidLine("ParseAgent()");
					break;
			}
		}
		
		definition.agentTypes.Add(agentType.id, agentType);
	}	
	
	private T FindRule<T>(string ruleId) where T : SimRule
	{
		if (!rules.ContainsKey(ruleId))
			ThrowInvalidLine("FindRule() - Rule Not Found");
		
		if (!(rules[ruleId] is T))
			ThrowInvalidLine("FindRule() - Invalid Rule Type");
		
		return (T) rules[ruleId];
	}
	
	private T[] FindRules<T>(string[] ruleIds) where T : SimRule
	{
		T[] foundRules = new T[ruleIds.Length];
		
		for (int i = 0; i < ruleIds.Length; i++)
		{
			if (!rules.ContainsKey(ruleIds[i]))
				ThrowInvalidLine("FindRules() - Rule Not Found '" + ruleIds[i] + "'");
			
			if (!(rules[ruleIds[i]] is T))
				ThrowInvalidLine("FindRules() - Invalid Rule Type '" + ruleIds[i] + "'");
			
			foundRules[i] = (T) rules[ruleIds[i]];
		}
		
		return foundRules;
	}
	
	private string[] ParseStringArray(string[] lineSplit, ref int splitOffset)
	{
		if (lineSplit[splitOffset] != "[")
			ThrowInvalidLine("ParseStringArray()");
		
		List<string> stringList = new List<string>();
		
		splitOffset++; //skip "["
		
		while(lineSplit[splitOffset] != "]")
		{
			stringList.Add(lineSplit[splitOffset]);
			
			splitOffset++;
		}
		
		splitOffset++; //skip "]"
		
		return stringList.ToArray();
	}
	
	private SimResourceBinCollection ParseResourcesArray(string[] lineSplit, ref int splitOffset)
	{
		string[] strings = ParseStringArray(lineSplit, ref splitOffset);
		
		SimResourceBinCollection resources = new SimResourceBinCollection();
		
		for (int i = 0; i < strings.Length; i += 2)
		{
			SimResource resource = definition.GetResource(strings[i]);
			
			if (resource == null)
				ThrowInvalidLine("ParseResourcesArray() - Unknown resource id");
			
			int amount = ParseInt(strings[i + 1]);
			
			resources.AddResource(resource, amount);
		}
		
		return resources;
	}	
	
	private uint ParseUint(string s)
	{
		if (s.StartsWith("0x"))
		{
			//hexa!
			return Convert.ToUInt32(s.Substring(2), 16);
		}
		else
		{
			return Convert.ToUInt32(s);
		}
	}
	
	private int ParseInt(string s)
	{
		if (s.StartsWith("0x"))
		{
			//hexa!
			return Convert.ToInt32(s.Substring(2), 16);
		}
		else
		{
			return Convert.ToInt32(s);
		}
	}
	
	private float ParseFloat(string s)
	{
		return Convert.ToSingle(s);
	}
	
	private bool ParseBool(string s)
	{
		if (s == "true" || s == "1")
			return true;
		else
			return false;
	}
	
	private void ParseResources()
	{
		string line = GetNextLine();
		
		while(line != null)
		{
			string[] lineSplit = SplitLine(line);
			
			switch(lineSplit[0])
			{
				case "end":
					return;
				
				case "resource":
					ReturnCurrentLine();
					ParseResource();
					break;
				
				default:
					ThrowInvalidLine("ParseResources()");
					break;
			}
			
			line = GetNextLine();
		}
	}
	
	private void ParseResource()
	{
		string line = GetNextLine();
		
		string[] lineSplit = SplitLine(line);
		
		if (lineSplit.Length == 1)
			ThrowInvalidLine("ParseResource()");
		
		SimResource resource = new SimResource();
		resource.id = lineSplit[1];
		
		definition.resourceTypes.Add(resource.id, resource);
	}
	
	private void LoadLines(string str)
	{
		lines = str.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < lines.Length; i++)
			lines[i] = lines[i].Trim();
		
		nextLineIndex = 0;
	}
	
	private void ReturnCurrentLine()
	{
		nextLineIndex--;
		if (nextLineIndex < 0)
			nextLineIndex = 0;
	}
	
	private string[] SplitLine(string line)
	{
		return line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
	}
	
	private string GetNextLine()
	{
		//Skip empty lines and lines that begin with "//"
		while(nextLineIndex < lines.Length && 
			(lines[nextLineIndex].Length == 0 || lines[nextLineIndex].IndexOf("//") == 0))
		{
			nextLineIndex++;
		}
		
		if (nextLineIndex >= lines.Length)
			return null;
		
		return lines[nextLineIndex++];
	}
}


