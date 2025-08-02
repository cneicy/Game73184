using UnityEngine;

namespace Menu.GamePlay
{
    public class PowerButton : MonoBehaviour
    {
        private bool _isOn;

        public void Click()
        {
            _isOn = !_isOn;
            var temp = transform.localScale;
            if (_isOn)
            {
                temp.x = -1;
            }else temp.x = 1;
            transform.localScale = temp;
        }
    }
}