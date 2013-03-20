using System;

// # class Vertex

// Represents a vertex of a polygon. Use your own vertex class instead of this
// one to provide additional features like texture coordinates and vertex
// colors. Custom vertex classes need to provide a `pos` property and `clone()`,
// `flip()`, and `interpolate()` methods that behave analogous to the ones
// defined by `CSG.Vertex`. This class provides `normal` so convenience
// functions like `CSG.sphere()` can return a smooth vertex normal, but `normal`
// is not used anywhere else.

public class CSGVertex
{
	public CSGVector3 pos;
	public CSGVector3 normal;
	
	public CSGVertex(CSGVector3 pos, CSGVector3 normal)
	{
		this.pos = pos;
		this.normal = normal;
	}
	
	public CSGVertex clone()
	{
		return new CSGVertex(pos, normal);
	}
	

	public void flip()
	{
		normal = normal.negated();
	}
	
	// Create a new vertex between this vertex and `other` by linearly
	// interpolating all properties using a parameter of `t`. Subclasses should
	// override this to interpolate additional properties.
	public CSGVertex interpolate(CSGVertex other, float t) 
	{
		return new CSGVertex(
			this.pos.lerp(other.pos, t),
			this.normal.lerp(other.normal, t)
		);
	}

	// Affine transformation of vertex. Returns a new CSG.Vertex
	public CSGVertex transform(CSGMatrix4x4 matrix4x4) 
	{
		CSGVector3 newpos = this.pos.multiply4x4(matrix4x4);
		CSGVector3 posPlusNormal = this.pos.plus(this.normal);
		CSGVector3 newPosPlusNormal = posPlusNormal.multiply4x4(matrix4x4);
		CSGVector3 newnormal = newPosPlusNormal.minus(newpos).unit();
		
		return new CSGVertex(newpos, newnormal);  
	}
}
