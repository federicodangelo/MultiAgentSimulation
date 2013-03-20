Shader "VertexColorSimple" 
{
	SubShader {
	    Pass {
	        ColorMaterial AmbientAndDiffuse
	        Lighting On
	        
	        SetTexture [_MainTex] {
	            constantColor (1,1,1,1)
	            Combine previous * constant DOUBLE, previous * constant
	        } 
    }
	}
}