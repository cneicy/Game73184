using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GamePlay
{
    [RequireComponent(typeof(RawImage))]
    public class RenderTextureInteraction: MonoBehaviour, IPointerClickHandler
    {
        [Header("Camera References")]
        public Camera postProcessingCamera; // 后处理相机
    
        [Header("Debug Settings")]
        public bool showDebugRays = true;
        public Color rayColor = Color.red;
        public Color hitColor = Color.green;
        public float gizmoSize = 0.2f;
        public bool verboseDebug = true;
    
        private RawImage _rawImage;
        private RectTransform _rectTransform;
    
        // 调试数据存储
        private Vector3 _debugRayOrigin = Vector3.zero;
        private Vector3 _debugRayDirection = Vector3.forward;
        private bool _hasHit;
        private Vector3 _hitPoint;
        private Collider2D _hitCollider;

        private void Awake()
        {
            _rawImage = GetComponent<RawImage>();
            _rectTransform = _rawImage.rectTransform;

            if (!verboseDebug) return;
            Debug.Log($"Canvas Render Mode: {GetComponentInParent<Canvas>().renderMode}");
            Debug.Log($"Post-Processing Camera: {postProcessingCamera.name}");
            Debug.Log($"后处理相机位置: {postProcessingCamera.transform.position}");
            Debug.Log($"后处理相机正交大小: {postProcessingCamera.orthographicSize}");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        
            // 重置调试数据
            _hasHit = false;
            _hitCollider = null;
        
            // 1. 屏幕坐标转RawImage局部坐标（Overlay模式相机参数为null）
            var success = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform,
                eventData.position,
                null, // 关键修复：Overlay模式必须传null
                out var localPoint
            );
        
            if (!success)
            {
                Debug.LogError("屏幕坐标转局部坐标失败！");
                return;
            }
        
            if (verboseDebug) Debug.Log($"屏幕坐标: {eventData.position} -> 局部坐标: {localPoint}");

            // 2. 计算归一化UV坐标（0-1范围）
            var rawImageRect = _rectTransform.rect;
            var normalizedPoint = new Vector2(
                (localPoint.x - rawImageRect.x) / rawImageRect.width,
                (localPoint.y - rawImageRect.y) / rawImageRect.height
            );
        
            if (verboseDebug) Debug.Log($"RawImage矩形: {rawImageRect} -> 归一化坐标: {normalizedPoint}");

            // 3. 处理自定义UV区域（如部分纹理显示）
            if (_rawImage.uvRect != new Rect(0, 0, 1, 1))
            {
                normalizedPoint = new Vector2(
                    normalizedPoint.x * _rawImage.uvRect.width + _rawImage.uvRect.x,
                    normalizedPoint.y * _rawImage.uvRect.height + _rawImage.uvRect.y
                );
                if (verboseDebug) Debug.Log($"调整后UV坐标: {normalizedPoint}");
            }

            // 4. 转换为后处理相机的世界坐标（关键步骤）
            // 对于2D场景，使用固定深度值而不是nearClipPlane
            var depth = 10f; // 根据你的场景调整这个值
            var viewportPoint = new Vector3(normalizedPoint.x, normalizedPoint.y, depth);
            var worldPoint = postProcessingCamera.ViewportToWorldPoint(viewportPoint);
        
            if (verboseDebug) Debug.Log($"视口点: {viewportPoint} -> 世界坐标: {worldPoint}");

            // 5. 2D点检测（适用于2D场景）
            var colliders = Physics2D.OverlapPointAll(worldPoint);
        
            if (verboseDebug) Debug.Log($"在 {worldPoint} 处找到 {colliders.Length} 个碰撞体");
        
            if (colliders.Length > 0)
            {
                // 找到最上层的碰撞体（根据渲染顺序）
                _hitCollider = GetTopmostCollider(colliders);
                _hasHit = true;
                _hitPoint = worldPoint;
            
                Debug.Log($"命中: {_hitCollider.name} at {_hitPoint}");
            
                // 触发交互
                var interactable = _hitCollider.GetComponent<IInteractable>();
                if (interactable != null) 
                {
                    Debug.Log("触发交互");
                    interactable.Interact();
                }
                else
                {
                    Debug.LogWarning($"对象 {_hitCollider.name} 没有实现IInteractable接口");
                }
            }
            else
            {
                Debug.LogWarning("没有命中任何碰撞体");
            }
        
            // 存储调试数据（确保这一步被执行）
            _debugRayOrigin = worldPoint;
            _debugRayDirection = Vector3.forward;
        
            Debug.Log($"=== 调试数据更新: 原点={_debugRayOrigin}, 命中={_hasHit} ===");
        }

        // 获取最上层的碰撞体（根据渲染顺序）
        private Collider2D GetTopmostCollider(Collider2D[] colliders)
        {
            var topmost = colliders[0];
            var highestOrder = topmost.GetComponent<Renderer>()?.sortingOrder ?? 0;
        
            foreach (var col in colliders)
            {
                var order = col.GetComponent<Renderer>()?.sortingOrder ?? 0;
                if (order <= highestOrder) continue;
                highestOrder = order;
                topmost = col;
            }
            return topmost;
        }

        // Gizmos调试可视化
        private void OnDrawGizmos()
        {
            if (!showDebugRays || !Application.isPlaying) return;
        
            // 绘制检测点
            Gizmos.color = rayColor;
            Gizmos.DrawSphere(_debugRayOrigin, gizmoSize);
        
            // 绘制射线方向（2D场景中通常垂直于屏幕）
            Gizmos.color = new Color(rayColor.r, rayColor.g, rayColor.b, 0.5f);
            Gizmos.DrawRay(_debugRayOrigin, _debugRayDirection * 2f);
        
            // 绘制命中点
            if (!_hasHit) return;
            Gizmos.color = hitColor;
            Gizmos.DrawSphere(_hitPoint, gizmoSize * 1.5f);
            
            // 绘制命中物体轮廓
            if (_hitCollider == null) return;
            Gizmos.color = new Color(hitColor.r, hitColor.g, hitColor.b, 0.3f);
            switch (_hitCollider)
            {
                case BoxCollider2D boxCollider:
                    Gizmos.DrawCube(_hitCollider.transform.position + (Vector3)boxCollider.offset, boxCollider.size);
                    break;
                case CircleCollider2D circleCollider:
                    Gizmos.DrawSphere(_hitCollider.transform.position + (Vector3)circleCollider.offset, circleCollider.radius);
                    break;
            }
        }
    }

    public interface IInteractable
    {
        void Interact();
    }
}