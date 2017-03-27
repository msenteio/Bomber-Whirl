Shader "Custom/Screen Space Sprites"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}	
		_BackgroundTex("Background Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
			_MoveSpeed("Move Speed", Float) = 50.0
		_MoveRange("Move Range", Float) = 0.1
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
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
#pragma target 2.0
#pragma multi_compile _ PIXELSNAP_ON
#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
#include "UnityCG.cginc"
#include "Random.cginc"

		struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord  : TEXCOORD0;
	};

	fixed4 _Color;
	float _MoveSpeed;
	float _MoveRange;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = UnityObjectToClipPos(IN.vertex);
		OUT.texcoord = IN.texcoord;
		OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
		OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

		return OUT;
	}

	sampler2D _MainTex;
	sampler2D _AlphaTex;
	sampler2D _BackgroundTex;
	fixed4 _Background_TexelSize;

	fixed4 SampleSpriteTexture(float4 vertex, float2 uv)
	{
		float size = 1.0 - _MoveRange;
		float2 v = float2(lerp(_MoveRange, 1.0 - _MoveRange, 1.0 / _ScreenParams.x * vertex.x), 
			lerp(_MoveRange, 1.0 - _MoveRange, 1.0 - (1 / _ScreenParams.y * vertex.y)));

		v += smoothRand(v, _MoveSpeed * _Time[1], _MoveRange);
		
		fixed4 color = tex2D(_BackgroundTex, v);
		color.a = tex2D(_MainTex, uv).a;
		
#if ETC1_EXTERNAL_ALPHA
		// get the color from an external texture (usecase: Alpha support for ETC1 on android)
		color.a = tex2D(_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

		return color;
	}

	fixed4 frag(v2f IN) : SV_Target
	{
		fixed4 c = SampleSpriteTexture(IN.vertex, IN.texcoord) * IN.color;
	c.rgb *= c.a;
	return c;
	}
		ENDCG
	}
	}
}
