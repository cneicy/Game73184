using UnityEngine;

namespace GamePlay.Objects.Towers.Strategy
{
    public class CoolDownTowerStrategy : BaseTowerStrategy,ITowerStrategy
    {
        [SerializeField] public float coolDown;
        public void ApplyUpgrade(AbstractTower tower)
        {
            tower.CoolDown -= coolDown;
        }
    }
}