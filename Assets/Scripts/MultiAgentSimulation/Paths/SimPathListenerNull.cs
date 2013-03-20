using System;

public class SimPathListenerNull : ISimPathListener
{
	public void OnSegmentAdded (SimPath path, SimSegment segment)
	{
	}

	public void OnSegmentModified (SimPath path, SimSegment segment)
	{
	}

	public void OnSegmentRemoved (SimPath path, SimSegment segment)
	{
	}

	public void OnPointAdded (SimPath path, SimPoint point)
	{
	}

	public void OnPointRemoved (SimPath path, SimPoint point)
	{
	}

	public void OnPointModified (SimPath path, SimPoint point)
	{
	}
}


