using UnityEngine;

namespace GamePlay
{
    public class Slot : MonoBehaviour
    {
        public Vector2Int gridPosition; // 在地图网格中的位置
        public GameObject placedUnit;    // 放置在slot上的单位
        public bool isOuterSlot;        // 是否是外部slot（道路外部区域）
        public bool IsOccupied => placedUnit != null; // 是否已被占用
    
        // 放置单位到slot
        public bool PlaceUnit(GameObject unit)
        {
            if (IsOccupied) return false; // 如果已被占用，返回失败
        
            placedUnit = unit;
            unit.transform.position = transform.position;
            unit.transform.parent = transform; // 可选：将单位设为slot的子对象
        
            return true;
        }
    
        // 从slot移除单位
        public bool RemoveUnit()
        {
            if (!IsOccupied) return false; // 如果没有单位，返回失败
        
            placedUnit.transform.parent = null; // 可选：解除父子关系
            placedUnit = null;
        
            return true;
        }
    }
}