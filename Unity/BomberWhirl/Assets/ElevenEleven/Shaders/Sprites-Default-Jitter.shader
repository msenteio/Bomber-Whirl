Shader "Custom/Sprites Jitter"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0

		_Jitterness ("Jitterness", Float) = 10
		_ColorJitterness ("Color Jitterness", Float) = 0.1
		_CustomTime ("Custom Time", Float) = 0.0
		_MoveSpeed ("Move Speed", Float) = 50.0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma shader_feature ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;

			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float _Jitterness;
			float _ColorJitterness;
			float _CustomTime;
			float _MoveSpeed;

			float rand(float3 co)
			{
				return frac(sin(dot(co.xyz ,float3(12.9898,78.233,45.5432))) * 43758.5453);
			}

			float rand(float2 co){
    			return frac(sin(dot(co.xy, float2(12.9898,78.233))) * 43758.5453);
			}

			float rand(float x) {
				return rand(float2(x, _Time[1]));
			}

			v2f vert(appdata_t IN)
			{
				v2f OUT;

				float time = _MoveSpeed * (_CustomTime > 0 ? _CustomTime : _Time[1]);
				float prevTime = floor(time);
				float nextTime = floor(time + 1);
				float percent = time % 1.0;

				float2 prevPos, nextPos;
				prevPos.x = _Jitterness * lerp(-1.0, 1.0, rand(float2(IN.vertex.x + 3 * IN.vertex.y, prevTime)));
				prevPos.y = _Jitterness * lerp(-1.0, 1.0, rand(float2(IN.vertex.x + 7 * IN.vertex.y, prevTime)));
				nextPos.x = _Jitterness * lerp(-1.0, 1.0, rand(float2(IN.vertex.x + 3 * IN.vertex.y, nextTime)));
				nextPos.y = _Jitterness * lerp(-1.0, 1.0, rand(float2(IN.vertex.x + 7 * IN.vertex.y, nextTime)));
				IN.vertex.x += 0.5 * lerp(prevPos.x, nextPos.x, percent);
				IN.vertex.y += 0.5 * lerp(prevPos.y, nextPos.y, percent);

				IN.vertex.x += _Jitterness * cos(_Time[0] * 20 + IN.vertex.x);
				IN.vertex.y += _Jitterness * sin(_Time[0] * 20 + IN.vertex.x);

				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				OUT.color = IN.color * _Color;
				float prevColor = _ColorJitterness * lerp(-1.0, 1.0, rand(float2(OUT.color.r + OUT.color.g + OUT.color.b, prevTime)));
				float nextColor = _ColorJitterness * lerp(-1.0, 1.0, rand(float2(OUT.color.r + OUT.color.g + OUT.color.b, nextTime)));


				float amt = lerp(prevColor, nextColor, percent);
				OUT.color.r += amt;
				OUT.color.g += amt;
				OUT.color.b += amt;

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				color.a = tex2D (_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
