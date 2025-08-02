using System;
using GamePlay.Objects.Towers;
using UnityEngine;

namespace GamePlay
{
    public class StoreSystem : MonoBehaviour
    {
        private BuildTowerData _buildTowerData;
        private void Start()
        {
            _buildTowerData = GetComponent<BuildTowerData>();
        }
    }
}
