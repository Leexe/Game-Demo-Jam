Shader "Custom/CandleBlackout"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        _Center("Center", Vector) = (0, 0, 0, 0)
        _Distance("Distance", Float) = 50
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            ZWrite Off
            ZTest Always
            Blend DstColor Zero // multiplicative

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                float4 _Center;
                float _Distance;
            CBUFFER_END

            // VERT

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            // FRAG

            float3 AbsoluteWorldPos(float2 screenUV)
            {
                float depth = SampleSceneDepth(screenUV);
                return ComputeWorldSpacePosition(screenUV, depth, UNITY_MATRIX_I_VP);
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 screenUV = GetNormalizedScreenSpaceUV(IN.positionHCS);

                float3 worldPosition = AbsoluteWorldPos(screenUV);

                float dist = length(worldPosition.xz - _Center.xz);

                float factor = smoothstep(0, _Distance, dist);
                half3 colorMult = half3(factor, factor, factor);
                return half4(colorMult, 1);

            }
            ENDHLSL
        }
    }
}
