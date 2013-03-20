using UnityEngine;
using System.Collections.Generic;

public class MaterialsFactory
{
	static private Dictionary<uint, Material> diffuseMaterials = new Dictionary<uint, Material>();
	
	public static Material CreateDiffuseColor(uint color)
	{
		Material material;
		
		if (!diffuseMaterials.ContainsKey(color))
		{
			material = new Material(Shader.Find("Diffuse"));
			material.color = SimUnityUtils.ConvertColor(color);
		}
		else
		{
			material = diffuseMaterials[color];
		}
		
		return material;
	}
}


