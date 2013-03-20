using System;
using System.Collections.Generic;

public class SimList<T>
{
	public List<T> items = new List<T>();
	
	public List<T> itemsToAdd = new List<T>();
	public List<T> itemsToRemove = new List<T>();
	
	private bool iterating;
	
	public void Add(T item)
	{
		if (iterating)
		{
			if (itemsToRemove.Contains(item))
				itemsToRemove.Remove(item);
			
			itemsToAdd.Add(item);
		}
		else
		{
			items.Add(item);
		}
	}

	public void Remove(T item)
	{
		if (iterating)
		{
			if (itemsToAdd.Contains(item))
				itemsToAdd.Remove(item);
			
			itemsToRemove.Add(item);
		}
		else
		{
			items.Remove(item);
		}
	}
	
	public void StartIterating()
	{
		iterating = true;
	}
	
	public void StopIterating()
	{
		iterating = false;
		
		for (int i = 0; i < itemsToAdd.Count; i++)
			items.Add(itemsToAdd[i]);
		
		for (int i = 0; i < itemsToRemove.Count; i++)
			items.Remove(itemsToRemove[i]);
		
		itemsToAdd.Clear();
		itemsToRemove.Clear();
	}
}


