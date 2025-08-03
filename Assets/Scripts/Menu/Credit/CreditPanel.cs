using Event;
using UnityEngine;
using Visual;

namespace Menu.Credit
{
    public class CreditPanel : MonoBehaviour
    {
        [SerializeField] private TextScroll textScroll;
        [SerializeField] private GameObject tipText;
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
            Invoke(nameof(Hide),2f);
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
            Hide();
        }

        private void Hide()
        {
            tipText.SetActive(false);
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