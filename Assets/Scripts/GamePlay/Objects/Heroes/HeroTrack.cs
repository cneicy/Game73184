using System.Collections.Generic;
using Event;
using GamePlay.Objects.Towers;
using UnityEngine;

namespace GamePlay.Objects.Heroes
{
    public class HeroTrack : MonoBehaviour
    {
        private Map _map;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        
        public float moveSpeed;
        private List<Vector2> _pathPoints;
        private int _currentTargetIndex;
        private bool _isMoving;
        //private SquashWalkAnimation  _walkSequence;
        private static readonly int DirX = Animator.StringToHash("DirX");
        private static readonly int DirY = Animator.StringToHash("DirY");

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

        [EventSubscribe("AttackHero")]
        public object OnGetHurt(TowerAttack towerAttack)
        {
            //todo:可能存在英雄受击减速？ 反正先留着了
            return null;
        }

        private void Start()
        {
            _map = FindFirstObjectByType<Map>();
            if (!_map)
            {
                Debug.LogError("Map引用未设置");
                return;
            }

            //_walkSequence = GetComponentInChildren<SquashWalkAnimation>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();

            if (!_spriteRenderer)
            {
                Debug.LogError("SpriteRenderer组件未找到");
                return;
            }

            if (!_animator)
            {
                Debug.LogError("Animator组件未找到");
                return;
            }
        }

        [EventSubscribe("GameStart")]
        public object GameStart(string anyway)
        {
            Invoke(nameof(LoadPath),0.4f);
            return null;
        }
        
        public void LoadPath()
        {
            _pathPoints = _map.GetRoadPathPoints();

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
            if (!_isMoving || _pathPoints.Count == 0)
            {
                //_walkSequence.StopWalking();
                return;
            }

            //_walkSequence.StartWalking();
            var targetPoint = _pathPoints[_currentTargetIndex];

            var direction = targetPoint - (Vector2)transform.position;
            direction.Normalize();
            /*var distance = direction.magnitude;
            var speedFactor = Mathf.Clamp01(moveSpeed * Time.deltaTime / distance);

            _walkSequence.SetAnimationSpeed(speedFactor);*/

            transform.position += (Vector3)direction * (moveSpeed * Time.deltaTime);

            UpdateAnimationParameters(direction);

            // 检查是否到达目标点
            if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
            {
                _currentTargetIndex = (_currentTargetIndex + 1) % _pathPoints.Count;
            }
        }

        private void UpdateAnimationParameters(Vector2 direction)
        {
            if (!_animator || direction == Vector2.zero) return;

            // 将方向转换为整数值（-1, 0, 1）
            int dirX;
            int dirY;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                dirX = direction.x > 0 ? 1 : -1;
                dirY = 0;
            }
            else
            {
                dirX = 0;
                dirY = direction.y > 0 ? 1 : -1;
            }

            _animator.SetInteger(DirX, dirX);
            _animator.SetInteger(DirY, dirY);

            //Debug.Log($"Direction: {direction}, DirX: {dirX}, DirY: {dirY}");
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