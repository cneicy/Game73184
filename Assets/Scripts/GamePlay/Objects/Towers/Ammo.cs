using Event;
using UnityEngine;

namespace GamePlay.Objects.Towers
{
    public class Ammo : MonoBehaviour
    {
        public int damage;

        public int Damage
        {
            get => damage;
            set => damage = value;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hero"))
            {
                EventManager.Instance.TriggerEvent("AttackHero", damage);
            }
        }
    }
}