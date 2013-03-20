using System;

public interface ISimPathListener
{
	void OnSegmentAdded(SimPath path, SimSegment segment);
	void OnSegmentRemoved(SimPath path, SimSegment segment);
	void OnSegmentModified(SimPath path, SimSegment segment);

	void OnPointAdded(SimPath path, SimPoint point);
	void OnPointRemoved(SimPath path, SimPoint point);
	void OnPointModified(SimPath path, SimPoint point);
}


