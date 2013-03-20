using System;

public class SimRuleValueLocal : SimRuleValue
{
	public SimResource resource;
	
	public override int Get (SimRuleContext context)
	{
		return context.localResources.GetAmount(resource);
	}
	
	public override int Capacity (SimRuleContext context)
	{
		return context.localResources.GetCapacity(resource);
	}

	public override void Add (SimRuleContext context, int toAdd)
	{
		context.localResources.AddResource(resource, toAdd);
	}

	public override void Remove (SimRuleContext context, int toRemove)
	{
		context.localResources.RemoveResource(resource, toRemove);
	}
}


