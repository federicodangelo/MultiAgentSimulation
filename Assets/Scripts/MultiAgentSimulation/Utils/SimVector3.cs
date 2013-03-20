#define DEFINE_UNITY_OPERATIONS
using System;

public struct SimVector3
{
	public float x;
	public float y;
	public float z;
	
	public SimVector3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}
	
	public float Len
	{
		get { return (float) Math.Sqrt(x * x + y * y + z * z); }
	}

	public float LenSqrt
	{
		get { return x * x + y * y + z * z; }
	}
	
	static public SimVector3 Zero
	{
		get { return new SimVector3(); }
	}
	
	public SimVector3 Normalized
	{
		get
		{
			SimVector3 norm = this;
			float len = this.Len;
			if (len > float.Epsilon)
				norm /= len;
			else
				norm = SimVector3.Zero;
			return norm;
		}
	}
	
	public static SimVector3 operator +(SimVector3 v1, SimVector3 v2)
	{
		return new SimVector3(
			v1.x + v2.x, 
			v1.y + v2.y, 
			v1.z + v2.z);
	}

	public static SimVector3 operator -(SimVector3 v1, SimVector3 v2)
	{
		return new SimVector3(
			v1.x - v2.x, 
			v1.y - v2.y, 
			v1.z - v2.z);
	}

	public static SimVector3 operator *(SimVector3 v1, float n)
	{
		return new SimVector3(
			v1.x * n, 
			v1.y * n, 
			v1.z * n);
	}

	public static SimVector3 operator /(SimVector3 v1, float n)
	{
		return new SimVector3(
			v1.x / n, 
			v1.y / n, 
			v1.z / n);
	}
	
#if DEFINE_UNITY_OPERATIONS
    public static implicit operator SimVector3(UnityEngine.Vector3 v)
    {
        return new SimVector3(v.x, v.y, v.z);
    }	
	
    public static implicit operator UnityEngine.Vector3(SimVector3 v)
    {
        return new UnityEngine.Vector3(v.x, v.y, v.z);
    }	
#endif
}

