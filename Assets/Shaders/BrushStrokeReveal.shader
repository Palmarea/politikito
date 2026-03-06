Shader "Custom/BrushStrokeReveal"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _RevealAmount ("Reveal Amount", Range(0, 1)) = 1.0
        _Brushiness ("Brushiness (1=strokes, 0=clean wipe)", Range(0, 1)) = 0.65
        _NoiseScaleX ("Noise Scale X", Float) = 5.0
        _NoiseScaleY ("Noise Scale Y", Float) = 3.0
        _EdgeWidth ("Edge Softness", Range(0.01, 0.5)) = 0.12
        _StrokeAngle ("Stroke Angle Bias", Range(0.0, 3.0)) = 1.5
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
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
            #pragma target 2.0
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _RevealAmount;
            float _NoiseScaleX;
            float _NoiseScaleY;
            float _EdgeWidth;
            float _StrokeAngle;
            float _Brushiness;

            // ── Noise helpers ──────────────────────────────────────────
            float hash2(float2 p)
            {
                p = float2(dot(p, float2(127.1, 311.7)),
                           dot(p, float2(269.5, 183.3)));
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            float valueNoise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float2 u = f * f * (3.0 - 2.0 * f); // smoothstep
                return lerp(lerp(hash2(i + float2(0,0)), hash2(i + float2(1,0)), u.x),
                            lerp(hash2(i + float2(0,1)), hash2(i + float2(1,1)), u.x), u.y);
            }

            // Fractal Brownian Motion – mimics layered brush strokes
            float fbm(float2 p)
            {
                float val   = 0.0;
                float amp   = 0.5;
                float freq  = 1.0;
                // 5 octaves – good balance between detail and cost
                for (int i = 0; i < 5; i++)
                {
                    val  += amp * valueNoise(p * freq);
                    freq *= 2.03;
                    amp  *= 0.48;
                }
                return saturate(val / 0.97); // normalise to [0,1]
            }
            // ─────────────────────────────────────────────────────────────

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.vertex   = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color    = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex   = UnityPixelSnap(OUT.vertex);
                #endif
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // ── Sample sprite ──────────────────────────────────────
                fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;

                // ── Build brush-stroke noise mask ──────────────────────
                float2 uv = IN.texcoord;

                // Shear UVs along X to give a diagonal brushstroke feel
                uv.x += uv.y * _StrokeAngle;

                // Scale independently so strokes can be wide but short (or vice-versa)
                uv *= float2(_NoiseScaleX, _NoiseScaleY);

                float n = fbm(uv);

                // ── Left-to-right directional blend ───────────────────
                // Blend the FBM noise with a plain horizontal gradient (UV.x).
                // This makes the reveal sweep from left to right while
                // keeping the organic brushstroke character on the edges.
                float gradient = IN.texcoord.x; // 0 = left, 1 = right
                float combined = lerp(gradient, n, _Brushiness);

                // ── Reveal threshold ───────────────────────────────────
                // _RevealAmount goes 0 (hidden) → 1 (fully shown)
                // We map it into [−edge, 1+edge] so the fade starts/ends smoothly
                float threshold = lerp(-_EdgeWidth, 1.0 + _EdgeWidth, _RevealAmount);

                // Pixels where combined < threshold are revealed; smoothstep gives a soft fringe
                float mask = smoothstep(threshold, threshold - _EdgeWidth, combined);

                // ── Apply mask, premultiply alpha ──────────────────────
                c.a  *= mask;
                c.rgb *= c.a; // premultiplied alpha for Unity's sprite blending

                return c;
            }
            ENDCG
        }
    }
}
