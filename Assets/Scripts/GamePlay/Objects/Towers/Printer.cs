using System;
using System.Collections;
using Event;
using UnityEngine;

namespace GamePlay.Objects.Towers
{
    public class Printer : AbstractTower
    {
        [SerializeField]private Slot parentSlot;
        [SerializeField]private Map map;
        private void Start()
        {
            DamageType = DamageType.Normal;
            TowerType = TowerType.Recorder;
            Damage = 300;
            AttackCoolDown = 1;
            PlaceCoolDown = 28;
            parentSlot = GetComponentsInParent<Slot>()[0];
            map = parentSlot.map;
            StartCoroutine(nameof(AttackCon));
            StartCoroutine(nameof(Living));
        }

        private IEnumerator Living()
        {
            yield return new WaitForSeconds(10f);
            Destroy(gameObject);
        }
        
        private IEnumerator AttackCon()
        {
            yield return new WaitForSeconds(AttackCoolDown);
            Attack();
            StartCoroutine(nameof(AttackCon));
        }
        

        protected override void Move()
        {
            
        }
    }
}