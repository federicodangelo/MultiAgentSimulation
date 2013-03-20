using UnityEngine;
using System.Collections;

public class TestSimulationUnity : SimulationUnity
{
	public TextAsset cityToTest;
	
	// Use this for initialization
	void Start ()
	{
		Simulation sim = new Simulation();
		sim.simulationDefinition = new SimulationDefinitionLoader().LoadDefinitionFromString(cityToTest.text);
		
		Init (sim);
		
		SimBox city = sim.AddBox("city", Vector3.zero, 32, 32);
		
		SimPath road = city.GetPath("Road");
		
		SimPoint p1 = road.AddPoint(new Vector3(20, 0, 20));
		SimPoint p2 = road.AddPoint(new Vector3(50, 0, 50));
		SimPoint p3 = road.AddPoint(new Vector3(20, 0, 50));
		
		SimSegment s1 = road.AddSegment(sim.simulationDefinition.GetSegmentType("Dirt"), p1, p2);
		SimSegment s2 = road.AddSegment(sim.simulationDefinition.GetSegmentType("Dirt"), p2, p3);
		SimSegment s3 = road.AddSegment(sim.simulationDefinition.GetSegmentType("Dirt"), p3, p1);
		
		SimSegmentPosition unitPos;
		
		unitPos.segment = s1;
		unitPos.offset = 0.66f;
		
		city.AddUnit(sim.simulationDefinition.GetUnitType("Home"), unitPos);
		
		unitPos.segment = s1;
		unitPos.offset = 0.5f;
		
		city.AddUnit(sim.simulationDefinition.GetUnitType("Home"), unitPos);
		
		unitPos.segment = s2;
		unitPos.offset = 0.5f;
		
		city.AddUnit(sim.simulationDefinition.GetUnitType("Work"), unitPos);
		
		unitPos.segment = s3;
		unitPos.offset = 0.5f;
		
		city.AddUnit(sim.simulationDefinition.GetUnitType("Work"), unitPos);
	}
}

