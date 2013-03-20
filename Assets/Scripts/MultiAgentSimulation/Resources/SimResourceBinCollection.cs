using System.Collections.Generic;

public class SimResourceBinCollection
{
	public List<SimResourceBin> bins = new List<SimResourceBin>();
	
	public void AddResource(SimResource resource, int amount)
	{
		SimResourceBin bin = FindOrAddBin(resource);
		
		bin.Add(amount);
	}
	
	public void AddResources(SimResourceBinCollection resourcesToAdd)
	{
		for (int i = 0; i < resourcesToAdd.bins.Count; i++)
			AddResource(resourcesToAdd.bins[i].resouce, resourcesToAdd.bins[i].amount);
	}
	
	public bool CanAddSomeResources(SimResourceBinCollection resourcesToTryAdd)
	{
		for (int i = 0; i < resourcesToTryAdd.bins.Count; i++)
		{
			SimResourceBin sourceBin = resourcesToTryAdd.bins[i];
			
			if (sourceBin.amount > 0 && GetAmount(sourceBin.resouce) < GetCapacity(sourceBin.resouce))
				return true;
		}
		
		return false;
	}
	
	public void TransferResourcesTo(SimResourceBinCollection resourcesTarget)
	{
		for (int i = 0; i < bins.Count; i++)
		{
			SimResourceBin sourceBin = bins[i];
			SimResourceBin targetBin = resourcesTarget.FindOrAddBin(sourceBin.resouce);
			
			sourceBin.TransferTo(targetBin);
		}
	}
	
	public void RemoveResource(SimResource resource, int amount)
	{
		SimResourceBin bin = FindBin(resource);
		
		if (bin != null)
			bin.Remove(amount);
	}
	
	public int GetAmount(SimResource resource)
	{
		SimResourceBin bin = FindBin(resource);
		
		if (bin != null)
			return bin.amount;
		
		return 0;
	}
	
	public void SetCapacity(SimResource resource, int capacity)
	{
		SimResourceBin bin = FindOrAddBin(resource);
		
		bin.SetCapacity(capacity);
	}
	
	public void SetCapacities(SimResourceBinCollection resourcesCapacities)
	{
		for (int i = 0; i < resourcesCapacities.bins.Count; i++)
			SetCapacity(resourcesCapacities.bins[i].resouce, resourcesCapacities.bins[i].amount);
	}
	
	public int GetCapacity(SimResource resource)
	{
		SimResourceBin bin = FindBin(resource);
		
		if (bin != null)
			return bin.capacity;
		
		return int.MaxValue;
	}
	
	private SimResourceBin FindBin(SimResource resource)
	{
		for (int i = 0; i < bins.Count; i++)
			if (bins[i].resouce == resource)
				return bins[i];
		
		return null;
	}
	
	private SimResourceBin FindOrAddBin(SimResource resource)
	{
		SimResourceBin bin = FindBin(resource);
		
		if (bin == null)
		{
			bin = new SimResourceBin();
			bin.resouce = resource;
			bins.Add(bin);
		}
		
		return bin;
	}
	
	public bool IsEmpty()
	{
		for (int i = 0; i < bins.Count; i++)
			if (bins[i].amount > 0)
				return false;
		
		return true;
	}
}
