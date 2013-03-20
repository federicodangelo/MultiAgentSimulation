using UnityEngine;

public class SimSegmentUnity : MonoBehaviour
{
	public SimSegment segment;
	
	private Transform cube;
	
	public void Init(SimSegment segment)
	{
		this.segment = segment;
		
		gameObject.name = "Segment-" + segment.id;
		
		cube = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
		cube.parent = transform;
		
		Modified();
	}
	
	public void Modified()
	{
		transform.localPosition = 
			new Vector3( 
				segment.point1.worldPosition.x,
				segment.point1.worldPosition.y,
				segment.point1.worldPosition.z);
		
		SimVector3 delta = segment.point2.worldPosition - segment.point1.worldPosition;
		SimVector3 midPoint = delta * 0.5f;
		
		cube.localPosition = new Vector3(midPoint.x, midPoint.y, midPoint.z);
		cube.LookAt(new Vector3(segment.point2.worldPosition.x, segment.point2.worldPosition.y, segment.point2.worldPosition.z));
		cube.localScale = new Vector3(0.2f, 0.2f, delta.Len);
	}
}


