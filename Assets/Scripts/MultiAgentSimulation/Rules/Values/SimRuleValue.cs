using System;

public class SimRuleValue
{
	public virtual int Get(SimRuleContext context)
	{
		//Override
		return 0;
	}

	public virtual int Capacity(SimRuleContext context)
	{
		//Override
		return 0;
	}
	
	public virtual void Add(SimRuleContext context, int toAdd)
	{
		//Override
	}
	
	public virtual void Remove(SimRuleContext context, int toRemove)
	{
		//Override
	}
	
}


