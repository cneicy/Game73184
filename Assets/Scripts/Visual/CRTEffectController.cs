using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Visual
{
    public class CRTEffectController : MonoBehaviour
    {
        private static readonly int ScanlineIntensity = Shader.PropertyToID("_ScanlineIntensity");
        private static readonly int ScanlineCount = Shader.PropertyToID("_ScanlineCount");
        private static readonly int ScanlineSpeed = Shader.PropertyToID("_ScanlineSpeed");
        private static readonly int Curvature = Shader.PropertyToID("_Curvature");
        private static readonly int VignetteIntensity = Shader.PropertyToID("_VignetteIntensity");
        private static readonly int ChromaAberration = Shader.PropertyToID("_ChromaAberration");
        private static readonly int NoiseIntensity = Shader.PropertyToID("_NoiseIntensity");
        private static readonly int Brightness = Shader.PropertyToID("_Brightness");
        private static readonly int Contrast = Shader.PropertyToID("_Contrast");
        private static readonly int OverlayOpacity = Shader.PropertyToID("_OverlayOpacity");
        
        public RawImage rawImage;
        public float scanlineIntensity = 0.5f;
        public int scanlineCount = 300;
        public float scanlineSpeed = 1.0f;
        public float curvature = 0.5f;
        public float vignetteIntensity = 0.5f;
        public float chromaAberration = 0.5f;
        public float noiseIntensity = 0.2f;
        public float brightness = 1.0f;
        public float contrast = 1.0f;
        public float overlayOpacity = 0.7f; // 蒙版透明度
        
        private Material _material;
        private bool _isOn = true;

        

        // 闪烁效果
        private void Flash(float duration = 0.5f, int flashes = 3)
        {
            var flashSequence = DOTween.Sequence();

            for (var i = 0; i < flashes; i++)
            {
                flashSequence.Append(
                    DOTween.To(() => _material.GetFloat(Brightness),
                            x => _material.SetFloat(Brightness, x),
                            brightness * 0.3f, duration / (flashes * 2))
                        .SetEase(Ease.InOutQuad)
                );

                flashSequence.Append(
                    DOTween.To(() => _material.GetFloat(Brightness),
                            x => _material.SetFloat(Brightness, x),
                            brightness, duration / (flashes * 2))
                        .SetEase(Ease.InOutQuad)
                );
            }

            flashSequence.Play();
        }

        // 设置扫描线强度
        public void SetScanlineIntensity(float intensity)
        {
            scanlineIntensity = intensity;
            _material.SetFloat(ScanlineIntensity, intensity);
        }

        // 设置扫描线数量
        public void SetScanlineCount(int count)
        {
            scanlineCount = count;
            _material.SetFloat(ScanlineCount, count);
        }

        // 设置扫描线速度
        public void SetScanlineSpeed(float speed)
        {
            scanlineSpeed = speed;
            _material.SetFloat(ScanlineSpeed, speed);
        }

        // 设置曲面强度
        public void SetCurvature(float curvatureValue)
        {
            curvature = curvatureValue;
            _material.SetFloat(Curvature, curvatureValue);
        }

        // 设置暗角强度
        public void SetVignetteIntensity(float intensity)
        {
            vignetteIntensity = intensity;
            _material.SetFloat(VignetteIntensity, intensity);
        }

        // 设置色差强度
        public void SetChromaAberration(float aberration)
        {
            chromaAberration = aberration;
            _material.SetFloat(ChromaAberration, aberration);
        }

        // 设置噪点强度
        public void SetNoiseIntensity(float intensity)
        {
            noiseIntensity = intensity;
            _material.SetFloat(NoiseIntensity, intensity);
        }

        // 设置亮度
        public void SetBrightness(float brightnessValue)
        {
            brightness = brightnessValue;
            _material.SetFloat(Brightness, brightnessValue);
        }

        // 设置对比度
        public void SetContrast(float contrastValue)
        {
            contrast = contrastValue;
            _material.SetFloat(Contrast, contrastValue);
        }
        
        // 设置蒙版透明度
        public void SetOverlayOpacity(float opacity)
        {
            overlayOpacity = opacity;
            _material.SetFloat(OverlayOpacity, opacity);
            rawImage.color = new Color(1, 1, 1, opacity);
        }

        // 应用预设CRT效果
        public void ApplyClassicCRTEffect()
        {
            SetScanlineIntensity(0.6f);
            SetScanlineCount(300);
            SetScanlineSpeed(1.0f);
            SetCurvature(0.5f);
            SetVignetteIntensity(0.6f);
            SetChromaAberration(0.3f);
            SetNoiseIntensity(0.15f);
            SetBrightness(1.0f);
            SetContrast(1.1f);
            SetOverlayOpacity(0.7f);
        }

        // 应用现代CRT效果
        public void ApplyModernCRTEffect()
        {
            SetScanlineIntensity(0.3f);
            SetScanlineCount(500);
            SetScanlineSpeed(0.5f);
            SetCurvature(0.2f);
            SetVignetteIntensity(0.3f);
            SetChromaAberration(0.1f);
            SetNoiseIntensity(0.05f);
            SetBrightness(1.1f);
            SetContrast(1.2f);
            SetOverlayOpacity(0.5f);
        }

        // 应用损坏CRT效果
        public void ApplyDamagedCRTEffect()
        {
            SetScanlineIntensity(0.8f);
            SetScanlineCount(200);
            SetScanlineSpeed(2.0f);
            SetCurvature(0.7f);
            SetVignetteIntensity(0.8f);
            SetChromaAberration(0.6f);
            SetNoiseIntensity(0.4f);
            SetBrightness(0.8f);
            SetContrast(1.3f);
            SetOverlayOpacity(0.9f);
            
            Flash(0.4f, 5);
        }
    }
}