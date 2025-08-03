using Event;
using Singleton;
using UnityEngine;

namespace GamePlay
{
    public enum GameState
    {
        FirstStart,
        Tutorial,
        Playing,
        RePlay,
        GameOver
    }

    public class GameManager : Singleton<GameManager>
    {
        public GameState GameState { get; set; } = GameState.FirstStart;

        public void ClickPowerButton()
        {
            EventManager.Instance.TriggerEvent("PowerButtonClick", "114514");
        }

        [EventSubscribe("PowerOn")]
        public object GameStart(string anyway)
        {
            if (GameState == GameState.GameOver)
            {
                EventManager.Instance.TriggerEvent("NewGame", "");
                GameState = GameState.FirstStart;
                
            }else
                GameState = GameState.Playing;
            Debug.Log(GameState.ToString());
            return null;
        }


        [EventSubscribe("GameOver")]
        public object GameOver(string anyway)
        {
            GameState = GameState.GameOver;
            return null;
        }
        
        [EventSubscribe("RePlay")]
        public object RePlay(string anyway)
        {
            GameState = GameState.RePlay;
            return null;
        }

        [EventSubscribe("Tutorial")]
        public object Tutorial(string anyway)
        {
            GameState = GameState.Tutorial;
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
    }
}