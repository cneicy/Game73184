using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GamePlay.TextureInteract
{
    public class TextureInteractSystem : MonoBehaviour,IPointerClickHandler
    {
        
        [Header("必需组件")]
        public Camera renderCamera; // 渲染纹理的相机
        public RawImage displayImage; // 显示渲染纹理的UI元素
        
        void Start()
        {
            // 确保有事件系统
            if (FindObjectOfType<EventSystem>() == null)
            {
                gameObject.AddComponent<EventSystem>();
                gameObject.AddComponent<StandaloneInputModule>();
            }
        }

        private void Update()
        {
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // 步骤1：获取鼠标在屏幕上的位置
            Vector2 mouseScreenPos = eventData.position;
            Debug.Log("步骤1: 鼠标屏幕位置 = " + mouseScreenPos);
            // 步骤2：转换为UI元素的局部坐标
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    displayImage.rectTransform, 
                    mouseScreenPos, 
                    null, 
                    out localPoint))
            {
                Debug.Log("步骤2: UI局部坐标 = " + localPoint);
            
                // 步骤3：转换为UV坐标 (0-1范围)
                Rect rect = displayImage.rectTransform.rect;
                Vector2 uv = new Vector2(
                    (localPoint.x - rect.x) / rect.width,
                    (localPoint.y - rect.y) / rect.height
                );
                Debug.Log("步骤3: UV坐标 = " + uv);
            
                // 步骤4：转换为渲染相机的世界坐标
                Vector3 worldPoint = renderCamera.ViewportToWorldPoint(new Vector3(uv.x, uv.y, 0));
                worldPoint.z = 0; // 确保Z坐标为0（2D）
                Debug.Log("步骤4: 世界坐标 = " + worldPoint);
                
                // 检测点击到的物体
                CheckForClickedObject(worldPoint);
            }
            
            // 检测点击到的物体
            void CheckForClickedObject(Vector2 worldPoint)
            {
                // 在2D世界中进行射线检测
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        
                if (hit.collider != null)
                {
                    Debug.Log("点击到物体: " + hit.collider.gameObject.name);
            
                    // 这里可以添加点击后的处理逻辑
                    // 例如：改变颜色、触发事件等
                    hit.collider.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
                }
                else
                {
                    Debug.Log("没有点击到任何物体");
                }
            }
        }
    }
}
