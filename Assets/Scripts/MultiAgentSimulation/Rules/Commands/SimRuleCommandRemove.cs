using System;

public class SimRuleCommandRemove : SimRuleCommand
{
	public SimRuleValue target;
	
	public int amount;
	
	public override bool Validate (SimRuleContext context)
	{
		return target.Get(context) >= amount;
	}
	
	public override void Execute (SimRuleContext context)
	{
		target.Remove(context, amount);
	}
}


