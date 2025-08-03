using Event;
using GamePlay;
using UnityEngine;

namespace Menu.GamePlay
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private GameObject[] tutorial;
        [SerializeField] private GameObject textGroup;
        private int _index;
        private bool _isEnabled;
        
        private void OnEnable()
        {
            if (EventManager.Instance)
                EventManager.Instance.RegisterEventHandlersFromAttributes(this);
            textGroup.SetActive(false);
            for (var i = 0; i < tutorial.Length; i++)
            {
                tutorial[i].SetActive(i == _index);
            }
        }

        private void OnDisable()
        {
            if (EventManager.Instance)
                EventManager.Instance.UnregisterAllEventsForObject(this);
        }

        [EventSubscribe("PowerOn")]
        public object PowerOn(string anyway)
        {
            textGroup.SetActive(false);
            return null;
        }
        
        [EventSubscribe("Tutorial")]
        public object OnTutorialStart(string anyway)
        {
            Invoke(nameof(StartDelay),0.5f);
            return null;
        }

        private void StartDelay()
        {
            if (GameManager.Instance.GameState == GameState.Tutorial)
            {
                textGroup.SetActive(true);
            }
        }

        [EventSubscribe("TurningNext")]
        public object NextTutorial(GameState gameState)
        {
            if(gameState != GameState.Tutorial) return null;
            if (_index + 1 == tutorial.Length)
            {
                _index = 0;
            }else _index++;

            for (var i = 0; i < tutorial.Length; i++)
            {
                tutorial[i].SetActive(i == _index);
            }
            return null;
        }

        [EventSubscribe("TurningPrevious")]
        public object PreviousTutorial(GameState gameState)
        {
            if (gameState != GameState.Tutorial) return null;
            if (_index - 1 < 0)
            {
                _index =  tutorial.Length - 1;
            }else _index--;
            
            for (var i = 0; i < tutorial.Length; i++)
            {
                tutorial[i].SetActive(i == _index);
            }
            return null;
        }
    }
}