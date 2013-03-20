using System;

public class SimRuleValueGlobal : SimRuleValue
{
	public SimResource resource;
	
	public override int Get (SimRuleContext context)
	{
		return context.globalResources.GetAmount(resource);
	}

	public override int Capacity (SimRuleContext context)
	{
		return context.globalResources.GetCapacity(resource);
	}
	
	public override void Add (SimRuleContext context, int toAdd)
	{
		context.globalResources.AddResource(resource, toAdd);
	}

	public override void Remove (SimRuleContext context, int toRemove)
	{
		context.globalResources.RemoveResource(resource, toRemove);
	}
}


