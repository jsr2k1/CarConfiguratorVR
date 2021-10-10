Shader "Custom/Overdraw"
{
	Properties
	{
		_Color ("Main Color", Color) = (0.1, 0.04, 0.02, 0)
		_MainTex ("Base", 2D) = "white" {}
	}

	SubShader
	{
		Fog { Mode Off }
		ZWrite Off
		ZTest Always
		Blend One One // additive blending

		Pass
		{
			SetTexture[_MainTex]
			{
			
				constantColor [_Color]
				combine constant, texture
			}
		}
	}
}