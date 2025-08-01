using UnityEngine;
using Singleton;

namespace GamePlay.HandCard
{
    public class CardEffect : Singleton<CardEffect>
    {
        [SerializeField] private BuildingSystem buildingSystem;
        
        public void MouseEnter(Vector3 localPosition,Transform localTransform,SpriteRenderer spriteRenderer)
        {
            //Vector3 targetPosition = localPosition+Vector3.up;
            //localTransform.DOLocalMove(targetPosition, 0.25f);
            spriteRenderer.sortingOrder += 10;
            spriteRenderer.color = Color.black;
            buildingSystem.state = BuildingSystem.BuiltState.Building;
        }

        public void MouseExit(Vector3 localPosition,Transform localTransform,SpriteRenderer spriteRenderer)
        {
            //Vector3 targetPosition = localPosition-Vector3.up;
            //localTransform.DOLocalMove(targetPosition, 0.25f);
            spriteRenderer.sortingOrder -= 10;
            spriteRenderer.color = Color.white;
            buildingSystem.state = BuildingSystem.BuiltState.Waiting;
        }
    }
}
