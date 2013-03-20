using System;
using System.Collections.Generic;

// # class Polygon

// Represents a convex polygon. The vertices used to initialize a polygon must
// be coplanar and form a convex loop. They do not have to be `CSG.Vertex`
// instances but they must behave similarly (duck typing can be used for
// customization).
// 
// Each convex polygon has a `shared` property, which is shared between all
// polygons that are clones of each other or were split from the same polygon.
// This can be used to define per-polygon properties (such as surface color).
public class CSGPolygon
{
	public List<CSGVertex> vertices;
	public CSGShared shared;
	public CSGPlane plane;
	
	public CSGPolygon(CSGVertex[] verticesArray, CSGShared shared)
	{
		this.vertices = new List<CSGVertex>(verticesArray);

		this.shared = shared;

		this.plane = CSGPlane.fromPoints(vertices[0].pos, vertices[1].pos, vertices[2].pos);
	}
	
	public CSGPolygon(List<CSGVertex> vertices, CSGShared shared)
	{
		this.vertices = vertices;

		this.shared = shared;

		this.plane = CSGPlane.fromPoints(vertices[0].pos, vertices[1].pos, vertices[2].pos);
	}
	
	public CSGPolygon clone()
	{
		List<CSGVertex> newVertices = new List<CSGVertex>(vertices.Count);

		for (int i = 0; i < vertices.Count; i++)
			newVertices.Add(vertices[i].clone());
		
	    return new CSGPolygon(newVertices, shared);
  	}
	
	public void flip()
	{
		vertices.Reverse();

		for (int i = 0; i < vertices.Count; i++)
			vertices[i].flip();

		plane.flip();
	}
	
	// Affine transformation of polygon. Returns a new CSG.Polygon
	public CSGPolygon transform(CSGMatrix4x4 matrix4x4)
	{
		List<CSGVertex> newVertices = new List<CSGVertex>(vertices.Count);

		for (int i = 0; i < vertices.Count; i++)
			newVertices.Add(vertices[i].transform(matrix4x4));

		return new CSGPolygon(newVertices, shared);
	}
}
