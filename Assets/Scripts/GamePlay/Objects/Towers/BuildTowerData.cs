using UnityEngine;

namespace GamePlay.Objects.Towers
{
    public class BuildTowerData : MonoBehaviour
    {
        public TowerType[] cardTowerType;
        public int[] cost;
        public GameObject[] targetTower;
        public bool[] canBuildOnRoad;
        public Sprite[] cardFace;
        public Sprite[] cardBack;
    }
}
