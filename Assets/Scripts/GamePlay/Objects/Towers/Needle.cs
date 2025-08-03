using System;
using System.Collections;
using DG.Tweening;
using Event;
using UnityEngine;

namespace GamePlay.Objects.Towers
{
    public class Needle : MonoBehaviour
    {
        [SerializeField] private Slot parentSlot;
        [SerializeField] private Map map;
        private Transform _target;
        private Slot _targetSlot;
        public Ease rotationEase;
        private Sequence _rotationSequence;
        public Ease moveEase; 
        public float moveDelayAfterRotation; 
        private GhostNeedle _ghostNeedle;
        private int _blockFlies;

        private void Start()
        {
            rotationEase = Ease.OutBack;
            _rotationSequence = DOTween.Sequence();
            moveEase = Ease.InOutSine; 
            moveDelayAfterRotation = 0.1f;
            
            parentSlot = GetComponentsInParent<Slot>()[0];
            map = parentSlot.map;
            foreach (var slot in map.Region)
            {
                if (!slot.needleTarget) continue;
                _targetSlot = slot;
                _target = slot.transform;
            }
            Shoot();
        }

        private void Shoot()
        {
            if (!_target) return;
            
            Vector2 direction = _target.position - transform.position;
            var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            _rotationSequence?.Kill();
            _rotationSequence = DOTween.Sequence();
        
            
            _rotationSequence.Append(
                transform.DORotate(new Vector3(0, 0, targetAngle), 0.25f)
                    .SetEase(rotationEase));
            
            _rotationSequence.Append(transform.DOMove(_target.transform.position, 1.25f)
                .SetEase(moveEase)
                .SetDelay(moveDelayAfterRotation));
            
            _rotationSequence.OnComplete(() => {
                Destroy(gameObject);
            });
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Road") || other.CompareTag("Slot"))
            {
                _blockFlies++;
                Debug.Log(_blockFlies);
            }
            if (other.CompareTag("Hero"))
            {
                EventManager.Instance.TriggerEvent("AttackHero", new TowerAttack(DamageType.Normal, _blockFlies * 60));
                _rotationSequence?.Kill();
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (_target && _target.GetComponentInChildren<GhostNeedle>())
            {
                _target.GetComponentInChildren<GhostNeedle>().DestroyItself();
            }
        }
    }
}