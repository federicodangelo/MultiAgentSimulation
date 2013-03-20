using UnityEngine;
using System.Collections.Generic;

public class SimPathUnity : MonoBehaviour, ISimPathListener
{
	public SimPath path;
	
	private List<SimSegmentUnity> segmentsUnity = new List<SimSegmentUnity>();
	private List<SimPointUnity> pointsUnity = new List<SimPointUnity>();

	public void Init(SimPath path)
	{
		this.path = path;
		
		path.pathListener = this;
		
		gameObject.name = path.id;
		transform.localPosition = Vector3.zero;
		
		foreach(SimSegment segment in path.segments)
			OnSegmentAdded(path, segment);
	}
		
	public void OnSegmentAdded (SimPath path, SimSegment segment)
	{
		GameObject goSegment = new GameObject();
		goSegment.transform.parent = transform;
		SimSegmentUnity segmentUnity = goSegment.AddComponent<SimSegmentUnity>();
		
		segmentUnity.Init(segment);
		
		segmentsUnity.Add(segmentUnity);
	}

	public void OnSegmentRemoved (SimPath path, SimSegment segment)
	{
		for(int i = 0; i < segmentsUnity.Count; i++)
		{
			if (segmentsUnity[i].segment == segment)
			{
				GameObject.Destroy(segmentsUnity[i].gameObject);
				segmentsUnity.RemoveAt(i);
				break;
			}
		}
	}

	public void OnSegmentModified (SimPath path, SimSegment segment)
	{
		for(int i = 0; i < segmentsUnity.Count; i++)
		{
			if (segmentsUnity[i].segment == segment)
			{
				segmentsUnity[i].Modified();
				break;
			}
		}
	}

	public void OnPointAdded (SimPath path, SimPoint point)
	{
		GameObject goPoint = new GameObject();
		goPoint.transform.parent = transform;
		SimPointUnity pointUnity = goPoint.AddComponent<SimPointUnity>();
		
		pointUnity.Init(point);
		
		pointsUnity.Add(pointUnity);
	}

	public void OnPointRemoved (SimPath path, SimPoint point)
	{
		for(int i = 0; i < pointsUnity.Count; i++)
		{
			if (pointsUnity[i].point == point)
			{
				GameObject.Destroy(pointsUnity[i].gameObject);
				pointsUnity.RemoveAt(i);
				break;
			}
		}
	}

	public void OnPointModified (SimPath path, SimPoint point)
	{
	}
}


