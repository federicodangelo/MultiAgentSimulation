using System;

public class SimRuleCommandAdd : SimRuleCommand
{
	public SimRuleValue target;
	
	public int amount;
	
	public override bool Validate (SimRuleContext context)
	{
		return target.Get(context) < target.Capacity(context);
	}
	
	public override void Execute (SimRuleContext context)
	{
		target.Add(context, amount);
	}
}


