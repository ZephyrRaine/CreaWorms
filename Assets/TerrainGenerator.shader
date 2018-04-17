Shader "TerrainGenerator"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Tex("InputTex", 2D) = "white" {}
		_noiseResolution ("Noise Resolution", Float) = 0.5
		_noiseResolutionBlack ("Noise Resolution Black", Float) = 0.8
		_noiseThreshold ("Noise Threshold", Float) = 0.2
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

            float4      _Color;
            sampler2D   _Tex;

			float _noiseResolution;
			float _noiseResolutionBlack = 0.8;
			float _noiseThreshold= 0.2;

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
				float4 colorReturn = tex2D(_Tex, IN.localTexcoord.xy);
				float x = IN.localTexcoord.x;
				float y = IN.localTexcoord.y;
				float4 color = tex2D(_Tex, IN.localTexcoord.xy);

				float value = abs(cnoise(float2(x / _noiseResolution, y / _noiseResolution)+_Time.xx));
				value = max(0, (25 - value * 256) * 8);
				
				if (color.r == 0 && color.g == 0 && color.b == 0)
				{
					float value2 = abs(cnoise(float2((_CustomRenderTextureWidth - x - 1) / _noiseResolutionBlack, y / _noiseResolutionBlack)+_Time.xx));
					value2 = max(0, (25 - value2 * 256) * 8);
					value = (value + value2) / 2.0;
				}
				if (color.r != 1)
				{
					colorReturn = float4(0,0,0,0);
					if (value > _noiseThreshold) 
					{
						colorReturn = float4(255,255,0,1);
					} 
				}

					return colorReturn;
				
            }

		ENDCG
		}
    }
}