// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "UI/Default Jitter"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		_Jitterness ("Jitterness", Float) = 10
		_ColorJitterness ("Color Jitterness", Float) = 0.1
		_CustomTime ("Custom Time", Float) = 0.0
		_MoveSpeed ("Move Speed", Float) = 50.0

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
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
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
			#include "Random.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
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
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
			};
			
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float _Jitterness;
			float _ColorJitterness;
			float _CustomTime;
			float _MoveSpeed;

			v2f vert(appdata_t IN)
			{
				v2f OUT;

//				float4 worldPos = mul(unity_ObjectToWorld, float4(0, 0, 0, 0));
//				IN.vertex.xy += smoothRand(worldPos.xy + IN.texcoord, _MoveSpeed * _Time[1], _Jitterness);
				IN.vertex.xy += smoothRand(IN.vertex.xy, _CustomTime > 0.0 ? _MoveSpeed * _CustomTime : _MoveSpeed * _Time[1], _Jitterness);

				OUT.worldPosition = IN.vertex;

				OUT.vertex = mul(UNITY_MATRIX_MVP, OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
				#endif

				
				OUT.color = IN.color * _Color;
//				float prevColor = _ColorJitterness * lerp(-1.0, 1.0, rand(float2(OUT.color.r + OUT.color.g + OUT.color.b, prevTime)));
//				float nextColor = _ColorJitterness * lerp(-1.0, 1.0, rand(float2(OUT.color.r + OUT.color.g + OUT.color.b, nextTime)));

//				float amt = lerp(prevColor, nextColor, percent);
//				OUT.color.r = IN.vertex.x / 100;//amt;
//				OUT.color.g = IN.vertex.y / 100;//amt;
//				OUT.color.b = 0;//amt;

				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
				
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
}
