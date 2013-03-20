using UnityEngine;

public class SimUnityUtils
{
	static public Color32 ConvertColor(uint color)
	{
		return new Color32(
			(byte) ((color >> 16) & 0xFF),
			(byte) ((color >> 8) & 0xFF),
			(byte) ((color >> 0) & 0xFF), 
			255);
	}
}


