using System;

public class SimUnit
{
	public SimUnitType unitType;
	
	public int id;
	
	public SimPoint position;
	
	public SimResourceBinCollection resources = new SimResourceBinCollection();
	
	private SimRuleContext context = new SimRuleContext();
	private int ticks;
	
	public void Init(SimUnitType unitType, int id, SimPoint position)
	{
		this.unitType = unitType;
		this.id = id;
		this.position = position;
		
		position.units.Add(this);
		
		resources.SetCapacities(unitType.caps);
		resources.AddResources(unitType.resources);
		
		context.localResources = resources;
		context.unit = this;
		context.box = position.path.box;
		context.globalResources = context.box.globals;
		context.mapPositionRadius = unitType.mapRadius;
	}
	
	public void Destroy()
	{
		position.units.Remove(this);
	}
	
	public void ExecuteRules()
	{
		ticks++;
		
		SimRule[] rules = unitType.rules;
		
		position.GetMapPosition(out context.mapPositionX, out context.mapPositionY);
		
		for (int i = 0; i < rules.Length; i++)
			if (ticks % rules[i].rate == 0)
				rules[i].Execute(context);
	}
	
	public bool Accepts(string searchTarget, SimResourceBinCollection resourcesToTryToAdd)
	{
		return Array.IndexOf(unitType.targets, searchTarget) >= 0 &&
				resources.CanAddSomeResources(resourcesToTryToAdd);
	}
}
