using Event;
using UnityEngine;
using Visual;

namespace Menu.Credit
{
    public class CreditPanel : MonoBehaviour
    {
        [SerializeField] private TextScroll textScroll;
        
        private void OnEnable()
        {
            if(EventManager.Instance)
                EventManager.Instance.RegisterEventHandlersFromAttributes(this);
        }

        private void OnDisable()
        {
            if(EventManager.Instance)
                EventManager.Instance.UnregisterAllEventsForObject(this);
        }
        
        [EventSubscribe("GameOver")]
        public object OpenCredit(string anyway)
        {
            TVPowerController.Instance.TurnOffTV();
            EventManager.Instance.TriggerEvent("OpenCredit","");
            textScroll.gameObject.SetActive(true);
            return null;
        }

        public void CloseCredit()
        {
            TVPowerController.Instance.TurnOnTV();
            textScroll.gameObject.SetActive(false);
        }

        public void SpeedUp()
        {
            textScroll.ScrollSpeed = 120;
        }

        public void ResetSpeed()
        {
            textScroll.ScrollSpeed = 30;
        }
        private void Update()
        {
            if (UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift))
            {
                SpeedUp();
            }
            else ResetSpeed();
        }
    }
}