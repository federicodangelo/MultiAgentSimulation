using System;

public class SimResourceBin
{
	public SimResource resouce;
	public int capacity = int.MaxValue;
	public int amount;
	
	public void Add(int toAdd)
	{
		if (toAdd < 0)
			throw new ArgumentOutOfRangeException("toAdd", toAdd, "toAdd can't be a negative value");
		
		amount += toAdd;
		if (amount > capacity)
			amount = capacity;
	}
	
	public void Remove(int toRemove)
	{
		if (toRemove < 0)
			throw new ArgumentOutOfRangeException("toRemove", toRemove, "toRemove can't be a negative value");
		
		amount -= toRemove;
		if (amount < 0)
			amount = 0;
	}
	
	public void TransferTo(SimResourceBin targetBin)
	{
		int toTransfer = Math.Min(amount, targetBin.capacity - targetBin.amount);
		
		amount -= toTransfer;
		
		targetBin.amount += toTransfer;
	}
	
	public void SetCapacity(int capacity)
	{
		this.capacity = capacity;
		if (amount > capacity)
			amount = capacity;
	}
}


