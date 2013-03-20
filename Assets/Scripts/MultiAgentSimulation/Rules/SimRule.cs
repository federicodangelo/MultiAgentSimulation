using System;
using System.Collections.Generic;

public class SimRule
{
	public string id;
	
	public SimRuleCommand[] commands;
	
	public int rate = 1;
	
	public virtual bool Execute(SimRuleContext context)
	{
		for (int i = 0; i < commands.Length; i++)
			if (!commands[i].Validate(context))
				return false;
		
		for (int i = 0; i < commands.Length; i++)
			commands[i].Execute(context);
		
		return true;
	}
	
	public virtual void SetOption(string optionId, string val)
	{
		switch(optionId)
		{
			case "rate":
				if (!int.TryParse(val, out rate))
					rate = 1;
				break;
		}
	}
}
