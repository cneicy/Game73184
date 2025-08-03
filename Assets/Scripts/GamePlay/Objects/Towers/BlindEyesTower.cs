using System;
using UnityEngine;

namespace GamePlay.Objects.Towers
{
    public class BlindEyesTower : AbstractTower
    {
        [SerializeField]private Slot _parentSlot;
        [SerializeField]private Map _map;
        
        protected override void Move()
        {
            
        }

        private void OnEnable()
        {
            _parentSlot = GetComponentsInParent<Slot>()[1];
            _map = _parentSlot.map;
        }

        private void Start()
        {
            int x = _parentSlot.gridPosition.x;
            int y = _parentSlot.gridPosition.y;

            for (int y1= y-1; y1 < y+2; y1++)
            {
                for (int x1 = x-1; x1 < x+2; x1++)
                {
                    int x2 = Mathf.Clamp(x1, 0, 11);  
                    int y2 = Mathf.Clamp(y1, 0, 8);  
                    _map.Region[x2,y2].GetComponent<SpriteRenderer>().color = Color.black;
                }
            }
        }

        private void OnDestroy()
        {
            int x = _parentSlot.gridPosition.x;
            int y = _parentSlot.gridPosition.y;

            for (int y1= y-1; y1 < y+2; y1++)
            {
                for (int x1 = x-1; x1 < x+2; x1++)
                {
                    int x2 = Mathf.Clamp(x1, 0, 11);  
                    int y2 = Mathf.Clamp(y1, 0, 8);  
                    _map.Region[x2,y2].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }
}
