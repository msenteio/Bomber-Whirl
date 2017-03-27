// Upgrade NOTE: commented out 'float3 _WorldSpaceCameraPos', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_ShadowFadeCenterAndType', a built-in variable
// Upgrade NOTE: commented out UNITY_DECLARE_TEX2D unity_DynamicLightmap, a built-in variable
// Upgrade NOTE: commented out UNITY_DECLARE_TEX2D unity_Lightmap, a built-in variable
// Upgrade NOTE: commented out UNITY_DECLARE_TEX2D_NOSAMPLER unity_LightmapInd, a built-in variable
// Upgrade NOTE: replaced 'glstate_matrix_invtrans_modelview0' with 'UNITY_MATRIX_IT_MV'
// Upgrade NOTE: replaced 'glstate_matrix_modelview0' with 'UNITY_MATRIX_MV'
// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'glstate_matrix_projection' with 'UNITY_MATRIX_P'
// Upgrade NOTE: replaced 'glstate_matrix_transpose_modelview0' with 'UNITY_MATRIX_T_MV'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced '_World2Shadow' with 'unity_WorldToShadow[0]'
// Upgrade NOTE: replaced '_World2Shadow1' with 'unity_WorldToShadow[1]'
// Upgrade NOTE: replaced '_World2Shadow2' with 'unity_WorldToShadow[2]'
// Upgrade NOTE: replaced '_World2Shadow3' with 'unity_WorldToShadow[3]'
// Upgrade NOTE: replaced 'unity_World2Shadow' with 'unity_WorldToShadow'

#ifndef ELEVEN_RANDOM_SHADER_VARIABLES_INCLUDED
#define ELEVEN_RANDOM_SHADER_VARIABLES_INCLUDED

float rand(float3 co)
{
	return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
}

float rand(float2 co) {
	return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
}

float rand(float x) {
	return rand(float2(x, _Time[1]));
}

float2 smoothRand(float2 co, float time, float moveRange) {
	float halfRange = 0.5 * moveRange;
	float prevTime = floor(time);
	float nextTime = floor(time + 1);
	float percent = time % 1.0;

	float2 prevPos, nextPos;
	prevPos.x = lerp(-halfRange, halfRange, rand(float2(co.x + 3 * co.y, prevTime)));
	prevPos.y = lerp(-halfRange, halfRange, rand(float2(co.x + 7 * co.y, prevTime)));
	nextPos.x = lerp(-halfRange, halfRange, rand(float2(co.x + 3 * co.y, nextTime)));
	nextPos.y = lerp(-halfRange, halfRange, rand(float2(co.x + 7 * co.y, nextTime)));

	return float2(lerp(prevPos.x, nextPos.x, percent), lerp(prevPos.y, nextPos.y, percent));
}

#endif
