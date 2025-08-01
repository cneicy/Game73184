using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GamePlay
{
    [RequireComponent(typeof(RawImage))]
    public class RenderTextureInteraction : MonoBehaviour, 
        IPointerClickHandler, 
        IPointerEnterHandler, 
        IPointerExitHandler
    {
        [Header("Camera References")]
        public Camera postProcessingCamera; // 后处理相机
    
        [Header("Debug Settings")]
        public bool showDebugRays = true;
        public Color rayColor = Color.red;
        public Color hitColor = Color.green;
        public Color hoverColor = Color.blue; // 新增悬停颜色
        public float gizmoSize = 0.2f;
        public bool verboseDebug = true;
    
        [Header("Hover Settings")]
        [Tooltip("悬停检测更新频率(秒)")]
        public float hoverCheckInterval = 0.05f; // 悬停检测间隔
    
        private RawImage _rawImage;
        private RectTransform _rectTransform;
    
        // 调试数据存储
        private Vector3 _debugRayOrigin = Vector3.zero;
        private Vector3 _debugRayDirection = Vector3.forward;
        private bool _hasHit;
        private Vector3 _hitPoint;
        private Collider2D _hitCollider;
        
        // 悬停状态管理
        private Collider2D _lastHoveredCollider;
        private bool _isPointerOverRawImage;
        private float _nextHoverCheckTime;
        private Vector2 _lastMousePosition;

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

        private void Update()
        {
            // 如果指针不在RawImage上，不需要检查悬停
            if (!_isPointerOverRawImage) return;
            
            // 根据设置的时间间隔检查悬停
            if (Time.time >= _nextHoverCheckTime)
            {
                CheckHover();
                _nextHoverCheckTime = Time.time + hoverCheckInterval;
            }
        }

        private void CheckHover()
        {
            // 获取当前鼠标位置
            Vector2 currentMousePosition = UnityEngine.Input.mousePosition;
            
            // 如果鼠标位置没有变化，不需要重新检测
            if (currentMousePosition == _lastMousePosition) return;
            
            _lastMousePosition = currentMousePosition;
            
            // 执行悬停检测
            var worldPoint = ScreenToWorldPoint(currentMousePosition);
            if (!worldPoint.HasValue) return;
            
            // 2D点检测
            var colliders = Physics2D.OverlapPointAll(worldPoint.Value);
            
            Collider2D currentHoveredCollider = null;
            if (colliders.Length > 0)
            {
                // 找到最上层的碰撞体
                currentHoveredCollider = GetTopmostCollider(colliders);
            }
            
            // 检查悬停状态是否变化
            if (currentHoveredCollider != _lastHoveredCollider)
            {
                // 处理离开上一个悬停物体
                if (_lastHoveredCollider != null)
                {
                    var hoverable = _lastHoveredCollider.GetComponent<IHoverable>();
                    if (hoverable != null) 
                    {
                        if (verboseDebug) Debug.Log($"悬停离开: {_lastHoveredCollider.name}");
                        hoverable.OnHoverExit();
                    }
                }
                
                // 处理进入新悬停物体
                if (currentHoveredCollider != null)
                {
                    var hoverable = currentHoveredCollider.GetComponent<IHoverable>();
                    if (hoverable != null) 
                    {
                        if (verboseDebug) Debug.Log($"悬停进入: {currentHoveredCollider.name}");
                        hoverable.OnHoverEnter();
                    }
                }
                
                // 更新最后悬停的碰撞体
                _lastHoveredCollider = currentHoveredCollider;
            }
            
            // 更新调试数据
            _debugRayOrigin = worldPoint.Value;
            _debugRayDirection = Vector3.forward;
            _hasHit = currentHoveredCollider != null;
            _hitCollider = currentHoveredCollider;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOverRawImage = true;
            _lastMousePosition = eventData.position;
            CheckHover(); // 立即检查悬停状态
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOverRawImage = false;
            
            // 处理离开悬停物体
            if (_lastHoveredCollider != null)
            {
                var hoverable = _lastHoveredCollider.GetComponent<IHoverable>();
                if (hoverable != null) 
                {
                    if (verboseDebug) Debug.Log($"悬停离开: {_lastHoveredCollider.name}");
                    hoverable.OnHoverExit();
                }
                _lastHoveredCollider = null;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // 重置调试数据
            _hasHit = false;
            _hitCollider = null;
        
            var worldPoint = ScreenToWorldPoint(eventData.position);
            if (!worldPoint.HasValue) return;
            
            // 2D点检测
            var colliders = Physics2D.OverlapPointAll(worldPoint.Value);
        
            if (verboseDebug) Debug.Log($"在 {worldPoint.Value} 处找到 {colliders.Length} 个碰撞体");
        
            if (colliders.Length > 0)
            {
                // 找到最上层的碰撞体
                _hitCollider = GetTopmostCollider(colliders);
                _hasHit = true;
                _hitPoint = worldPoint.Value;
            
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
                //Debug.LogWarning("没有命中任何碰撞体");
            }
        
            // 存储调试数据
            _debugRayOrigin = worldPoint.Value;
            _debugRayDirection = Vector3.forward;
        
            //Debug.Log($"=== 调试数据更新: 原点={_debugRayOrigin}, 命中={_hasHit} ===");
        }
        
        /// <summary>
        /// 将屏幕坐标转换为世界坐标
        /// </summary>
        private Vector3? ScreenToWorldPoint(Vector2 screenPosition)
        {
            // 1. 屏幕坐标转RawImage局部坐标
            var success = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform,
                screenPosition,
                null, // Overlay模式必须传null
                out var localPoint
            );
        
            if (!success)
            {
                Debug.LogError("屏幕坐标转局部坐标失败！");
                return null;
            }
        
            /*if (verboseDebug) Debug.Log($"屏幕坐标: {screenPosition} -> 局部坐标: {localPoint}");*/

            // 2. 计算归一化UV坐标（0-1范围）
            var rawImageRect = _rectTransform.rect;
            var normalizedPoint = new Vector2(
                (localPoint.x - rawImageRect.x) / rawImageRect.width,
                (localPoint.y - rawImageRect.y) / rawImageRect.height
            );
        
            /*if (verboseDebug) Debug.Log($"RawImage矩形: {rawImageRect} -> 归一化坐标: {normalizedPoint}");*/

            // 3. 处理自定义UV区域
            if (_rawImage.uvRect != new Rect(0, 0, 1, 1))
            {
                normalizedPoint = new Vector2(
                    normalizedPoint.x * _rawImage.uvRect.width + _rawImage.uvRect.x,
                    normalizedPoint.y * _rawImage.uvRect.height + _rawImage.uvRect.y
                );
                if (verboseDebug) Debug.Log($"调整后UV坐标: {normalizedPoint}");
            }

            // 4. 转换为后处理相机的世界坐标
            var depth = 10f; // 根据场景调整
            var viewportPoint = new Vector3(normalizedPoint.x, normalizedPoint.y, depth);
            return postProcessingCamera.ViewportToWorldPoint(viewportPoint);
        }

        // 获取最上层的碰撞体（根据渲染顺序）
        private Collider2D GetTopmostCollider(Collider2D[] colliders)
        {
            if (colliders == null || colliders.Length == 0) 
                return null;
            
            var topmost = colliders[0];
            var highestOrder = GetSortingOrder(topmost);
        
            for (int i = 1; i < colliders.Length; i++)
            {
                var col = colliders[i];
                var order = GetSortingOrder(col);
                if (order > highestOrder)
                {
                    highestOrder = order;
                    topmost = col;
                }
            }
            return topmost;
        }
        
        private int GetSortingOrder(Collider2D collider)
        {
            if (collider == null) return 0;
            
            var renderer = collider.GetComponent<Renderer>();
            return renderer != null ? renderer.sortingOrder : 0;
        }

        // Gizmos调试可视化
        private void OnDrawGizmos()
        {
            if (!showDebugRays || !Application.isPlaying) return;
        
            // 绘制检测点
            Gizmos.color = _isPointerOverRawImage ? hoverColor : rayColor;
            Gizmos.DrawSphere(_debugRayOrigin, gizmoSize);
        
            // 绘制射线方向
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);
            Gizmos.DrawRay(_debugRayOrigin, _debugRayDirection * 2f);
        
            // 绘制命中点
            if (!_hasHit) return;
            
            Gizmos.color = hitColor;
            Gizmos.DrawSphere(_debugRayOrigin, gizmoSize * 1.2f);
            
            // 绘制命中物体轮廓
            if (_hitCollider == null) return;
            Gizmos.color = new Color(hitColor.r, hitColor.g, hitColor.b, 0.3f);
            switch (_hitCollider)
            {
                case BoxCollider2D boxCollider:
                    Gizmos.DrawCube(boxCollider.bounds.center, boxCollider.bounds.size);
                    break;
                case CircleCollider2D circleCollider:
                    Gizmos.DrawSphere(circleCollider.bounds.center, circleCollider.radius);
                    break;
                case PolygonCollider2D polyCollider:
                    // 绘制多边形碰撞体轮廓
                    DrawPolygonGizmo(polyCollider);
                    break;
            }
        }
        
        // 绘制多边形碰撞体轮廓
        private void DrawPolygonGizmo(PolygonCollider2D collider)
        {
            if (collider == null || collider.pathCount == 0) return;
            
            Gizmos.color = new Color(hoverColor.r, hoverColor.g, hoverColor.b, 0.7f);
            Vector3 offset = collider.transform.position;
            
            for (int i = 0; i < collider.pathCount; i++)
            {
                Vector2[] path = collider.GetPath(i);
                for (int j = 0; j < path.Length; j++)
                {
                    Vector3 start = offset + (Vector3)path[j];
                    Vector3 end = offset + (Vector3)path[(j + 1) % path.Length];
                    Gizmos.DrawLine(start, end);
                }
            }
        }
    }

    /*public interface IInteractable
    {
        void Interact();
    }*/
    
    // 新增悬停接口
    public interface IHoverable
    {
        void OnHoverEnter();
        void OnHoverExit();
    }
}