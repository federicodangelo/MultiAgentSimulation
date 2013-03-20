using System;
using System.Collections.Generic;

// Constructive Solid Geometry (CSG) is a modeling technique that uses Boolean
// operations like union and intersection to combine 3D solids. This library
// implements CSG operations on meshes elegantly and concisely using BSP trees,
// and is meant to serve as an easily understandable implementation of the
// algorithm. All edge cases involving overlapping coplanar polygons in both
// solids are correctly handled.
// 
// Example usage:
// 
//     var cube = CSG.cube();
//     var sphere = CSG.sphere({ radius: 1.3 });
//     var polygons = cube.subtract(sphere).toPolygons();
// 
// ## Implementation Details
// 
// All CSG operations are implemented in terms of two functions, `clipTo()` and
// `invert()`, which remove parts of a BSP tree inside another BSP tree and swap
// solid and empty space, respectively. To find the union of `a` and `b`, we
// want to remove everything in `a` inside `b` and everything in `b` inside `a`,
// then combine polygons from `a` and `b` into one solid:
// 
//     a.clipTo(b);
//     b.clipTo(a);
//     a.build(b.allPolygons());
// 
// The only tricky part is handling overlapping coplanar polygons in both trees.
// The code above keeps both copies, but we need to keep them in one tree and
// remove them in the other tree. To remove them from `b` we can clip the
// inverse of `b` against `a`. The code for union now looks like this:
// 
//     a.clipTo(b);
//     b.clipTo(a);
//     b.invert();
//     b.clipTo(a);
//     b.invert();
//     a.build(b.allPolygons());
// 
// Subtraction and intersection naturally follow from set operations. If
// union is `A | B`, subtraction is `A - B = ~(~A | B)` and intersection is
// `A & B = ~(~A | ~B)` where `~` is the complement operator.
// 
// ## License
// 
// Copyright (c) 2011 Evan Wallace (http://madebyevan.com/), under the MIT license.
// Parts Copyright (c) 2012 Joost Nieuwenhuijse (joost@newhouse.nl) under the MIT license.

// # class CSG

// Holds a binary space partition tree representing a 3D solid. Two solids can
// be combined using the `union()`, `subtract()`, and `intersect()` methods.

public class CSG
{
	private List<CSGPolygon> polygons = new List<CSGPolygon>();

	// Construct a CSG solid from a list of `CSG.Polygon` instances.
	static public CSG fromPolygons(List<CSGPolygon> polygons) 
	{
	  CSG csg = new CSG();
	  csg.polygons = polygons;
	  return csg;
	}
	
	public CSG clone()
	{
	    CSG csg = new CSG();
		foreach(CSGPolygon polygon in this.polygons)
			csg.polygons.Add(polygon.clone());
	    return csg;
	}
	
	public List<CSGPolygon> toPolygons() 
	{
    	return this.polygons;
  	}

	// Return a new CSG solid representing space in either this solid or in the
	// solid `csg`. Neither this solid nor the solid `csg` are modified.
	// 
	//     A.union(B)
	// 
	//     +-------+            +-------+
	//     |       |            |       |
	//     |   A   |            |       |
	//     |    +--+----+   =   |       +----+
	//     +----+--+    |       +----+       |
	//          |   B   |            |       |
	//          |       |            |       |
	//          +-------+            +-------+
	// 
	public CSG union(CSG csg) 
	{
		CSGNode a = new CSGNode(this.clone().polygons);
		CSGNode b = new CSGNode(csg.clone().polygons);
		a.clipTo(b);
		b.clipTo(a);
		b.invert();
		b.clipTo(a);
		b.invert();

		List<CSGPolygon> result = a.allPolygons();
		result.AddRange(b.allPolygons());

		return CSG.fromPolygons(result);
	}
	
	// Return a new CSG solid representing space in this solid but not in the
	// solid `csg`. Neither this solid nor the solid `csg` are modified.
	// 
	//     A.subtract(B)
	// 
	//     +-------+            +-------+
	//     |       |            |       |
	//     |   A   |            |       |
	//     |    +--+----+   =   |    +--+
	//     +----+--+    |       +----+
	//          |   B   |
	//          |       |
	//          +-------+
	// 
	public CSG subtract(CSG csg) 
	{
		CSGNode a = new CSGNode(this.clone().polygons);
		CSGNode b = new CSGNode(csg.clone().polygons);
		a.invert();
		a.clipTo(b);
		b.clipTo(a);
		b.invert();
		b.clipTo(a);
		b.invert();
		a.addPolygons(b.allPolygons());
		a.invert();
		
		return CSG.fromPolygons(a.allPolygons());
	}

	// Return a new CSG solid representing space both this solid and in the
	// solid `csg`. Neither this solid nor the solid `csg` are modified.
	// 
	//     A.intersect(B)
	// 
	//     +-------+
	//     |       |
	//     |   A   |
	//     |    +--+----+   =   +--+
	//     +----+--+    |       +--+
	//          |   B   |
	//          |       |
	//          +-------+
	// 
	public CSG intersect(CSG csg) 
	{
		CSGNode a = new CSGNode(this.clone().polygons);
		CSGNode b = new CSGNode(csg.clone().polygons);
		a.invert();
		b.clipTo(a);
		b.invert();
		a.clipTo(b);
		b.clipTo(a);
		a.addPolygons(b.allPolygons());
		a.invert();
		return CSG.fromPolygons(a.allPolygons());
	}

	// Return a new CSG solid with solid and empty space switched. This solid is
	// not modified.
	public CSG inverse() 
	{
		CSG csg = this.clone();
		foreach(CSGPolygon p in csg.polygons)
			p.flip();
		return csg;
	}
  
	// Affine transformation of CSG object. Returns a new CSG object
	public CSG transform(CSGMatrix4x4 matrix4x4) 
	{
		List<CSGPolygon> newpolygons = new List<CSGPolygon>();
		foreach(CSGPolygon p in this.polygons)
			newpolygons.Add(p.transform(matrix4x4));
		return CSG.fromPolygons(newpolygons);  
	}
	
	public CSG translate(CSGVector3 v) 
	{
		return this.transform(CSGMatrix4x4.translation(v));
	}
	
	public CSG scale(CSGVector3 v) 
	{
		return this.transform(CSGMatrix4x4.scaling(v));
	}
	
	public CSG rotateX(float deg) 
	{
		return this.transform(CSGMatrix4x4.rotationX(deg));
	}
	
	public CSG rotateY(float deg) 
	{
		return this.transform(CSGMatrix4x4.rotationY(deg));
	}
	
	public CSG rotateZ(float deg) 
	{
		return this.transform(CSGMatrix4x4.rotationZ(deg));
	}

	// Construct an axis-aligned solid cuboid. Optional parameters are `center` and
	// `radius`, which default to `[0, 0, 0]` and `[1, 1, 1]`. The radius can be
	// specified using a single number or a list of three numbers, one for each axis.
	static public CSG cube(CSGVector3 center, CSGVector3 halfSize, CSGShared shared) 
	{
		int[][][] data = new int[][][] {
			new int[][] {new int[] { 0, 4, 6, 2}, new int[] {-1, 0, 0}},
			new int[][] {new int[] { 1, 3, 7, 5}, new int[] {+1, 0, 0}},
			new int[][] {new int[] { 0, 1, 5, 4}, new int[] {0, -1, 0}},
			new int[][] {new int[] { 2, 6, 7, 3}, new int[] {0, +1, 0}},
			new int[][] {new int[] { 0, 2, 3, 1}, new int[] {0, 0, -1}},
			new int[][] {new int[] { 4, 5, 7, 6}, new int[] {0, 0, +1}}
		};
		
		List<CSGPolygon> polygons = new List<CSGPolygon>();
		
		foreach(int[][] info in data)
		{
			List<CSGVertex> vertices = new List<CSGVertex>();
			
			foreach(int i in info[0])
			{
				CSGVector3 pos = new CSGVector3(
					center.x + halfSize.x * (2 * ((i & 1) != 0 ? 1 : 0) - 1),
			        center.y + halfSize.y * (2 * ((i & 2) != 0 ? 1 : 0) - 1),
			        center.z + halfSize.z * (2 * ((i & 4) != 0 ? 1 : 0) - 1)
				);
				
				vertices.Add(new CSGVertex(pos, new CSGVector3(info[1][0], info[1][1], info[1][2]))); 
			}
			
			polygons.Add(new CSGPolygon(vertices, shared));
		}
		
		return CSG.fromPolygons(polygons);
	}
	
	// Construct a solid sphere. Optional parameters are `center`, `radius`,
	// `slices`, and `stacks`, which default to `[0, 0, 0]`, `1`, `16`, and `8`.
	// The `slices` and `stacks` parameters control the tessellation along the
	// longitude and latitude directions.
	// 
	// Example usage:
	// 
	//     var sphere = CSG.sphere({
	//       center: [0, 0, 0],
	//       radius: 1,
	//       slices: 16,
	//       stacks: 8
	//     });
	static public CSG sphere(CSGVector3 center, float radius, int slices, int stacks, CSGShared shared) 
	{
		List<CSGPolygon> polygons = new List<CSGPolygon>();
		
		for (float i = 0; i < slices; i++) 
		{
			for (float j = 0; j < stacks; j++) 
			{
				List<CSGVertex> vertices = new List<CSGVertex>();
				sphereVertex(center, radius, i / slices, j / stacks, vertices);
				
				if (j > 0) 
					sphereVertex(center, radius, (i + 1) / slices, j / stacks, vertices);
				
				if (j < stacks - 1) 
					sphereVertex(center, radius, (i + 1) / slices, (j + 1) / stacks, vertices);
				
				sphereVertex(center, radius, i / slices, (j + 1) / stacks, vertices);
				
				polygons.Add(new CSGPolygon(vertices, shared));
			}
		}
		
		return CSG.fromPolygons(polygons);
	}
	
	
	static private void sphereVertex(CSGVector3 center, float radius, float theta, float phi, List<CSGVertex> vertices) 
	{
		theta *= ((float) Math.PI) * 2;
		phi *= ((float) Math.PI);
		CSGVector3 dir = new CSGVector3(
			(float) Math.Cos(theta) * (float) Math.Sin(phi),
			(float) Math.Cos(phi),
			(float) Math.Sin(theta) * (float) Math.Sin(phi)
		);
		vertices.Add(new CSGVertex(center.plus(dir.times(radius)), dir));
	}	
	
	// Construct a solid cylinder. Optional parameters are `start`, `end`,
	// `radius`, and `slices`, which default to `[0, -1, 0]`, `[0, 1, 0]`, `1`, and
	// `16`. The `slices` parameter controls the tessellation.
	// 
	// Example usage:
	// 
	//     var cylinder = CSG.cylinder({
	//       start: [0, -1, 0],
	//       end: [0, 1, 0],
	//       radius: 1,
	//       slices: 16
	//     });
	static public CSG cylinder(CSGVector3 s, CSGVector3 e, float radius, int slices, CSGShared shared) 
	{
		CSGVector3 ray = e.minus(s);
		CSGVector3 axisZ = ray.unit();
		bool isY = (Math.Abs(axisZ.y) > 0.5f);
		
		CSGVector3 axisX = new CSGVector3(isY ? 1 : 0, !isY ? 1 : 0, 0).cross(axisZ).unit();
		CSGVector3 axisY = axisX.cross(axisZ).unit();
		
		CSGVertex start = new CSGVertex(s, axisZ.negated());
		CSGVertex end = new CSGVertex(e, axisZ.unit());
		
		List<CSGPolygon> polygons = new List<CSGPolygon>();
		
		for (float i = 0; i < slices; i++) 
		{
			float t0 = i / slices;
			float t1 = (i + 1) / slices;
			
			//cylinderPoint(s, ray, radius, axisX, axisY, axisZ, 
			
			polygons.Add(
				new CSGPolygon(
					new CSGVertex[] { 
						start, 
						cylinderPoint(s, ray, radius, axisX, axisY, axisZ, 0, t0, -1), 
						cylinderPoint(s, ray, radius, axisX, axisY, axisZ, 0, t1, -1)}, shared));
			
			polygons.Add(
				new CSGPolygon(
					new CSGVertex[] {
						cylinderPoint(s, ray, radius, axisX, axisY, axisZ, 0, t1, 0), 
						cylinderPoint(s, ray, radius, axisX, axisY, axisZ, 0, t0, 0), 
						cylinderPoint(s, ray, radius, axisX, axisY, axisZ, 1, t0, 0), 
						cylinderPoint(s, ray, radius, axisX, axisY, axisZ, 1, t1, 0)}, shared));
			
			polygons.Add(
				new CSGPolygon(
					new CSGVertex[] {
						end, 
						cylinderPoint(s, ray, radius, axisX, axisY, axisZ, 1, t1, 1), 
						cylinderPoint(s, ray, radius, axisX, axisY, axisZ, 1, t0, 1)}, shared));
		}
		
		return CSG.fromPolygons(polygons);
	}
	
	static private CSGVertex cylinderPoint(CSGVector3 s, CSGVector3 ray, float r, CSGVector3 axisX, CSGVector3 axisY, CSGVector3 axisZ, float stack, float slice, float normalBlend)
	{
		float angle = slice * ((float) Math.PI) * 2;
		CSGVector3 outv = axisX.times((float) Math.Cos(angle)).plus(axisY.times((float) Math.Sin(angle)));
		CSGVector3 pos = s.plus(ray.times(stack)).plus(outv.times(r));
		CSGVector3 normal = outv.times(1 - Math.Abs(normalBlend)).plus(axisZ.times(normalBlend));
		
		return new CSGVertex(pos, normal);
	}
}







