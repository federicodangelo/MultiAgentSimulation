using UnityEngine;

public class SimAgentUnity : MonoBehaviour
{
	public SimAgent agent;
	
	private Transform sphere;
	
	public void Init(SimAgent agent)
	{
		this.agent = agent;
		
		gameObject.name = agent.agentType.id + "-" + agent.id;
		
		transform.localPosition = agent.worldPosition;
		
		sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
		
		sphere.parent = transform;
		sphere.localPosition = Vector3.zero;
		
		Material unitMaterial = MaterialsFactory.CreateDiffuseColor(agent.agentType.color);
		
		sphere.renderer.sharedMaterial = unitMaterial; 
	}
	
	public void Update()
	{
		transform.localPosition = agent.worldPosition;
	}
}


