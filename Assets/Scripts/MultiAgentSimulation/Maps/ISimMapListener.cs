using System;

public interface ISimMapListener
{
	void OnMapModified(SimMap map, int x, int y, int val);
}


