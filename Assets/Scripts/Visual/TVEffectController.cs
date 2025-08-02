using DG.Tweening;
using Event;
using UnityEngine;
using UnityEngine.UI;

namespace Visual
{
    public class TVPowerController : MonoBehaviour
    {
        private static readonly int IsTurningOn = Shader.PropertyToID("_IsTurningOn");
        private static readonly int TransitionProgress = Shader.PropertyToID("_TransitionProgress");
        private static readonly int SnowEnabled = Shader.PropertyToID("_SnowEnabled");
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
        private static readonly int PulseIntensity = Shader.PropertyToID("_PulseIntensity");
        private static readonly int PulseWidth = Shader.PropertyToID("_PulseWidth");
        
        public RawImage rawImage;
        public float transitionDuration = 0.3f;
        public float pulseIntensity = 5f;
        public float pulseWidth = 0.01f;
        public float onOpacity = 0.03f;

        private Material _material;
        private bool _isOn = true;

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
        
        private void Start()
        {
            if (!rawImage.material)
            {
                rawImage.material = new Material(Shader.Find("Unlit/SnowNoiseShader"));
            }

            _material = rawImage.material;

            _material.SetFloat(IsTurningOn, 1);
            _material.SetFloat(TransitionProgress, 1);
            _material.SetFloat(SnowEnabled, 1);
            _material.SetFloat(Opacity, 1);
            _material.SetFloat(PulseIntensity, pulseIntensity);
            _material.SetFloat(PulseWidth, pulseWidth);

            rawImage.color = new Color(1, 1, 1, 1);
        }
        
        [EventSubscribe("PowerButtonClick")]
        public object TogglePower(string anyway)
        {
            if (_isOn)
            {
                TurnOffTV();
            }
            else
            {
                TurnOnTV();
                EventManager.Instance.TriggerEvent("PowerOn","114514");
            }

            _isOn = !_isOn;
            return "114514";
        }

        private void TurnOffTV()
        {
            var turnOffSequence = DOTween.Sequence();
            
            turnOffSequence.AppendCallback(() => Shake());
            
            turnOffSequence.AppendCallback(() =>
            {
                _material.SetFloat(IsTurningOn, 0);
                _material.SetFloat(TransitionProgress, 0);
                
                DOTween.To(() => _material.GetFloat(TransitionProgress),
                        x => _material.SetFloat(TransitionProgress, x),
                        1, transitionDuration)
                    .SetEase(Ease.InOutQuad);
            });
            turnOffSequence.AppendCallback(() =>
            {
                DOTween.To(() => _material.GetFloat(Opacity),
                        x => _material.SetFloat(Opacity, x),
                        1, transitionDuration)
                    .SetEase(Ease.InOutQuad);
            });

            turnOffSequence.Play();
        }

        private void TurnOnTV()
        {
            var turnOnSequence = DOTween.Sequence();

            turnOnSequence.AppendCallback(() => { rawImage.DOFade(onOpacity, 0.2f).SetEase(Ease.InOutQuad); });
            turnOnSequence.AppendInterval(0.2f);

            turnOnSequence.AppendCallback(() => Shake());

            turnOnSequence.AppendCallback(() =>
            {
                _material.SetFloat(IsTurningOn, 1);
                _material.SetFloat(TransitionProgress, 0);

                DOTween.To(() => _material.GetFloat(TransitionProgress),
                        x => _material.SetFloat(TransitionProgress, x),
                        1, transitionDuration)
                    .SetEase(Ease.InOutQuad);
            });
            turnOnSequence.AppendCallback(() =>
            {
                _material.SetFloat(Opacity, 1);

                DOTween.To(() => _material.GetFloat(Opacity),
                        x => _material.SetFloat(Opacity, x),
                        0.03f, transitionDuration)
                    .SetEase(Ease.InOutQuad);
            });
            
            turnOnSequence.Play();
        }

        private void Flash(float duration = 0.5f, int flashes = 3)
        {
            var flashSequence = DOTween.Sequence();

            for (var i = 0; i < flashes; i++)
            {
                flashSequence.Append(
                    DOTween.To(() => _material.GetFloat(Opacity),
                            x => _material.SetFloat(Opacity, x),
                            0.3f, duration / (flashes * 2))
                        .SetEase(Ease.InOutQuad)
                );

                flashSequence.Append(
                    DOTween.To(() => _material.GetFloat(Opacity),
                            x => _material.SetFloat(Opacity, x),
                            1, duration / (flashes * 2))
                        .SetEase(Ease.InOutQuad)
                );
            }

            flashSequence.Play();
        }

        private void Shake(float strength = 10f, float duration = 0.3f)
        {
            rawImage.rectTransform
                .DOShakeAnchorPos(duration, strength, 20, 90, false, true)
                .SetEase(Ease.OutQuad);
        }

        public void SetSnowEnabled(bool enabledd)
        {
            _material.SetFloat(SnowEnabled, enabledd ? 1 : 0);
        }

        public void SetOpacity(float opacity)
        {
            _material.SetFloat(Opacity, opacity);
        }

        public void SetPulseIntensity(float intensity)
        {
            pulseIntensity = intensity;
            _material.SetFloat(PulseIntensity, intensity);
        }

        public void SetPulseWidth(float width)
        {
            pulseWidth = width;
            _material.SetFloat(PulseWidth, width);
        }

        public void RandomizeEffect()
        {
            var randomIntensity = Random.Range(3f, 8f);
            var randomWidth = Random.Range(0.005f, 0.02f);

            SetPulseIntensity(randomIntensity);
            SetPulseWidth(randomWidth);
            Flash(0.4f, Random.Range(2, 5));
        }
    }
}