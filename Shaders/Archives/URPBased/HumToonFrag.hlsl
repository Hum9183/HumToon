#ifndef HUM_TOON_FRAG_INCLUDED
#define HUM_TOON_FRAG_INCLUDED

#include "HumToonTemp.hlsl"

#include "../../../ShaderLibrary/InitializeInputData.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

#if defined(LOD_FADE_CROSSFADE)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#endif

#if defined(DEBUG_DISPLAY)
    #include "../../../ShaderLibrary/DebugOverrideOutputColor.hlsl"
#endif

void frag(
    Varyings input
    , out half4 outColor : SV_Target0
#ifdef _WRITE_RENDERING_LAYERS
    , out float4 outRenderingLayers : SV_Target1
#endif
)
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

#ifdef LOD_FADE_CROSSFADE
    LODFadeCrossFade(input.positionCS);
#endif

    half2 uv0 = input.uv;

    InputData inputData;
    InitializeInputData(input, inputData);
    SETUP_DEBUG_TEXTURE_DATA(inputData, uv0, _BaseMap);

    half4 finalColor;
    finalColor     = CalcBaseColor(uv0);
    // finalColor.rgb = CalcShade(finalColor.rgb, input.normalWS, float3(0,0,1));

#ifdef _DBUFFER // BaseColorに載せたほうが良さそう
    ApplyDecalToBaseColor(input.positionCS, color);
#endif

#if defined(DEBUG_DISPLAY)
    finalColor = DebugOverrideOutputColor(inputData, finalColor);
#endif

#if defined(_SCREEN_SPACE_OCCLUSION) && !defined(_SURFACE_TYPE_TRANSPARENT)
    float2 normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
    AmbientOcclusionFactor aoFactor = GetScreenSpaceAmbientOcclusion(normalizedScreenSpaceUV);
    finalColor.rgb *= aoFactor.directAmbientOcclusion;
#endif

#if defined(_FOG_FRAGMENT)
#if (defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2))
    float viewZ = -input.fogCoord;
    float nearToFarZ = max(viewZ - _ProjectionParams.y, 0);
    half fogFactor = ComputeFogFactorZ0ToFar(nearToFarZ);
#else
    half fogFactor = 0;
#endif
#else
    half fogFactor = input.fogCoord;
#endif
    finalColor.rgb = MixFog(finalColor.rgb, fogFactor);

    finalColor.a = OutputAlpha(finalColor.a, IsSurfaceTypeTransparent(_SurfaceType));

    outColor = finalColor;

#ifdef _WRITE_RENDERING_LAYERS
    uint renderingLayers = GetMeshRenderingLayer();
    outRenderingLayers = float4(EncodeMeshRenderingLayer(renderingLayers), 0, 0, 0);
#endif
}

#endif
