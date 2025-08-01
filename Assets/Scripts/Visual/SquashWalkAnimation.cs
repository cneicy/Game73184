using DG.Tweening;
using UnityEngine;

namespace Visual
{
    public class SquashWalkAnimation : MonoBehaviour
    {
        [Header("挤压参数")]
        [SerializeField] [Range(0.01f, 0.1f)] private float squashIntensity = 0.05f;   // 降低挤压强度 (0.01-0.1)
        [SerializeField] [Range(0.1f, 0.5f)] private float squashDuration = 0.2f;       // 缩短单次挤压持续时间
        [SerializeField] private AnimationCurve squashCurve = AnimationCurve.Linear(0, 0, 1, 1); // 自定义挤压曲线
        
        private Vector3 _originalScale;
        private Sequence _walkSequence;
        private bool _isWalking;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        private void Start()
        {
            CreateWalkSequence();
        }

        private void CreateWalkSequence()
        {
            // 确保之前的序列被正确释放
            _walkSequence?.Kill();
        
            _walkSequence = DOTween.Sequence()
                // 第一步：轻微挤压（垂直压缩，水平拉伸）
                .Append(transform.DOScaleY(1f - squashIntensity, squashDuration * 0.3f).SetEase(squashCurve))
                .Join(transform.DOScaleX(1f + squashIntensity * 0.8f, squashDuration * 0.3f).SetEase(squashCurve))
            
                // 第二步：恢复并轻微拉伸（垂直拉伸，水平压缩）
                .Append(transform.DOScaleY(1f + squashIntensity * 0.3f, squashDuration * 0.3f).SetEase(squashCurve))
                .Join(transform.DOScaleX(1f - squashIntensity * 0.2f, squashDuration * 0.3f).SetEase(squashCurve))
            
                // 第三步：恢复正常
                .Append(transform.DOScale(_originalScale, squashDuration * 0.4f).SetEase(Ease.InOutSine))
            
                // 设置循环
                .SetLoops(-1, LoopType.Restart)
                .SetAutoKill(false)
                .Pause(); // 初始暂停状态
        }

        public void StartWalking()
        {
            if (_isWalking) return;
            _isWalking = true;
            _walkSequence.Play();
        }

        public void StopWalking()
        {
            if (!_isWalking) return;
            _isWalking = false;
            _walkSequence.Pause();
            
            // 平滑恢复原始大小
            transform.DOScale(_originalScale, 0.15f).SetEase(Ease.OutSine);
        }

        // 根据移动速度调整动画速度
        public void SetAnimationSpeed(float speedFactor)
        {
            if (_walkSequence != null && _walkSequence.IsPlaying())
            {
                // 速度因子范围0.5-1.5
                _walkSequence.timeScale = Mathf.Lerp(0.5f, 1.5f, speedFactor);
            }
        }

        void OnDestroy()
        {
            _walkSequence?.Kill();
        }
    }
}