using Event;
using Singleton;

namespace GamePlay
{
    public enum GameState
    {
        FirstStart,
        Playing,
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
            GameState = GameState.Playing;
            return null;
        }

        [EventSubscribe("GameOver")]
        public object GameOver(string anyway)
        {
            GameState = GameState.GameOver;
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