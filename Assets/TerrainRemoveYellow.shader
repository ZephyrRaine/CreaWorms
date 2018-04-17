Shader "TerrainRemoveYellow"
{
    Properties
    {
        _Tex("InputTex", 2D) = "white" {}
     }

     SubShader
     {
        Lighting Off
        Blend One Zero
		Pass{
  CGPROGRAM
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCustomRenderTexture.cginc"
			#include "HLSL/ClassicNoise2D.hlsl"

            sampler2D   _Tex;


            float4 frag(v2f_customrendertexture IN) : COLOR
            {
				float4 colorReturn = tex2D(_Tex, IN.localTexcoord.xy);
				if(all(colorReturn == float4(1,1,0,1)))
					return float4(0,0,0,0);
				else
					return colorReturn;
            }

		ENDCG
		}
    }
}