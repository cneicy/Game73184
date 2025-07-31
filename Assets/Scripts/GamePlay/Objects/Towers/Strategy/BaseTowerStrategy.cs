using UnityEngine;

namespace GamePlay.Objects.Towers.Strategy
{
    public class BaseTowerStrategy : MonoBehaviour
    {
        [SerializeField] public string upgradeName;
        [SerializeField] public float upgradeCost;
    }
}