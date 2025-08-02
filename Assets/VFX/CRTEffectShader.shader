Shader "Unlit/CRTEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScanlineIntensity ("Scanline Intensity", Range(0,1)) = 0.5
        _ScanlineCount ("Scanline Count", Float) = 300
        _ScanlineSpeed ("Scanline Speed", Float) = 1.0
        _Curvature ("Curvature", Range(0,1)) = 0.5
        _VignetteIntensity ("Vignette Intensity", Range(0,1)) = 0.5
        _ChromaAberration ("Chroma Aberration", Range(0,1)) = 0.5
        _NoiseIntensity ("Noise Intensity", Range(0,1)) = 0.2
        _Brightness ("Brightness", Range(0,2)) = 1.0
        _Contrast ("Contrast", Range(0,2)) = 1.0
        _OverlayOpacity ("Overlay Opacity", Range(0,1)) = 0.7
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
            float _ScanlineIntensity, _ScanlineCount, _ScanlineSpeed, _Curvature;
            float _VignetteIntensity, _ChromaAberration, _NoiseIntensity;
            float _Brightness, _Contrast, _OverlayOpacity;
            
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            // 生成噪点
            float random(float2 uv) {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }
            
            // 生成扫描线
            float scanline(float2 uv) {
                float scanline = sin(uv.y * _ScanlineCount + _Time.y * _ScanlineSpeed * 10.0);
                scanline = (scanline * 0.5 + 0.5) * _ScanlineIntensity;
                return scanline;
            }
            
            // 生成曲面效果
            float2 curve(float2 uv) {
                uv = (uv - 0.5) * 2.0;
                uv *= 1.1;  
                uv.x *= 1.0 + pow((abs(uv.y) / 5.0), 2.0) * _Curvature;
                uv.y *= 1.0 + pow((abs(uv.x) / 4.0), 2.0) * _Curvature;
                uv  = (uv / 2.0) + 0.5;
                return uv;
            }
            
            // 生成暗角效果
            float vignette(float2 uv) {
                float2 center = float2(0.5, 0.5);
                float dist = distance(uv, center);
                float vignette = smoothstep(0.8, 0.2, dist);
                return lerp(1.0, vignette, _VignetteIntensity);
            }
            
            fixed4 frag (v2f i) : SV_Target {
                // 应用曲面效果
                float2 curvedUV = curve(i.uv);
                
                // 采样纹理
                fixed4 col = tex2D(_MainTex, curvedUV);
                
                // 应用色差效果
                float2 aberrationOffset = float2(_ChromaAberration * 0.01, 0);
                fixed4 colR = tex2D(_MainTex, curvedUV + aberrationOffset);
                fixed4 colB = tex2D(_MainTex, curvedUV - aberrationOffset);
                col.r = colR.r;
                col.b = colB.b;
                
                // 应用扫描线
                float scan = scanline(curvedUV);
                col.rgb *= scan;
                
                // 应用噪点
                float noise = random(curvedUV + _Time.y * 0.1) * _NoiseIntensity;
                col.rgb += noise;
                
                // 应用暗角
                float vig = vignette(curvedUV);
                col.rgb *= vig;
                
                // 应用亮度和对比度
                col.rgb = (col.rgb - 0.5) * _Contrast + 0.5;
                col.rgb *= _Brightness;
                
                // 应用蒙版透明度
                col.a = _OverlayOpacity;
                
                return col;
            }
            ENDCG
        }
    }
}