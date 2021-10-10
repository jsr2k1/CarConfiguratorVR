Shader "FX/Projector Multiply Offset" {
    Properties {
        _ShadowTex ("Cookie", 2D) = "gray" { TexGen ObjectLinear }
//        _FalloffTex ("FallOff", 2D) = "white" { TexGen ObjectLinear }
        _Tint ("Offset", Color) = (0,0,0,0)
    }
 
    Subshader {
	    Tags {"Queue" = "Transparent" }
        Pass {
            ZWrite Off
            Offset 0, -1
 
            Fog { Color (1, 1, 1) }
            AlphaTest Greater 0
            ZTest LEqual
            ColorMask RGB
            Blend DstColor Zero
            SetTexture [_ShadowTex] {
                combine texture, ONE - texture
                Matrix [_Projector]
            }
/*
            SetTexture [_FalloffTex] {
                constantColor (1,1,1,0)
                combine previous lerp (texture) constant
                Matrix [_ProjectorClip]
            }
*/
            SetTexture [_FalloffTex] { // add offset
                constantColor [_Tint]
                combine previous + constant
            }

        }
    }
}