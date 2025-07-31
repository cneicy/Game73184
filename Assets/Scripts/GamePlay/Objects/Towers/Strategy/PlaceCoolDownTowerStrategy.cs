using UnityEngine;

namespace GamePlay.Objects.Towers.Strategy
{
    public class PlaceCoolDownTowerStrategy : BaseTowerStrategy,ITowerStrategy
    {
        [SerializeField] public float placeCoolDown;
        public void ApplyUpgrade(AbstractTower tower)
        {
            tower.PlaceCoolDown -= placeCoolDown;
        }
    }
}