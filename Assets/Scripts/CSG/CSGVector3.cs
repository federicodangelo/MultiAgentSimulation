#define DEFINE_UNITY_OPERATIONS
using System;

// # class Vector

// Represents a 3D vector.
// 
// Example usage:
// 
//     new CSG.Vector(1, 2, 3);
//     new CSG.Vector([1, 2, 3]);
//     new CSG.Vector({ x: 1, y: 2, z: 3 });
public struct CSGVector3
{
	public float x;
	public float y;
	public float z;
	
	public CSGVector3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}
	
	public CSGVector3 negated()
	{
		return new CSGVector3(-x, -y, -z);
	}
	
	public CSGVector3 plus(CSGVector3 a)
	{
		return new CSGVector3(x + a.x, y + a.y, z + a.z);
	}

	public CSGVector3 minus(CSGVector3 a)
	{
		return new CSGVector3(x - a.x, y - a.y, z - a.z);
	}

	public CSGVector3 times(float a)
	{
		return new CSGVector3(x * a, y * a, z * a);
	}
	
	public CSGVector3 dividedBy(float a) 
	{
		return new CSGVector3(x / a, y / a, z / a);
 	}
	
	public float dot(CSGVector3 a)
	{
	    return this.x * a.x + this.y * a.y + this.z * a.z;
  	}
	
	public CSGVector3 lerp(CSGVector3 a, float t)
	{
	    return this.plus(a.minus(this).times(t));
	}
	
	public float length()
	{
	    return (float) Math.Sqrt(this.dot(this));
  	}

	public CSGVector3 unit() 
	{
    	return this.dividedBy(this.length());
  	}

	public CSGVector3 cross(CSGVector3 a) 
	{
	    return new CSGVector3(
	      this.y * a.z - this.z * a.y,
	      this.z * a.x - this.x * a.z,
	      this.x * a.y - this.y * a.x
	    );
	}
  
	// Right multiply by a 4x4 matrix (the vector is interpreted as a row vector)
	// Returns a new CSG.Vector
	public CSGVector3 multiply4x4(CSGMatrix4x4 matrix4x4) 
	{
		return matrix4x4.rightMultiply1x3Vector(this);
	}
	
#if DEFINE_UNITY_OPERATIONS
    public static implicit operator CSGVector3(UnityEngine.Vector3 v)
    {
        return new CSGVector3(v.x, v.y, v.z);
    }	
	
    public static implicit operator UnityEngine.Vector3(CSGVector3 v)
    {
        return new UnityEngine.Vector3(v.x, v.y, v.z);
    }	
#endif
}
