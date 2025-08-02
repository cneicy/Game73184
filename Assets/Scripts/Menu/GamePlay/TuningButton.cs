using Event;
using GamePlay;
using UnityEngine;

namespace Menu.GamePlay
{
    public class TuningButton : MonoBehaviour
    {
        public void Next()
        {
            EventManager.Instance.TriggerEvent("TurningNext", GameManager.Instance.GameState);
        }

        public void Previous()
        {
            EventManager.Instance.TriggerEvent("TurningPrevious", GameManager.Instance.GameState);
        }
    }
}