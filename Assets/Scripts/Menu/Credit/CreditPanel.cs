using Event;
using UnityEngine;
using Visual;

namespace Menu.Credit
{
    public class CreditPanel : MonoBehaviour
    {
        [SerializeField] private TextScroll textScroll;
        
        
        public void OpenCredit()
        {
            TVPowerController.Instance.TurnOffTV();
            EventManager.Instance.TriggerEvent("OpenCredit","");
            textScroll.gameObject.SetActive(true);
        }

        public void CloseCredit()
        {
            TVPowerController.Instance.TurnOnTV();
            textScroll.gameObject.SetActive(false);
        }

        public void SpeedUp()
        {
            textScroll.ScrollSpeed = 60;
        }

        public void ResetSpeed()
        {
            textScroll.ScrollSpeed = 30;
        }
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift) || UnityEngine.Input.GetKeyDown(KeyCode.RightShift))
            {
                SpeedUp();
            }
            else ResetSpeed();
        }
    }
}