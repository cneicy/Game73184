using DG.Tweening;
using Event;
using GamePlay.Objects.Heroes;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

namespace Visual
{
    public class HPtoScreenColor : MonoBehaviour
    {
        /*private Hero _hero;
        public PostProcessVolume postProcessVolume;
        private ColorGrading _colorGrading;
        private ChromaticAberration _chromaticAberration;
        private LensDistortion _lensDistortion;

        [Header("色调变化曲线")] public AnimationCurve hueShiftCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("最大色调偏移角度")] [Range(0, 360)] public float maxHueShift = 180f;

        [Header("色调变化过渡时间")] public float transitionDuration = 0.5f;

        [Header("色调浮动基础速度")] public float baseFloatSpeed = 1f;

        [Header("色调浮动强度曲线")] public AnimationCurve floatIntensityCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("最大色调浮动强度")] [Range(0, 180)] public float maxFloatIntensity = 30f;

        [Header("色调随机变化强度")] [Range(0, 1)] public float randomIntensity = 0.5f;

        [Header("色差强度范围")] [Tooltip("血量满时的色差强度")]
        public float minChromaticAberration = 0.2f;

        [Tooltip("血量空时的色差强度")] public float maxChromaticAberration = 0.4f;

        [Header("镜头畸变范围")] [Tooltip("血量满时的镜头畸变强度")]
        public float minLensDistortion = 17f;

        [Tooltip("血量空时的镜头畸变强度")] public float maxLensDistortion = 22f;

        private float _currentBaseHueShift;
        private float _targetBaseHueShift;
        private float _currentFloatIntensity;
        private float _targetFloatIntensity;
        private float _currentChromaticAberration;
        private float _targetChromaticAberration;
        private float _currentLensDistortion;
        private float _targetLensDistortion;

        private Tween _baseHueShiftTween;
        private Tween _floatIntensityTween;
        private Tween _chromaticAberrationTween;
        private Tween _lensDistortionTween;

        private float _time;
        private float _randomOffset;

        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
            _hero.Health = Hero.MaxHealth;

            if (postProcessVolume.profile.TryGetSettings(out _colorGrading))
            {
            }
            else
            {
                _colorGrading = postProcessVolume.profile.AddSettings<ColorGrading>();
            }

            if (postProcessVolume.profile.TryGetSettings(out _chromaticAberration))
            {
            }
            else
            {
                _chromaticAberration = postProcessVolume.profile.AddSettings<ChromaticAberration>();
            }

            if (postProcessVolume.profile.TryGetSettings(out _lensDistortion))
            {
            }
            else
            {
                _lensDistortion = postProcessVolume.profile.AddSettings<LensDistortion>();
            }

            _lensDistortion.intensity.value = minLensDistortion;
            _chromaticAberration.intensity.value = minChromaticAberration;
            _colorGrading.hueShift.value = 0;

            _currentBaseHueShift = 0;
            _targetBaseHueShift = 0;
            _currentFloatIntensity = 0;
            _targetFloatIntensity = 0;
            _currentChromaticAberration = minChromaticAberration;
            _targetChromaticAberration = minChromaticAberration;
            _currentLensDistortion = minLensDistortion;
            _targetLensDistortion = minLensDistortion;
            _randomOffset = Random.Range(0f, 100f);
        }

        private void Update()
        {
            _time += Time.deltaTime * baseFloatSpeed;

            var floatEffect = Mathf.Sin(_time) * _currentFloatIntensity;
            var randomEffect = (Mathf.PerlinNoise(_time * 0.5f + _randomOffset, 0f) - 0.5f) * 2f *
                               _currentFloatIntensity * randomIntensity;
            var totalHueShift = _currentBaseHueShift + floatEffect + randomEffect;

            _colorGrading.hueShift.value = totalHueShift;

            _chromaticAberration.intensity.value = _currentChromaticAberration;
            _lensDistortion.intensity.value = _currentLensDistortion;
        }

        [EventSubscribe("Harvest")]
        public object UpdateColorGrading(int finalDamage)
        {
            var healthPercent = _hero.Health / (float)Hero.MaxHealth;
            var factor = 1 - healthPercent;

            var curveValue = hueShiftCurve.Evaluate(factor);

            _targetBaseHueShift = curveValue * maxHueShift;

            var floatCurveValue = floatIntensityCurve.Evaluate(factor);
            _targetFloatIntensity = floatCurveValue * maxFloatIntensity;

            _targetChromaticAberration = Mathf.Lerp(minChromaticAberration, maxChromaticAberration, factor);
            _targetLensDistortion = Mathf.Lerp(minLensDistortion, maxLensDistortion, factor);

            _baseHueShiftTween?.Kill();
            _baseHueShiftTween = DOTween.To(() => _currentBaseHueShift,
                x => _currentBaseHueShift = x,
                _targetBaseHueShift, transitionDuration);

            _floatIntensityTween?.Kill();
            _floatIntensityTween = DOTween.To(() => _currentFloatIntensity,
                x => _currentFloatIntensity = x,
                _targetFloatIntensity, transitionDuration);

            _chromaticAberrationTween?.Kill();
            _chromaticAberrationTween = DOTween.To(() => _currentChromaticAberration,
                x => _currentChromaticAberration = x,
                _targetChromaticAberration, transitionDuration);

            _lensDistortionTween?.Kill();
            _lensDistortionTween = DOTween.To(() => _currentLensDistortion,
                x => _currentLensDistortion = x,
                _targetLensDistortion, transitionDuration);

            return null;
        }

        private void OnEnable()
        {
            if (EventManager.Instance)
                EventManager.Instance.RegisterEventHandlersFromAttributes(this);
        }

        private void OnDisable()
        {
            if (EventManager.Instance)
                EventManager.Instance.UnregisterAllEventsForObject(this);
        }
    }
*/
    }
}