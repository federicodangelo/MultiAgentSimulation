using UnityEngine;
using System.Collections.Generic;

public class TestCSG : MonoBehaviour
{
	public void Start()
	{
		Test1();
		Test2();
	}

	private void Test1()
	{
		int resolution = 32;

		float startTime = Time.realtimeSinceStartup;

		CSG cylinder = CSG.cylinder(new Vector3(0, 0, 0), new Vector3(0, 1, 0), 0.25f, resolution, new CSGShared(0, 0, 1));
		cylinder = cylinder.union(cylinder.clone().rotateX(90).translate(new Vector3(0, 0.5f, -0.5f)));
		cylinder = cylinder.union(cylinder.clone().rotateZ(90).translate(new Vector3(0.5f, 0.5f, 0)));
		
		CSG sphere1 = CSG.sphere(new Vector3(0, 0, 0), 1.0f, resolution, resolution / 2, new CSGShared(0, 1, 0));
		
		CSG final = sphere1.subtract(cylinder.scale(new Vector3(2.0f, 2.0f, 2.0f)));

		float endTime = Time.realtimeSinceStartup;
		 
		Debug.Log("Test1 - Build Time: " + ((int) ((endTime - startTime) * 1000)) + " ms");

		AddMesh(CreateMesh(final));
	}
	
	private void Test2()
	{
		int resolution = 32;
		
		float startTime = Time.realtimeSinceStartup;
		
		CSG cube1 = CSG.cube(new Vector3(0.5f, 0, 0), new Vector3(1, 1, 1), new CSGShared(1, 0, 0));
		CSG cube2 = CSG.cube(new Vector3(0, 0.5f, 0), new Vector3(1, 1, 1), new CSGShared(0, 1, 0));
		CSG cube3 = CSG.cube(new Vector3(0, 0, 0.5f), new Vector3(1, 1, 1), new CSGShared(0, 0, 1));

		CSG final = cube1.union(cube2).union(cube3);
		
		float endTime = Time.realtimeSinceStartup;
		
		Debug.Log("Test2 - Build Time: " + ((int) ((endTime - startTime) * 1000)) + " ms");
		
		AddMesh(CreateMesh(final)).transform.localPosition += Vector3.forward * 3.0f;
	}

	private GameObject AddMesh(Mesh mesh)
	{
		GameObject newGO = new GameObject();
		newGO.transform.parent = transform;
		newGO.transform.localPosition = Vector3.zero;
		newGO.transform.localRotation = Quaternion.identity;
		
		newGO.AddComponent<MeshFilter>().sharedMesh = mesh;
		newGO.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("VertexColorSimple"));

		return newGO;
	}
	
	private Mesh CreateMesh(CSG csg)
	{
		Mesh mesh = new Mesh();
		
		List<Vector3> meshVertices = new List<Vector3>();
		List<Vector3> meshNormals = new List<Vector3>();
		List<Color32> meshColors = new List<Color32>();
		List<int> meshTriangles = new List<int>();
		
		List<int> indices = new List<int>();
		
		int lastIndex = 0;
		
		List<CSGPolygon> polygons = csg.toPolygons();
		
		foreach(CSGPolygon polygon in polygons)
		{
			indices.Clear();
			
			foreach(CSGVertex vertex in polygon.vertices)
			{
				meshVertices.Add(new Vector3(vertex.pos.x, vertex.pos.y, vertex.pos.z));
				meshNormals.Add(new Vector3(vertex.normal.x, vertex.normal.y, vertex.normal.z));
				meshColors.Add(new Color(polygon.shared.color.x, polygon.shared.color.y, polygon.shared.color.z));
				indices.Add(lastIndex++);
			}
			
			for (int i = 2; i < indices.Count; i++) 
			{
				meshTriangles.Add(indices[0]);
				meshTriangles.Add(indices[i - 1]);
				meshTriangles.Add(indices[i]);
			}
		}
		
		mesh.vertices = meshVertices.ToArray();
		mesh.normals = meshNormals.ToArray();
		mesh.colors32 = meshColors.ToArray();
		mesh.triangles = meshTriangles.ToArray();

		Debug.Log("Triangles: " + meshTriangles.Count / 3);
		
		return mesh;
	}
}
