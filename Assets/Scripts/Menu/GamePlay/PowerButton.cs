using Event;
using UnityEngine;

namespace Menu.GamePlay
{
    public class PowerButton : MonoBehaviour
    {
        private bool _isOn;

        [EventSubscribe("PowerOff")]
        public object OnPowerOff(string anyway)
        {
            var temp = transform.localScale;
            temp.x = -1;
            _isOn = false;
            transform.localScale = temp;
            return null;
        }

        [EventSubscribe("PowerOn")]
        public object OnPowerOn(string anyway)
        {
            var temp = transform.localScale;
            temp.x = 1;
            _isOn = true;
            transform.localScale = temp;
            return null;
        }
        
        private void OnEnable()
        {
            if (EventManager.Instance)
                EventManager.Instance.RegisterEventHandlersFromAttributes(this);
        }

        private void OnDisable()
        {
            if (EventManager.Instance)
                EventManager.Instance.UnregisterAllEventsForObject(this);
        }
        
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