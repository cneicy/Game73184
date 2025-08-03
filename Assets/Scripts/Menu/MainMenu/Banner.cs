using Event;
using UnityEngine;

namespace Menu.MainMenu
{
    public class Banner : MonoBehaviour
    {
        [SerializeField] private GameObject banner;
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

        [EventSubscribe("PowerOff")]
        public object Hide(string anyway)
        {
            banner.SetActive(false);
            return null;
        }
    }
}
