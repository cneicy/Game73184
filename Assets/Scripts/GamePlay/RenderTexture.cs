using UnityEngine;
using UnityEngine.UI;

namespace GamePlay
{
    public class RenderTexture : MonoBehaviour,IInteractable
    {
        [Header("Camera References")]
        public Camera postProcessingCamera; // 后处理相机
        
        [Header("Debug Settings")]
        public bool showDebugRays = true;
        public Color rayColor = Color.red;
        public Color hitColor = Color.green;
        public float gizmoSize = 0.2f;
        public bool verboseDebug = true;
        
        [Header("Input Settings")]
        public KeyCode interactionKey = KeyCode.Mouse0; // 默认鼠标左键
        public bool requireKeyDown = true; // 是否需要按键按下才检测
        
        private RawImage _rawImage;
        private RectTransform _rectTransform;
        
        // 调试数据存储
        private Vector3 _debugRayOrigin = Vector3.zero;
        private Vector3 _debugRayDirection = Vector3.forward;
        private bool _hasHit;
        private Vector3 _hitPoint;
        private Collider2D _hitCollider;
        
        // 输入状态
        private bool _wasPressedLastFrame;
        
        // 鼠标位置属性（归一化坐标）
        private Vector2 _currentMousePosition;
        public Vector2 CurrentMousePosition => _currentMousePosition;
        
        // 世界坐标位置属性（新增）
        [SerializeField] private Vector3 currentWorldPosition;
        public Vector3 CurrentWorldPosition => currentWorldPosition;
        
        // 鼠标是否在RawImage区域内
        private bool _isMouseOverRawImage;
        public bool IsMouseOverRawImage => _isMouseOverRawImage;

        private void Awake()
        {
            _rawImage = GetComponent<RawImage>();
            _rectTransform = _rawImage.rectTransform;
            
            // 确保RawImage可以接收射线检测
            if (_rawImage.raycastTarget)
            {
                Debug.LogWarning("RawImage的Raycast Target已启用，这可能会干扰自定义交互检测。建议禁用。");
            }

            if (verboseDebug)
            {
                Debug.Log($"Canvas Render Mode: {GetComponentInParent<Canvas>().renderMode}");
                Debug.Log($"Post-Processing Camera: {postProcessingCamera.name}");
                Debug.Log($"后处理相机位置: {postProcessingCamera.transform.position}");
                Debug.Log($"后处理相机正交大小: {postProcessingCamera.orthographicSize}");
                Debug.Log($"交互按键: {interactionKey}");
            }
        }

        private void FixedUpdate()
        {
            // 更新鼠标位置（无论是否按下按键）
            UpdateMousePosition();
            
            // 检测按键状态
            bool isPressed = UnityEngine.Input.GetKey(interactionKey);
            bool justPressed = UnityEngine.Input.GetKeyDown(interactionKey);
            
            // 根据设置决定是否检测
            bool shouldCheck = requireKeyDown ? justPressed : isPressed;
            
            // 如果按键状态变化或持续按下时检测
            if (shouldCheck || (isPressed && !_wasPressedLastFrame))
            {
                ProcessMouseInteraction();
            }
            
            // 更新按键状态
            _wasPressedLastFrame = isPressed;
        }

        private void UpdateMousePosition()
        {
            // 获取鼠标位置
            Vector2 mousePosition = UnityEngine.Input.mousePosition;
            
            // 检查鼠标是否在RawImage区域内
            _isMouseOverRawImage = RectTransformUtility.RectangleContainsScreenPoint(
                _rectTransform, 
                mousePosition, 
                null // Overlay模式使用null
            );
            
            if (!_isMouseOverRawImage)
            {
                // 鼠标不在RawImage区域内，设置无效位置
                _currentMousePosition = new Vector2(-1, -1);
                currentWorldPosition = Vector3.negativeInfinity; // 使用负无穷表示无效位置
                return;
            }
            
            // 屏幕坐标转RawImage局部坐标（Overlay模式相机参数为null）
            bool success = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform,
                mousePosition,
                null, // 关键修复：Overlay模式必须传null
                out Vector2 localPoint
            );
            
            if (!success)
            {
                // 转换失败，设置无效位置
                _currentMousePosition = new Vector2(-1, -1);
                currentWorldPosition = Vector3.negativeInfinity;
                return;
            }
            
            // 计算归一化UV坐标（0-1范围）
            Rect rawImageRect = _rectTransform.rect;
            Vector2 normalizedPoint = new Vector2(
                (localPoint.x - rawImageRect.x) / rawImageRect.width,
                (localPoint.y - rawImageRect.y) / rawImageRect.height
            );
            
            // 处理自定义UV区域（如部分纹理显示）
            if (_rawImage.uvRect != new Rect(0, 0, 1, 1))
            {
                normalizedPoint = new Vector2(
                    normalizedPoint.x * _rawImage.uvRect.width + _rawImage.uvRect.x,
                    normalizedPoint.y * _rawImage.uvRect.height + _rawImage.uvRect.y
                );
            }
            
            // 更新当前鼠标位置（归一化坐标）
            _currentMousePosition = normalizedPoint;
            
            // 计算世界坐标（新增）
            float depth = 10f; // 根据你的场景调整这个值
            Vector3 viewportPoint = new Vector3(normalizedPoint.x, normalizedPoint.y, depth);
            currentWorldPosition = postProcessingCamera.ViewportToWorldPoint(viewportPoint);
            
            if (verboseDebug)
            {
                Debug.Log($"鼠标位置更新: 归一化={normalizedPoint}, 世界坐标={currentWorldPosition}, 在RawImage内: {_isMouseOverRawImage}");
            }
        }

        private void ProcessMouseInteraction()
        {
            // 重置调试数据
            _hasHit = false;
            _hitCollider = null;
            
            // 如果鼠标不在RawImage区域内，不进行交互检测
            if (!_isMouseOverRawImage)
            {
                if (verboseDebug) Debug.Log("鼠标不在RawImage区域内，跳过交互检测");
                return;
            }
            
            // 使用已经计算好的世界坐标
            Vector3 worldPoint = currentWorldPosition;
            
            if (verboseDebug) Debug.Log($"使用世界坐标进行交互检测: {worldPoint}");

            // 2D点检测（适用于2D场景）
            Collider2D[] colliders = Physics2D.OverlapPointAll(worldPoint);
            
            if (verboseDebug) Debug.Log($"在 {worldPoint} 处找到 {colliders.Length} 个碰撞体");
            
            if (colliders.Length > 0)
            {
                // 找到最上层的碰撞体（根据渲染顺序）
                _hitCollider = GetTopmostCollider(colliders);
                _hasHit = true;
                _hitPoint = worldPoint;
                
                Debug.Log($"命中: {_hitCollider.name} at {_hitPoint}");
                
                // 触发交互
                IInteractable interactable = _hitCollider.GetComponent<IInteractable>();
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
            Collider2D topmost = colliders[0];
            int highestOrder = topmost.GetComponent<Renderer>()?.sortingOrder ?? 0;
            
            foreach (var col in colliders)
            {
                int order = col.GetComponent<Renderer>()?.sortingOrder ?? 0;
                if (order > highestOrder)
                {
                    highestOrder = order;
                    topmost = col;
                }
            }
            return topmost;
        }

        // Gizmos调试可视化
        private void OnDrawGizmos()
        {
            if (!showDebugRays || !Application.isPlaying) return;
            
            // 绘制鼠标位置（即使没有点击）
            if (_isMouseOverRawImage)
            {
                // 使用已经计算好的世界坐标
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(currentWorldPosition, gizmoSize * 0.8f);
                
                // 绘制从相机到鼠标位置的线
                Gizmos.color = new Color(1, 1, 0, 0.3f);
                Gizmos.DrawLine(postProcessingCamera.transform.position, currentWorldPosition);
            }
            
            // 绘制检测点
            Gizmos.color = rayColor;
            Gizmos.DrawSphere(_debugRayOrigin, gizmoSize);
            
            // 绘制射线方向（2D场景中通常垂直于屏幕）
            Gizmos.color = new Color(rayColor.r, rayColor.g, rayColor.b, 0.5f);
            Gizmos.DrawRay(_debugRayOrigin, _debugRayDirection * 2f);
            
            // 绘制命中点
            if (_hasHit)
            {
                Gizmos.color = hitColor;
                Gizmos.DrawSphere(_hitPoint, gizmoSize * 1.5f);
                
                // 绘制命中物体轮廓
                if (_hitCollider != null)
                {
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
        }

        public void Interact()
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IInteractable
    {
        void Interact();
    }
}

