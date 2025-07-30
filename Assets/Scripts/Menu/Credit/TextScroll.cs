using UnityEngine;

namespace Menu.Credit
{
    public class TextScroll : MonoBehaviour
    {
        private Vector2 _temp;
        [SerializeField]
        private float scrollSpeed;
        private void Start()
        {
            _temp = transform.position;
        }

        private void Update()
        {
            _temp.y += scrollSpeed * Time.deltaTime;
            transform.position = _temp;
        }
    }
}