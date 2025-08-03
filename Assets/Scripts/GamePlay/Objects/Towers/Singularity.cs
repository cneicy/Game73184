using System;
using DG.Tweening;
using Event;
using UnityEngine;

namespace GamePlay.Objects.Towers
{
    public class Singularity : MonoBehaviour
    {
        [SerializeField]private Slot parentSlot;
        [SerializeField]private Map map;

        private void OnEnable()
        {
            parentSlot = GetComponentsInParent<Slot>()[0];
            map = parentSlot.map;
        }

        private void Start()
        {
            Boom();
        }

        private void Boom()
        {
            transform.DOScale(new Vector3(0.95f, 0.95f, 0.95f), 0.9f)
                .OnComplete(() =>
                {
                    EventManager.Instance.TriggerEvent("AttackHero", new TowerAttack(DamageType.Normal, 10000));
                    Destroy(gameObject);
                });
            
        }
    }
}
