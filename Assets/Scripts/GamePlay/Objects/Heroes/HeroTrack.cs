using System.Collections.Generic;
using Event;
using GamePlay.Objects.Towers;
using UnityEngine;

namespace GamePlay.Objects.Heroes
{
    public class HeroTrack : MonoBehaviour
    {
        public Map map;
        public float moveSpeed = 5f;
        public bool faceMovementDirection = true;

        private List<Vector2> _pathPoints;
        private int _currentTargetIndex;
        private bool _isMoving;
        private SpriteRenderer _spriteRenderer;
        
        private void OnEnable()
        {
            if(EventManager.Instance)
                EventManager.Instance.RegisterEventHandlersFromAttributes(this);
        }

        private void OnDisable()
        {
            if(EventManager.Instance)
                EventManager.Instance.UnregisterAllEventsForObject(this);
        }
        
        [EventSubscribe("AttackHero")]
        public object OnGetHurt(TowerAttack towerAttack)
        {
            //todo:可能存在英雄受击减速？ 反正先留着了
            return null;
        }

        private void Start()
        {
            if (!map)
            {
                Debug.LogError("Map引用未设置");
                return;
            }

            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer组件未找到");
                return;
            }

            _pathPoints = map.GetRoadPathPoints();

            if (_pathPoints.Count > 0)
            {
                transform.position = new Vector3(_pathPoints[0].x, _pathPoints[0].y, 0);
                _currentTargetIndex = 1;
                _isMoving = true;
            }
            else
            {
                Debug.LogError("无可用路径点");
            }
        }

        private void Update()
        {
            if (!_isMoving || _pathPoints.Count == 0) return;

            var targetPoint = _pathPoints[_currentTargetIndex];

            var direction = targetPoint - (Vector2)transform.position;
            direction.Normalize();

            //transform.position = Vector2.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
            transform.position += (Vector3)direction * (moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
            {
                _currentTargetIndex = (_currentTargetIndex + 1) % _pathPoints.Count;
            }

            if (!faceMovementDirection || direction == Vector2.zero) return;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);
            _spriteRenderer.flipX = direction.x < 0;
        }

        private void OnDrawGizmos()
        {
            if (_pathPoints == null || _pathPoints.Count == 0) return;

            Gizmos.color = Color.yellow;

            foreach (var point in _pathPoints)
            {
                Gizmos.DrawSphere(new Vector3(point.x, point.y, 0), 0.2f);
            }

            for (var i = 0; i < _pathPoints.Count; i++)
            {
                var currentPoint = _pathPoints[i];
                var nextPoint = _pathPoints[(i + 1) % _pathPoints.Count];
                Gizmos.DrawLine(new Vector3(currentPoint.x, currentPoint.y, 0),
                    new Vector3(nextPoint.x, nextPoint.y, 0));
            }
        }
    }
}