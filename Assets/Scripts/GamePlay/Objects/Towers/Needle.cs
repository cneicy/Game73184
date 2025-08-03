using System;
using System.Collections;
using DG.Tweening;
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
        Sequence rotationSequence;
        public Ease moveEase; 
        public float moveDelayAfterRotation; 
        private GhostNeedle _ghostNeedle;

        private void Start()
        {
            rotationEase = Ease.OutBack;
            rotationSequence = DOTween.Sequence();
            moveEase = Ease.InOutSine; 
            moveDelayAfterRotation = 0.1f;
            
            parentSlot = GetComponentsInParent<Slot>()[0];
            map = parentSlot.map;
            foreach (var slot in map.Region)
            {
                if (slot.needleTarget)
                {
                    _targetSlot = slot;
                    _target = slot.transform;
                }
            }
            Shoot();
            Destroy(this);
        }

        private void Shoot()
        {
            if (_target == null) return;
            
            Vector2 direction = _target.position - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            rotationSequence?.Kill();
            rotationSequence = DOTween.Sequence();
        
            
            rotationSequence.Append(
                transform.DORotate(new Vector3(0, 0, targetAngle), 0.25f)
                    .SetEase(rotationEase));
            
            rotationSequence.Append(transform.DOMove(_target.transform.position, 1.25f)
                .SetEase(moveEase)
                .SetDelay(moveDelayAfterRotation));
            
        }

        private void OnDestroy()
        {
            if (_target.GetComponentInChildren<GhostNeedle>()!=null)
            {
                _target.GetComponentInChildren<GhostNeedle>().DestroyItself();
            }
        }
    }
}
