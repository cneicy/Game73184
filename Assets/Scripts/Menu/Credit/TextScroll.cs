using UnityEngine;

namespace Menu.Credit
{
    public class TextScroll : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed;
        [SerializeField] private Transform top;
        [SerializeField] private Transform bottom;
        [SerializeField] private Transform[] text;

        public float ScrollSpeed
        {
            get => scrollSpeed;
            set => scrollSpeed = value;
        }

        private void Update()
        {
            foreach (var variable in text)
            {
                var temp = variable.localPosition;
                temp.y += scrollSpeed * Time.deltaTime;
                if (temp.y > top.localPosition.y || temp.y < bottom.localPosition.y)
                {
                    variable.gameObject.SetActive(false);
                }
                else
                {
                    variable.gameObject.SetActive(true);
                }

                variable.localPosition = temp;
            }
        }
    }
}