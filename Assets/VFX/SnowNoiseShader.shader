Shader "Unlit/SnowNoiseShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseScale ("Noise Scale", Float) = 100.0
        _Speed ("Speed", Float) = 10.0
        _TransitionProgress ("Transition Progress", Range(0,1)) = 0
        _IsTurningOn ("Is Turning On", Float) = 0
        _TransitionSpeed ("Transition Speed", Float) = 10.0
        _SnowEnabled ("Snow Enabled", Float) = 1.0
        _Opacity ("Opacity", Range(0,1)) = 1.0
        _PulseIntensity ("Pulse Intensity", Range(0,10)) = 5.0
        _PulseWidth ("Pulse Width", Range(0.001,0.05)) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata { 
                float4 vertex : POSITION; 
                float2 uv : TEXCOORD0; 
            };
            
            struct v2f { 
                float2 uv : TEXCOORD0; 
                float4 vertex : SV_POSITION; 
            };
            
            sampler2D _MainTex; 
            float4 _MainTex_ST;
            float _NoiseScale, _Speed, _TransitionProgress, _IsTurningOn, _TransitionSpeed;
            float _SnowEnabled, _Opacity, _PulseIntensity, _PulseWidth;
            
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                float2 distortion = float2(sin(_Time.y * 10) * 0.01, 0);
                i.uv += distortion * (1 - _TransitionProgress);
                float noise = 0;
                if (_SnowEnabled > 0.5) {
                    noise = frac(sin(dot(i.uv * _NoiseScale + _Time.y * _Speed, float2(12.9898,78.233))) * 43758.5453);
                }
                float3 snow = float3(noise, noise, noise);
                
                float line_factor = _TransitionProgress * (1.0 - _TransitionProgress) * 4.0;
                
                float off_edge_left = 0.5 - _TransitionProgress * 0.5;
                float off_edge_right = 0.5 + _TransitionProgress * 0.5;
                float off_dist = min(abs(i.uv.x - off_edge_left), abs(i.uv.x - off_edge_right));
                float off_line_intensity = 1.0 - smoothstep(0.0, _PulseWidth, off_dist);
                float3 off_effect = lerp(snow, float3(0,0,0), _TransitionProgress) + 
                                  float3(off_line_intensity, off_line_intensity, off_line_intensity) * line_factor * _PulseIntensity;
                
                float on_edge_left = _TransitionProgress * 0.5;
                float on_edge_right = 1.0 - _TransitionProgress * 0.5;
                float on_dist = min(abs(i.uv.x - on_edge_left), abs(i.uv.x - on_edge_right));
                float on_line_intensity = 1.0 - smoothstep(0.0, _PulseWidth, on_dist);
                float3 on_effect = lerp(float3(0,0,0), snow, _TransitionProgress) + 
                                 float3(on_line_intensity, on_line_intensity, on_line_intensity) * line_factor * _PulseIntensity;
                
                float3 final_color = lerp(off_effect, on_effect, _IsTurningOn);
                
                return fixed4(final_color, _Opacity);
            }
            ENDCG
        }
    }
}