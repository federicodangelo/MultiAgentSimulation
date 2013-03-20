using System;

// # class Matrix4x4:
// Represents a 4x4 matrix. Elements are specified in row order
public struct CSGMatrix4x4
{
	public float this0;
	public float this1;
	public float this2;
	public float this3;
	
	public float this4;
	public float this5;
	public float this6;
	public float this7;
	
	public float this8;
	public float this9;
	public float this10;
	public float this11;
	
	public float this12;
	public float this13;
	public float this14;
	public float this15;
	
	public CSGMatrix4x4 plus(CSGMatrix4x4 m)
	{
		CSGMatrix4x4 r = this;
		
		r.this0 += m.this0;
		r.this1 += m.this1;
		r.this2 += m.this2;
		r.this3 += m.this3;
		r.this4 += m.this4;
		r.this5 += m.this5;
		r.this6 += m.this6;
		r.this7 += m.this7;
		r.this8 += m.this8;
		r.this9 += m.this9;
		r.this10 += m.this10;
		r.this11 += m.this11;
		r.this12 += m.this12;
		r.this13 += m.this13;
		r.this14 += m.this14;
		r.this15 += m.this15;
		
		return r;
	}
	  
	public CSGMatrix4x4 minus(CSGMatrix4x4 m)
	{
		CSGMatrix4x4 r = this;
		
		r.this0 -= m.this0;
		r.this1 -= m.this1;
		r.this2 -= m.this2;
		r.this3 -= m.this3;
		r.this4 -= m.this4;
		r.this5 -= m.this5;
		r.this6 -= m.this6;
		r.this7 -= m.this7;
		r.this8 -= m.this8;
		r.this9 -= m.this9;
		r.this10 -= m.this10;
		r.this11 -= m.this11;
		r.this12 -= m.this12;
		r.this13 -= m.this13;
		r.this14 -= m.this14;
		r.this15 -= m.this15;
		
		return r;
	}
	
	public CSGMatrix4x4 multiply(CSGMatrix4x4 m)
	{
		float m0 = m.this0;
		float m1 = m.this1;
		float m2 = m.this2;
		float m3 = m.this3;
		float m4 = m.this4;
		float m5 = m.this5;
		float m6 = m.this6;
		float m7 = m.this7;
		float m8 = m.this8;
		float m9 = m.this9;
		float m10 = m.this10;
		float m11 = m.this11;
		float m12 = m.this12;
		float m13 = m.this13;
		float m14 = m.this14;
		float m15 = m.this15;
		
		CSGMatrix4x4 result;

		result.this0 = this0*m0 + this1*m4 + this2*m8 + this3*m12;
		result.this1 = this0*m1 + this1*m5 + this2*m9 + this3*m13;
		result.this2 = this0*m2 + this1*m6 + this2*m10 + this3*m14;
		result.this3 = this0*m3 + this1*m7 + this2*m11 + this3*m15;
		result.this4 = this4*m0 + this5*m4 + this6*m8 + this7*m12;
		result.this5 = this4*m1 + this5*m5 + this6*m9 + this7*m13;
		result.this6 = this4*m2 + this5*m6 + this6*m10 + this7*m14;
		result.this7 = this4*m3 + this5*m7 + this6*m11 + this7*m15;
		result.this8 = this8*m0 + this9*m4 + this10*m8 + this11*m12;
		result.this9 = this8*m1 + this9*m5 + this10*m9 + this11*m13;
		result.this10 = this8*m2 + this9*m6 + this10*m10 + this11*m14;
		result.this11 = this8*m3 + this9*m7 + this10*m11 + this11*m15;
		result.this12 = this12*m0 + this13*m4 + this14*m8 + this15*m12;
		result.this13 = this12*m1 + this13*m5 + this14*m9 + this15*m13;
		result.this14 = this12*m2 + this13*m6 + this14*m10 + this15*m14;
		result.this15 = this12*m3 + this13*m7 + this14*m11 + this15*m15;
		
		return result;
	}
	
	public CSGMatrix4x4 clone()
	{
		return this;
	}
	
	// Multiply a CSG.Vector (interpreted as 1 row, 3 column) by this matrix 
	// Fourth element is taken as 1
	public CSGVector3 rightMultiply1x3Vector(CSGVector3 v) 
	{
		float v0 = v.x;
		float v1 = v.y;
		float v2 = v.z;
		float v3 = 1;    
		float x = v0*this0 + v1*this1 + v2*this2 + v3*this3;    
		float y = v0*this4 + v1*this5 + v2*this6 + v3*this7;    
		float z = v0*this8 + v1*this9 + v2*this10 + v3*this11;    
		float w = v0*this12 + v1*this13 + v2*this14 + v3*this15;
		// scale such that fourth element becomes 1:
		if(w != 1)
		{
			float invw=1.0f/w;
			x *= invw;
			y *= invw;
			z *= invw;
		}
		
		return new CSGVector3(x,y,z);
	}
	
	// return the unity matrix
	static public CSGMatrix4x4 unity()
	{
		CSGMatrix4x4 mat = new CSGMatrix4x4();
		
		mat.this0 = 1;
		mat.this5 = 1;
		mat.this10 = 1;
		mat.this15 = 1;
		
		return mat;
	}
	
	// Create a rotation matrix for rotating around the x axis
	static public CSGMatrix4x4 rotationX(float degrees) 
	{
		float radians = degrees * ((float) Math.PI) * (1.0f/180.0f);
		
		float cos = (float) Math.Cos(radians);
		float sin = (float) Math.Sin(radians);
		
		CSGMatrix4x4 result = new CSGMatrix4x4();
		
		result.this0 = 1;
		result.this5 = cos;
		result.this6 = -sin;
		result.this9 = sin;
		result.this10 = cos;
		result.this15 = 1;
		
		/*
		float[] els = new float[] {
			1, 0, 0, 0,
			0, cos, -sin, 0,
			0, sin, cos, 0,
			0, 0, 0, 1
		};
		*/
		
		return result;
	}
	
	// Create a rotation matrix for rotating around the y axis
	static public CSGMatrix4x4 rotationY(float degrees) 
	{
		float radians = degrees * ((float) Math.PI) * (1.0f/180.0f);
		
		float cos = (float) Math.Cos(radians);
		float sin = (float) Math.Sin(radians);
		
		CSGMatrix4x4 result = new CSGMatrix4x4();
		
		result.this0 = cos;
		result.this2 = sin;
		result.this5 = 1;
		result.this8 = -sin;
		result.this10 = cos;
		result.this15 = 1;
		
		/*
		float[] els = new float[] {
			cos, 0, sin, 0,
			0, 1, 0, 0,
			-sin, 0, cos, 0,
			0, 0, 0, 1
		};
		*/
		
		return result;
	}
	
	// Create a rotation matrix for rotating around the z axis
	static public CSGMatrix4x4 rotationZ(float degrees) 
	{
		float radians = degrees * ((float) Math.PI) * (1.0f/180.0f);
		
		float cos = (float) Math.Cos(radians);
		float sin = (float) Math.Sin(radians);
		
		CSGMatrix4x4 result = new CSGMatrix4x4();
		
		result.this0 = cos;
		result.this1 = -sin;
		result.this4 = sin;
		result.this5 = cos;
		result.this10 = 1;
		result.this15 = 1;
		
		/*
		float[] els = new float[] {
			cos, -sin, 0, 0,
			sin, cos, 0, 0,
			0, 0, 1, 0,
			0, 0, 0, 1
		};
		*/
		
		return result;
	}		
	
	// Create an affine matrix for translation:
	static public CSGMatrix4x4 translation(CSGVector3 vec) 
	{
		CSGMatrix4x4 result = new CSGMatrix4x4();
		
		result.this0 = 1;
		result.this3 = vec.x;
		result.this5 = 1;
		result.this7 = vec.y;
		result.this10 = 1;
		result.this11 = vec.z;
		result.this15 = 1;
		
		/*		
		float[] els = new float[] {
			1, 0, 0, vec.x,
			0, 1, 0, vec.y,
			0, 0, 1, vec.z,
			0, 0, 0, 1
		};
		*/
		
		return result;
	}
	
	// Create an affine matrix for scaling:
	static public CSGMatrix4x4 scaling(CSGVector3 vec) 
	{
		CSGMatrix4x4 result = new CSGMatrix4x4();
		
		result.this0 = vec.x;
		result.this5 = vec.y;
		result.this10 = vec.z;
		result.this15 = 1;
		
		/*
		float[] els = new float[] {
			vec.x, 0, 0, 0,
			0, vec.y, 0, 0,
			0, 0, vec.z, 0,
			0, 0, 0, 1
		};
		*/
		
		return result;
	}
}

