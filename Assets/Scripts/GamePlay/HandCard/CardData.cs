using GamePlay.Objects;
using UnityEngine;

namespace GamePlay.HandCard
{
    public class CardData : MonoBehaviour
    {
        public TowerType towerType;
        public int cost;
        public GameObject towerPrefab;
        public bool canBuildOnRoad;
        public Sprite cardFace;
        public Sprite cardBack;
    }
}
