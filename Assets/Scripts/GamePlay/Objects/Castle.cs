using System.Collections.Generic;
using Event;
using GamePlay.Objects.Heroes;
using UnityEngine;

namespace GamePlay.Objects
{
    public class Castle : MonoBehaviour
    {
        private Map _map;
        private List<Vector2> _pathPoints;

        [EventSubscribe("PowerOn")]
        public object GameStart(string anyway)
        {
            Debug.Log("Cas");
            Invoke(nameof(Spawn),0.1f);
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
            var temp = other.GetComponent<Hero>();
            Debug.Log($"Hero HP Left:{temp.Health}");
            temp.Health = Hero.MaxHealth;
        }
    }
}