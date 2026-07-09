Shader "Skybox/StarryBackground"
{
    Properties
    {
        [Header(Scrolling)]
        [Tooltip(Controls how fast the fog moves horizontally)]
        _FogScrollSpeedX ("Fog Scroll Speed X", Float) = 0.0
        [Tooltip(Controls how fast the fog moves vertically)]
        _FogScrollSpeedY ("Fog Scroll Speed Y", Float) = 0.1


        [Header(Resolution)]
        [Tooltip(The pixelation resolution for fog. Lower values will pixelate)]
        _FogPixelResolution ("Fog Pixel Resolution", Range(1.0, 2048.0)) = 600.0
        [Tooltip(The pixelation resolution constraint for the star field lower values will pixelate)]
        _StarPixelResolution ("Star Pixel Resolution", Range(1.0, 2048.0)) = 600.0

        [Header(Fog)]
        [Tooltip(The number of noise layers used for the fog. Higher values add more detail)]
        _Octaves ("Octaves", Integer) = 10
        [Tooltip(The overall scale of the fog noise)]
        _FogScale ("Fog Scale", Float) = 1
        [Tooltip(How fast the fog festers in place)]
        _FogSpeed ("Fog Speed", Float) = 0.5
        [Tooltip(The color gradient applied to the fog)]
        _FogColorRamp ("Fog Color Ramp", 2D) = "white" {}
        [Toggle(USE_8X8_DITHER)] _Use8x8Dither ("Use 8x8 Dither", Float) = 0
        [Tooltip(Controls the intensity of the dithering)]
        _FogDitherSpread ("Fog Dither Spread", Range(0, 1)) = 0.05

        [Header(Stars)]
        [Tooltip(The density of the star grid)]
        _StarGrid ("Star Grid", Range(1, 1000)) = 700.0
        [Tooltip(The size of each individual star)]
        _StarSize ("Star Scale", Range(0.0, 1.0)) = 0.3
        [Tooltip(The opacity of the stars)]
        _StarOpacity ("Star Opacity", Range(0.0, 1.0)) = 1
        [Tooltip(The probability of a star appearing in a grid cell)]
        _StarProbability ("Star Probability", Range(0.0, 1.0)) = 0.02
        [Tooltip(The speed of the star twinkling)]
        _StarFlicker ("Star Flicker", Float) = 3

        [Header(Moon)]
        [Tooltip(The moon texture)]
        _MoonTex ("Moon Texture", 2D) = "black" {}
        [Tooltip(The direction the moon appears in the sky)]
        _MoonDir ("Moon Direction", Vector) = (0.3, 0.6, 0.5, 0)
        [Tooltip(The size of the moon)]
        _MoonSize ("Moon Size", Range(0.01, 0.5)) = 0.08
        [Tooltip(Color tint applied to the moon)]
        [HDR] _MoonColor ("Moon Color", Color) = (1, 1, 1, 1)
        [Tooltip(Intensity of the glow around the moon)]
        _MoonGlow ("Moon Glow", Range(0, 2)) = 0.3
        [Tooltip(Size of the glow around the moon)]
        _MoonGlowSize ("Moon Glow Size", Range(1, 15)) = 2.0
    }
    SubShader
    {
        Tags
        {
            "Queue"="Background"
            "RenderType"="Background"
            "PreviewType"="Skybox"
        }
        LOD 100
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature USE_8X8_DITHER

            #include "UnityCG.cginc"
            #include "Assets/Shaders/Utility/Fbm.hlsl"
            #include "Assets/Shaders/Utility/Dithering.hlsl"
            #include "Assets/Shaders/Utility/keijiro/SimplexNoise2D.hlsl"

            struct MeshData
            {
                float4 vertex : POSITION;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float3 viewDir : TEXCOORD0;
            };

            UNITY_DECLARE_TEX2D(_FogColorRamp);
            float _FogScrollSpeedX;
            float _FogScrollSpeedY;

            float _FogPixelResolution;
            float _StarPixelResolution;
            int _Octaves;
            float _StarGrid;
            float _StarSize;
            float _StarProbability;
            float _StarOpacity;
            float _FogScale;
            float _FogSpeed;
            float _FogDitherSpread;
            float _StarFlicker;

            UNITY_DECLARE_TEX2D(_MoonTex);
            float4 _MoonDir;
            float _MoonSize;
            float4 _MoonColor;
            float _MoonGlow;
            float _MoonGlowSize;

            Interpolators vert(MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.viewDir = v.vertex.xyz;
                return o;
            }

            float hash31(float3 p)
            {
                p = frac(p * float3(443.897, 441.423, 437.195));
                p += dot(p, p.yzx + 19.19);
                return frac((p.x + p.y) * p.z);
            }

            fixed4 frag(Interpolators i) : SV_Target
            {
                float3 dir = normalize(i.viewDir);

                float3 fogDir = dir;
                fogDir.x += _Time.y * _FogScrollSpeedX;
                fogDir.y += _Time.y * _FogScrollSpeedY;

                float3 fogPixelDir = floor(fogDir * _FogPixelResolution) / _FogPixelResolution;

                // Dithering
                int ditherX = (int)i.vertex.x;
                int ditherY = (int)i.vertex.y;
#ifdef USE_8X8_DITHER
                float dither = bayerMatrix8x8[(ditherX % 8) + (ditherY % 8) * 8];
#else
                float dither = bayerMatrix4x4[(ditherX % 4) + (ditherY % 4) * 4];
#endif
                float fogNoise = (dither - 0.5) * _FogDitherSpread;

                // FBM Fog
                float offset = fbm(fogPixelDir * _FogScale + float3(0, 0, _Time.y * _FogSpeed), _Octaves);
                fogNoise += fbm(fogPixelDir + offset + float3(0, 0, _Time.y * _FogSpeed), _Octaves);
                // Sample Color From Color Ramp
                float3 fbmColor = UNITY_SAMPLE_TEX2D(_FogColorRamp, float2(fogNoise, 0.5)).rgb;

                // Stars
                float3 starPixelDir = floor(dir * _StarPixelResolution) / _StarPixelResolution;
                // 1) Create 3D grid
                float3 starGridCell = floor(starPixelDir * _StarGrid);
                float3 starLocal3D = frac(starPixelDir * _StarGrid) - 0.5;
                // 2) Random value per cell
                float randomStar = hash31(starGridCell);
                // 3) Offset the center of the star within each cell
                float oX = hash31(starGridCell + float3(13.5, 0.0, 7.3)) * 0.8 - 0.4;
                float oY = hash31(starGridCell + float3(0.0, 42.7, 3.1)) * 0.8 - 0.4;
                float oZ = hash31(starGridCell + float3(5.2, 0.0, 91.4)) * 0.8 - 0.4;
                // 4) Get local position relative to the offset star center
                float3 starOffset = starLocal3D - float3(oX, oY, oZ);
                // 5) Draw a small dot for star
                float starShape = smoothstep(_StarSize, 0.0, length(starOffset));
                float star = starShape * step(1.0 - _StarProbability, randomStar);
                // 6) Make stars twinkle
                float3 twinkleCell = floor(dir * _StarGrid);
                float twinklePhase = hash31(twinkleCell + float3(77.7, 33.3, 55.5));
                float twinkle = sin(_Time.y * _StarFlicker + twinklePhase * UNITY_TWO_PI) * 0.5 + 0.5;
                float3 stars = star * twinkle * _StarOpacity;

                // Moon
                float3 moonDir = normalize(_MoonDir.xyz);
                float moonDist = acos(saturate(dot(dir, moonDir)));
                // Project view direction onto moon disc
                float3 moonRight = normalize(cross(float3(0, 1, 0), moonDir));
                float3 moonUp = cross(moonDir, moonRight);
                float2 moonUV = float2(dot(dir - moonDir, moonRight), dot(dir - moonDir, moonUp));
                moonUV = moonUV / _MoonSize * 0.5 + 0.5;
                // Box mask, to prevent texture from repeating
                float boxMask = step(0.0, moonUV.x) * step(moonUV.x, 1.0) * step(0.0, moonUV.y) * step(moonUV.y, 1.0);
                // Sample the moon texture
                float4 moonTexSample = UNITY_SAMPLE_TEX2D(_MoonTex, moonUV);
                float3 moonTex = moonTexSample.rgb * _MoonColor.rgb;
                float moonAlpha = moonTexSample.a * _MoonColor.a * boxMask;
                // Soft glow around the moon (glow remains circular)
                float glow = smoothstep(_MoonSize * _MoonGlowSize, _MoonSize, moonDist) * _MoonGlow;
                float3 glowColor = _MoonColor.rgb * glow;

                float3 col = fbmColor + stars * (1.0 - moonAlpha);
                col = lerp(col, moonTex, moonAlpha);
                col += glowColor * (1.0 - moonAlpha);

                return float4(col, 1.0);
            }
            ENDCG
        }
    }
}
