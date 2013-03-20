using System;

public class SimRuleUnit : SimRule
{
	public SimRuleUnit onFail;
	
	public override bool Execute (SimRuleContext context)
	{
		if (base.Execute(context))
		{
			return true;
		}
		else
		{
			if (onFail != null)
				return onFail.Execute(context);
			else
				return false;
		}
	}
}


