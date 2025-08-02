using System;
using GamePlay.HandCard;
using GamePlay.Objects.Towers;
using UnityEngine;

namespace GamePlay
{
    public class StoreSystem : MonoBehaviour
    {
        [SerializeField]private BuildTowerData _buildTowerData;
        [SerializeField] CardCurve cardCurve;
        private void Start()
        {
            _buildTowerData = GetComponent<BuildTowerData>();
        }
        
        
    }
}
