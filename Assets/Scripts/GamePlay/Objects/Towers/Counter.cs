using System;
using UnityEngine;

namespace GamePlay.Objects.Towers
{
    public class Counter : MonoBehaviour
    {
        [SerializeField]private Slot parentSlot;
        [SerializeField]private Map map;

        private void OnEnable()
        {
            parentSlot = GetComponentsInParent<Slot>()[0];
            map = parentSlot.map;
        }

        private void Start()
        {
            Painting();
        }

        private void Painting()
        {
            for (int y1= 0; y1 < 9; y1+=2)
            {
                for (int x1 = 0; x1 < 12; x1+=2)
                {
                    int x2 = Mathf.Clamp(x1, 0, 11);  
                    int y2 = Mathf.Clamp(y1, 0, 8);
                    if (map.Region[x2,y2].gameObject.CompareTag("Road"))
                    {
                        map.Region[x2,y2].GetComponent<SpriteRenderer>().color = Color.red;
                        map.Region[x2,y2].isCounter = true;
                    }
                }
            }
        }
    }
}
