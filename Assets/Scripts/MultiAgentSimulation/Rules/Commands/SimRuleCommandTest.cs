using System;

public class SimRuleCommandTest : SimRuleCommand
{
	public enum Comparison
	{
		Equals,
		Greater,
		Less
	}
	
	public SimRuleValue target;
	
	public int amount;
	
	public Comparison comparison;
	
	public override bool Validate (SimRuleContext context)
	{
		int val = target.Get(context);
		
		switch(comparison)
		{
			case Comparison.Equals:
				return val == amount;
			
			case Comparison.Greater:
				return val > amount;
			
			case Comparison.Less:
				return val < amount;
		}
		
		return true;
	}
	
	public override void Execute (SimRuleContext context)
	{
		//do nothing
	}
}


