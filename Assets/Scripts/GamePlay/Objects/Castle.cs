using System.Collections.Generic;
using Event;
using UnityEngine;

namespace GamePlay.Objects
{
    public class Castle : MonoBehaviour
    {
        private Map _map;
        private List<Vector2> _pathPoints;
        private bool _isSecondTime;

        [EventSubscribe("PowerOn")]
        public object GameStart(string anyway)
        {
            if(GameManager.Instance.GameState == GameState.GameOver) return null;
            Invoke(nameof(Spawn),0.1f);
            _isSecondTime = false;
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

        private void Spawn()
        {
            _map = FindFirstObjectByType<Map>();
            _pathPoints = _map.GetRoadPathPoints();

            if (_pathPoints.Count > 0)
            {
                transform.position = new Vector3(_pathPoints[0].x, _pathPoints[0].y, 0);
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Hero")) return;
            {
                if (_isSecondTime)
                    EventManager.Instance.TriggerEvent("RePlay", "1");
                else _isSecondTime = true;
            }
        }
    }
}