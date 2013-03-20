using System;

//Data shared between polygons
public class CSGShared
{
	public CSGVector3 color = new CSGVector3(1, 1, 1);
	
	public CSGShared()
	{
	}
	
	public CSGShared(float r, float g, float b)
	{
		this.color = new CSGVector3(r, g, b);
	}
}
