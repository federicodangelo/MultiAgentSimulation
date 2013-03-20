using System;

public class TestSimulation : Simulation
{
	public TestSimulation()
	{
		simulationDefinition = CreateDefinition2();
	}
	
	private SimulationDefinition CreateDefinition2()
	{
		SimulationDefinitionLoader loader = new SimulationDefinitionLoader();
		
		string strDefinition = @"
			resources
				resource Water
				resource Grass
			end

			rules

				mapRule AddWater
					rate 10
					randomTiles true
					randomTilesPercent 25
					
					map Water add 2
				end

				mapRule CreateGrass
					rate 7
					
					map Water remove 10
					map Grass add 1
				end

			end

			maps
				map Water color 0x0000FF capacity 100 rules [ AddWater ]
				map Grass color 0x00FF00 capacity 10 rules [ CreateGrass ]
			end
		";
		
		
		return loader.LoadDefinitionFromString(strDefinition);
	}
	
	private SimulationDefinition CreateDefinition()
	{
		SimulationDefinition definition = new SimulationDefinition();
		
		SimResource resourceWater = new SimResource();
		resourceWater.id = "Water";
		SimResource resourceGrass = new SimResource();
		resourceGrass.id = "Grass";
		
		definition.resourceTypes.Add(resourceWater.id, resourceWater);
		definition.resourceTypes.Add(resourceGrass.id, resourceGrass);
		
		//Add 2 of water every 10 ticks
		SimRuleValueMap valueMapWater = new SimRuleValueMap();
		valueMapWater.mapId = "Water";
		
		SimRuleCommandAdd commandAddWater = new SimRuleCommandAdd();
		commandAddWater.amount = 2;
		commandAddWater.target = valueMapWater;
		
		SimRuleMap ruleMapAddWater = new SimRuleMap();
		ruleMapAddWater.id = "AddWater";
		ruleMapAddWater.rate = 10;
		ruleMapAddWater.randomTiles = true;
		ruleMapAddWater.randomTilesPercent = 25;
		ruleMapAddWater.commands = new SimRuleCommand[] {
			commandAddWater
		};
		
		//Water Map
		SimMapType mapTypeWater = new SimMapType();
		mapTypeWater.id = "Water";
		mapTypeWater.color = 0x0000FF;
		mapTypeWater.capacity = 100;
		mapTypeWater.rules = new SimRuleMap[] {
			ruleMapAddWater
		};
		
		definition.mapTypes.Add(mapTypeWater.id, mapTypeWater);
		
		//Add grass every 7 ticks if there is 10 of water available
		SimRuleValueMap valueMapGrass = new SimRuleValueMap();
		valueMapGrass.mapId = "Grass";
		
		SimRuleCommandRemove commandRemoveWater = new SimRuleCommandRemove();
		commandRemoveWater.amount = 10;
		commandRemoveWater.target = valueMapWater;
		
		SimRuleCommandAdd commandAddGrass = new SimRuleCommandAdd();
		commandAddGrass.amount = 1;
		commandAddGrass.target = valueMapGrass;
		
		SimRuleMap ruleMapCreateGrass = new SimRuleMap();
		ruleMapCreateGrass.id = "CreateGrass";
		ruleMapCreateGrass.rate = 7;
		ruleMapCreateGrass.commands = new SimRuleCommand[] {
			commandRemoveWater,
			commandAddGrass
		};
		
		//Grass Map
		SimMapType mapTypeGrass = new SimMapType();
		mapTypeGrass.id = "Grass";
		mapTypeGrass.color = 0x00FF00;
		mapTypeGrass.capacity = 10;
		mapTypeGrass.rules = new SimRuleMap[] {
			ruleMapCreateGrass
		};
		
		definition.mapTypes.Add(mapTypeGrass.id, mapTypeGrass);
		
		return definition;
	}
}


