using System.Collections.Generic;
using UnityEngine;

namespace Visual
{
    public class RectTransformScaler : MonoBehaviour
    {
        [Header("需要统一缩放的RectTransform")]
        public Transform[] targetRects;
    
        [Header("缩放设置")]
        [SerializeField] private float baseScale = 1f;
        [SerializeField] private bool preserveLayout = true;
        
        private Dictionary<Transform, Vector3> _originalLocalScales = new();
        private Dictionary<Transform, Vector2> _originalSizes = new();
        private Dictionary<Transform, Vector2> _originalAnchoredPositions = new();
    
        private void Start()
        {
            RecordOriginalData();
        }
    
        private void RecordOriginalData()
        {
            _originalLocalScales.Clear();
            _originalSizes.Clear();
            _originalAnchoredPositions.Clear();
        
            foreach (var target in targetRects)
            {
                if (!target || target is not RectTransform rectTransform) continue;
                _originalLocalScales[target] = rectTransform.localScale;
                _originalSizes[target] = rectTransform.sizeDelta;
                _originalAnchoredPositions[target] = rectTransform.anchoredPosition;
            }
        }
    
        /// <summary>
        /// 设置基础缩放倍数（绝对值）
        /// </summary>
        /// <param name="scale">缩放倍数</param>
        public void SetBaseScale(float scale)
        {
            baseScale = scale;
            ApplyScale();
        }
    
        /// <summary>
        /// 相对缩放（基于当前基础缩放）
        /// </summary>
        /// <param name="multiplier">缩放倍数</param>
        public void ScaleBy(float multiplier)
        {
            baseScale *= multiplier;
            ApplyScale();
        }
    
        /// <summary>
        /// 增量缩放
        /// </summary>
        /// <param name="delta">缩放增量</param>
        public void ScaleAdd(float delta)
        {
            baseScale += delta;
            ApplyScale();
        }
    
        /// <summary>
        /// 应用缩放
        /// </summary>
        private void ApplyScale()
        {
            baseScale = Mathf.Max(0.01f, baseScale);
        
            foreach (var target in targetRects)
            {
                if (!target || target is not RectTransform rectTransform) continue;
                if (preserveLayout)
                {
                    ApplyLayoutPreservingScale(rectTransform, baseScale);
                }
                else
                {
                    if (_originalLocalScales.ContainsKey(target))
                    {
                        rectTransform.localScale = _originalLocalScales[target] * baseScale;
                    }
                }
            }
        }
    
        /// <summary>
        /// 应用保持布局的缩放
        /// </summary>
        private void ApplyLayoutPreservingScale(RectTransform rectTransform, float targetScale)
        {
            // 重置到原始状态
            if (_originalLocalScales.TryGetValue(rectTransform, out var scale))
                rectTransform.localScale = scale;
        
            if (_originalSizes.TryGetValue(rectTransform, out var siz))
                rectTransform.sizeDelta = siz;
            
            if (_originalAnchoredPositions.TryGetValue(rectTransform, out var position))
                rectTransform.anchoredPosition = position;
            
            if (_originalSizes.TryGetValue(rectTransform, out var originalSiz))
            {
                var scaledSize = originalSiz * targetScale;
                rectTransform.sizeDelta = scaledSize;
            }

            if (!_originalAnchoredPositions.TryGetValue(rectTransform, out var anchoredPosition)) return;
            var scaledPosition = anchoredPosition * targetScale;
            rectTransform.anchoredPosition = scaledPosition;
        }
    
        /// <summary>
        /// 重置到原始状态
        /// </summary>
        public void ResetToOriginal()
        {
            baseScale = 1f;
        
            foreach (var target in targetRects)
            {
                if (!target || target is not RectTransform rectTransform) continue;
                if (_originalLocalScales.TryGetValue(target, out var scale))
                    rectTransform.localScale = scale;
                    
                if (_originalSizes.TryGetValue(target, out var siz))
                    rectTransform.sizeDelta = siz;
                    
                if (_originalAnchoredPositions.TryGetValue(target, out var position))
                    rectTransform.anchoredPosition = position;
            }
        }
    
        /// <summary>
        /// 获取当前基础缩放值
        /// </summary>
        public float GetBaseScale()
        {
            return baseScale;
        }
    
        /// <summary>
        /// 添加RectTransform到管理列表
        /// </summary>
        public void AddRectTransform(Transform newRect)
        {
            if (!newRect || newRect is not RectTransform rect) return;
        
            var tempList = new List<Transform>(targetRects);
            if (tempList.Contains(rect)) return;
            tempList.Add(rect);
            targetRects = tempList.ToArray();
            
            // 记录新添加的RectTransform的原始数据
            _originalLocalScales[rect] = rect.localScale;
            _originalSizes[rect] = rect.sizeDelta;
            _originalAnchoredPositions[rect] = rect.anchoredPosition;
        }
    
        /// <summary>
        /// 刷新原始数据记录（当手动调整后需要重新记录时调用）
        /// </summary>
        public void RefreshOriginalData()
        {
            RecordOriginalData();
            ApplyScale(); // 重新应用当前缩放
        }
    
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying && targetRects != null)
            {
                ApplyScale();
            }
        }
#endif
    }
}