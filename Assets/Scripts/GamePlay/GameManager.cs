using Event;
using Singleton;
namespace GamePlay
{
    public class GameManager : Singleton<GameManager>
    {
        public void ClickPowerButton()
        {
            EventManager.Instance.TriggerEvent("PowerButtonClick", "114514");
        }
    }
}