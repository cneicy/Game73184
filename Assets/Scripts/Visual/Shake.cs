using DG.Tweening;
using Event;
using UnityEngine;
using UnityEngine.UI;

namespace Visual
{
    public class Shake : MonoBehaviour
    {
        [Header("抖动参数")]
        [SerializeField] private float duration = 0.5f;    // 抖动持续时间
        [SerializeField] private float strength = 30f;     // 抖动强度
        [SerializeField] private int vibrato = 20;         // 抖动振动次数
        [SerializeField] private float randomness = 90f;   // 随机性(0-180)
    
        private RawImage _rawImage;
        private Vector2 _originalPosition;

        private void OnEnable()
        {
            if(EventManager.Instance)
                EventManager.Instance.RegisterEventHandlersFromAttributes(this);
        }

        private void OnDisable()
        {
            if(EventManager.Instance)
                EventManager.Instance.UnregisterAllEventsForObject(this);
        }
        
        private void Awake()
        {
            _rawImage = GetComponent<RawImage>();
            _originalPosition = _rawImage.rectTransform.anchoredPosition;
        }

        [EventSubscribe("Harvest")]
        public object TriggerHitEffect(float harvest)
        {
            _rawImage.rectTransform.anchoredPosition = _originalPosition;
            _rawImage.rectTransform
                .DOShakeAnchorPos(duration, strength, vibrato, randomness)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    _rawImage.rectTransform.anchoredPosition = _originalPosition;
                });
            return null;
        }
    }
}