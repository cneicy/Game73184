using UnityEngine;

namespace GamePlay.Objects.Towers
{
    public class GhostNeedle : AbstractTower
    {
        [SerializeField]private Slot parentSlot;
        [SerializeField]private Map map;
        [SerializeField]private GameObject needle;
        
        private void OnEnable()
        {
            parentSlot = GetComponentsInParent<Slot>()[0];
            map = parentSlot.map;
            ShootNeedle();
        }

        private void Start()
        {
            parentSlot.needleTarget = true;
        }

        public void ShootNeedle()
        {
            foreach (var slot in map.Region)
            {
                if (!slot.IsOccupied)
                {
                    Instantiate(needle, slot.transform.position, Quaternion.identity,slot.transform);
                }
            }
        }

        public void DestroyItself()
        {
            Destroy(this.gameObject);
        }
        
        private void OnDestroy()
        {
            parentSlot.needleTarget = false;
        }

        protected override void Move()
        {
        }
    }
}
