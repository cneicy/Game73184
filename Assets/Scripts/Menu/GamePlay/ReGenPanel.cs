using Event;
using GamePlay;
using TMPro;
using UnityEngine;

namespace Menu.GamePlay
{
    public class ReGenPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text tip;
        [SerializeField] private string[] tips;
        [SerializeField] private string[] retips;
        private bool _isPlayed;

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

        [EventSubscribe("NewGame")]
        public object NewGame(string anyway = "")
        {
            _isPlayed = true;
            return null;
        }
        
        [EventSubscribe("PowerOff")]
        public object PowerOff(string anyway = "")
        {
            if (GameManager.Instance.GameState != GameState.Playing) return null;
            tip.gameObject.SetActive(true);
            tip.text = !_isPlayed ? tips[Random.Range(0, tips.Length)] : retips[Random.Range(0, retips.Length)];
            return null;
        }

        [EventSubscribe("PowerOn")]
        public object PowerOn(string anyway = "")
        {
            if (GameManager.Instance.GameState != GameState.Playing) return null;
            tip.gameObject.SetActive(false);
            return null;
        }
    }
}