using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Objects.Heroes
{
    public class HeroTrack : MonoBehaviour
    {
       public Map map; // 地图引用
        public float moveSpeed = 5f;
        public bool faceMovementDirection = true; // 是否朝向移动方向
        
        private List<Vector2> _pathPoints; // 路径点列表
        private int _currentTargetIndex; // 当前目标点索引
        private bool _isMoving; // 是否正在移动
        private SpriteRenderer _spriteRenderer; // 精灵渲染器

        private void Start()
        {
            if (map == null)
            {
                Debug.LogError("Map reference is not set!");
                return;
            }
            
            // 获取精灵渲染器
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer component not found!");
                return;
            }
            
            // 获取道路路径点
            _pathPoints = map.GetRoadPathPoints();
            
            if (_pathPoints.Count > 0)
            {
                // 将Hero放置在第一个路径点
                transform.position = new Vector3(_pathPoints[0].x, _pathPoints[0].y, 0);
                _currentTargetIndex = 1;
                _isMoving = true;
            }
            else
            {
                Debug.LogError("No path points found!");
            }
        }

        private void Update()
        {
            if (!_isMoving || _pathPoints.Count == 0) return;
            
            // 获取当前目标点
            var targetPoint = _pathPoints[_currentTargetIndex];
            
            // 计算方向
            var direction = targetPoint - (Vector2)transform.position;
            direction.Normalize();
            
            // 移动Hero
            //transform.position = Vector2.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
            transform.position += (Vector3)direction * (moveSpeed * Time.deltaTime);
            
            // 检查是否到达目标点
            if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
            {
                // 移动到下一个路径点
                _currentTargetIndex = (_currentTargetIndex + 1) % _pathPoints.Count;
            }
            
            // 旋转Hero，使其朝向移动方向
            if (!faceMovementDirection || direction == Vector2.zero) return;
            // 计算旋转角度
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                
            // 调整角度，使精灵朝向正确方向
            // 默认情况下，精灵的右侧是朝向右方的
            // 如果你的精灵默认朝向是上，则使用 angle - 90
            transform.rotation = Quaternion.Euler(0, 0, angle);
                
            // 如果需要翻转精灵（例如，当向左移动时）
            _spriteRenderer.flipX = direction.x < 0;
        }
        
        // 可视化路径（仅在编辑器中可见）
        private void OnDrawGizmos()
        {
            if (_pathPoints == null || _pathPoints.Count == 0) return;
            
            Gizmos.color = Color.yellow;
            
            // 绘制路径点
            foreach (var point in _pathPoints)
            {
                Gizmos.DrawSphere(new Vector3(point.x, point.y, 0), 0.2f);
            }
            
            // 绘制路径线
            for (var i = 0; i < _pathPoints.Count; i++)
            {
                var currentPoint = _pathPoints[i];
                var nextPoint = _pathPoints[(i + 1) % _pathPoints.Count];
                Gizmos.DrawLine(new Vector3(currentPoint.x, currentPoint.y, 0), new Vector3(nextPoint.x, nextPoint.y, 0));
            }
        }
    }
}