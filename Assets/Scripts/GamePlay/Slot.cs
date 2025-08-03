using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay
{
    public class Slot : MonoBehaviour,IInteractable,IHoverable
    {
        public Vector2Int gridPosition; // 在地图网格中的位置
        public GameObject placedUnit;    // 放置在slot上的单位
        public bool isOuterSlot;        // 是否是外部slot（道路外部区域）
        public bool IsOccupied => placedUnit != null; // 是否已被占用
        [SerializeField] private GameObject towerGameObject;
        [SerializeField] private GameObject shadowGameObject;
        public Map map;
    
        // 放置单位到slot
        public bool PlaceUnit(GameObject unit)
        {
            if (IsOccupied) return false; // 如果已被占用，返回失败
        
            placedUnit = unit;
            unit.transform.position = transform.position;
            unit.transform.parent = transform; // 可选：将单位设为slot的子对象
        
            return true;
        }

        private void OnEnable()
        {
            map = GetComponentInParent<Map>();
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            if (BuildingSystem.Instance.state == BuildingSystem.BuiltState.Building)
            {
                towerGameObject = BuildingSystem.Instance.NowCard.TowerPrefab;
                
                /*shadowGameObject = BuildingSystem.Instance.NowCard.TowerPrefab;*/
                /*Color alpha = shadowGameObject.GetComponent<SpriteRenderer>().color;*/
                /*shadowGameObject.GetComponent<SpriteRenderer>().color = new Color(alpha.r, alpha.g, alpha.b, 0.65f);*/
            }
        }

        // 从slot移除单位
        public bool RemoveUnit()
        {
            if (!IsOccupied) return false; // 如果没有单位，返回失败
        
            placedUnit.transform.parent = null; // 可选：解除父子关系
            placedUnit = null;
        
            return true;
        }
        public void Interact()
        {
            if (BuildingSystem.Instance.state == BuildingSystem.BuiltState.Building)
            {
                Instantiate(towerGameObject,transform.position,transform.localRotation,transform);
                if (BuildingSystem.Instance.NowCard.nowChargingIndex == 0)
                {
                    BuildingSystem.Instance.state = BuildingSystem.BuiltState.Waiting;
                }
            }
            print("建造");
        }

        public void OnHoverEnter()
        {
            /*if (isBuilding)
            { 
                Instantiate(shadowGameObject,transform.position,transform.localRotation,transform);
            }*/
            print("该地块位于"+gridPosition.ToString());
        }

        public void OnHoverExit()
        {
            /*print("鼠标退出地块");*/
        }
    }
}