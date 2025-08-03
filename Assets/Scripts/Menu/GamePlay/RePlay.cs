using Event;
using GamePlay;
using GamePlay.Objects.Heroes;
using TMPro;
using UnityEngine;

namespace Menu.GamePlay
{
    public class RePlay : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Hero hero;
        [SerializeField] private GameObject panel;

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

        [EventSubscribe("PowerOn")]
        public object PowerOn(string anyway)
        {
            if(GameManager.Instance.GameState == GameState.GameOver) return null;
            panel.SetActive(false);
            hero.Health = Hero.MaxHealth;
            return null;
        }

        [EventSubscribe("RePlay")]
        public object OnRePlay(string anyway = "")
        {
            panel.SetActive(true);
            healthText.text = "Hero HP Left: " + hero.Health;
            return null;
        }
    }
}