using System;

public class SimRuleValueMap : SimRuleValue
{
	public string mapId;
	
	public override int Get (SimRuleContext context)
	{
		return context.box.GetMap(mapId).Get(context.mapPositionX, context.mapPositionY, context.mapPositionRadius);
	}

	public override int Capacity (SimRuleContext context)
	{
		return context.box.GetMap(mapId).Capacity(context.mapPositionX, context.mapPositionY, context.mapPositionRadius);
	}
	
	public override void Add (SimRuleContext context, int toAdd)
	{
		context.box.GetMap(mapId).Add(context.mapPositionX, context.mapPositionY, context.mapPositionRadius, toAdd);
	}

	public override void Remove (SimRuleContext context, int toRemove)
	{
		context.box.GetMap(mapId).Remove(context.mapPositionX, context.mapPositionY, context.mapPositionRadius, toRemove);
	}
}


