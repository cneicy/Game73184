using GamePlay.HandCard;
using GamePlay.Objects.Towers;
using UnityEngine;
using Singleton;

namespace GamePlay
{
    public class StoreSystem : Singleton<StoreSystem>
    {
        [SerializeField]private BuildTowerData _buildTowerData;
        [SerializeField] public CardCurve1 cardCurve;
        private void Start()
        {
            _buildTowerData = GetComponent<BuildTowerData>();
        }
        
        
    }
}
