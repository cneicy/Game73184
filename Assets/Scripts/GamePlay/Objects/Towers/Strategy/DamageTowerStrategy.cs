using UnityEngine;

namespace GamePlay.Objects.Towers.Strategy
{
    public class DamageTowerStrategy : BaseTowerStrategy,ITowerStrategy
    {
        [SerializeField] public int damage;
        public void ApplyUpgrade(AbstractTower tower)
        {
            tower.Damage += damage;
        }
    }
}