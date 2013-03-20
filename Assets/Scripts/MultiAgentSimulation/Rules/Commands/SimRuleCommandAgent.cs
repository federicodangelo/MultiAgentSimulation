using System;

public class SimRuleCommandAgent : SimRuleCommand
{
	public string searchTarget;
	
	public SimAgentType agentType;
	
	public SimResourceBinCollection resources = new SimResourceBinCollection();
	
	public override bool Validate (SimRuleContext context)
	{
		return true;
	}
	
	public override void Execute (SimRuleContext context)
	{
		context.box.AddAgent(agentType, context.unit.position, context.unit, resources, searchTarget);
	}
}
