using UnityEngine;
using System.Collections;

public class SimMapUnity : MonoBehaviour, ISimMapListener
{
	public SimMap map;
	
	private Transform[] mapValues;
	
	public void Init(SimMap map)
	{
		this.map = map;
		
		map.mapListener = this;
		gameObject.name = map.id;
		transform.localPosition = Vector3.zero;
		
		mapValues = new Transform[map.sizeX * map.sizeY];
		
		Material mapMaterial = MaterialsFactory.CreateDiffuseColor(map.mapType.color);
		
		for (int x = 0; x < map.sizeX; x++)
		{
			for (int y = 0; y < map.sizeY; y++)
			{
				int val = map.Get(x, y);
				float scale = ((float) val) / ((float) map.mapType.capacity);
				
				SimVector3 pos = map.GetWorldPosition(x, y);
				
				GameObject goCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				goCube.transform.parent = transform;
				goCube.transform.localScale = new Vector3(0.5f, scale, 0.5f);
				goCube.transform.localPosition = new Vector3(pos.x, pos.y + scale * 0.5f, pos.z);
				goCube.renderer.sharedMaterial = mapMaterial;
				mapValues[y * map.sizeX + x] = goCube.transform;
			}
		}
	}
	
	public void OnMapModified (SimMap map, int x, int y, int val)
	{
		float scale = ((float) val) / ((float) map.mapType.capacity);
		
		SimVector3 pos = map.GetWorldPosition(x, y);
		
		mapValues[y * map.sizeX + x].localScale = new Vector3(0.5f, scale, 0.5f);
		mapValues[y * map.sizeX + x].localPosition = new Vector3(pos.x, pos.y + scale * 0.5f, pos.z);
	}
}

