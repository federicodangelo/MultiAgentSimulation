using UnityEngine;

public class SimUnitUnity : MonoBehaviour
{
	public SimUnit unit;
	
	private Transform box;
	
	public void Init(SimUnit unit)
	{
		this.unit = unit;
		
		gameObject.name = unit.unitType.id + "-" + unit.id;
		
		transform.localPosition = unit.position.worldPosition;
		
		box = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
		
		box.parent = transform;
		box.localPosition = Vector3.zero;
		
		Material unitMaterial = MaterialsFactory.CreateDiffuseColor(unit.unitType.color);
		
		box.renderer.sharedMaterial = unitMaterial; 
	}
}


