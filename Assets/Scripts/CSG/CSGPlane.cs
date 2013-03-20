using System;
using System.Collections.Generic;

// # class Plane
// Represents a plane in 3D space.
public struct CSGPlane
{
	//CSG.Plane.EPSILON` is the tolerance used by `splitPolygon()` to decide if a
	//point is on the plane.
	private const float EPSILON = 1e-5f;
	
	private const int COPLANAR = 0;
	private const int FRONT = 1;
	private const int BACK = 2;
	private const int SPANNING = 3;
	
	public CSGVector3 normal;
	public float w;
	
	public CSGPlane(CSGVector3 normal, float w)
	{
		this.normal = normal;
		this.w = w;
	}
	
	static public CSGPlane fromPoints(CSGVector3 a, CSGVector3 b, CSGVector3 c) 
	{
		CSGVector3 n = b.minus(a).cross(c.minus(a)).unit();
		
		return new CSGPlane(n, n.dot(a));
	}
	
	public void flip()
	{
		normal = normal.negated();
		w = -w;
	}
	
	// Split `polygon` by this plane if needed, then put the polygon or polygon
	// fragments in the appropriate lists. Coplanar polygons go into either
	// `coplanarFront` or `coplanarBack` depending on their orientation with
	// respect to this plane. Polygons in front or in back of this plane go into
	// either `front` or `back`.
	public void splitPolygon(CSGPolygon polygon, List<CSGPolygon> coplanarFront, List<CSGPolygon> coplanarBack, List<CSGPolygon> front, List<CSGPolygon> back) 
	{
		// Classify each point as well as the entire polygon into one of the above
		// four classes.
		int polygonType = 0;
		int[] types = new int[polygon.vertices.Count];
		
		for (int i = 0; i < polygon.vertices.Count; i++) 
		{
			float t = this.normal.dot(polygon.vertices[i].pos) - this.w;
			int type = (t < -EPSILON) ? BACK : (t > EPSILON) ? FRONT : COPLANAR;
			polygonType |= type;
			types[i] = type;
		}
		
		// Put the polygon in the correct list, splitting it when necessary.
		switch (polygonType) 
		{
			case COPLANAR:
				(this.normal.dot(polygon.plane.normal) > 0 ? coplanarFront : coplanarBack).Add(polygon);
				break;
			
			case FRONT:
				front.Add(polygon);
				break;
			
			case BACK:
				back.Add(polygon);
				break;
			
			case SPANNING:
			{
				List<CSGVertex> f = new List<CSGVertex>();
				List<CSGVertex> b = new List<CSGVertex>();
				for (int i = 0; i < polygon.vertices.Count; i++) 
				{
					int j = (i + 1) % polygon.vertices.Count;
					int ti = types[i];
					int tj = types[j];
					CSGVertex vi = polygon.vertices[i];
					CSGVertex vj = polygon.vertices[j];
				
					if (ti != BACK) 
						f.Add(vi);
				
					if (ti != FRONT) 
						b.Add(ti != BACK ? vi.clone() : vi);
					
					if ((ti | tj) == SPANNING) 
					{
						float t = (this.w - this.normal.dot(vi.pos)) / this.normal.dot(vj.pos.minus(vi.pos));
						CSGVertex v = vi.interpolate(vj, t);
						f.Add(v);
						b.Add(v.clone());
					}
				}
				if (f.Count >= 3) 
					front.Add(new CSGPolygon(f, polygon.shared));
			
				if (b.Count >= 3) 
					back.Add(new CSGPolygon(b, polygon.shared));
				break;
			}
		}
	}
}

