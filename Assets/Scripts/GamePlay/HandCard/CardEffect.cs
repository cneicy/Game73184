using DG.Tweening;

using UnityEngine;
using Singleton;

namespace GamePlay.HandCard
{
    public class CardEffect : Singleton<CardEffect>
    {
        [SerializeField] private BuildingSystem buildingSystem;
        public void MouseEnter(Vector3 localPosition,Transform localTransform,SpriteRenderer spriteRenderer)
        {
            localTransform.DOLocalMove(localPosition+Vector3.up, 0.25f);
            spriteRenderer.sortingOrder += 10;
        }

        public void MouseExit(Vector3 localPosition,Transform localTransform,SpriteRenderer spriteRenderer)
        {
            buildingSystem.ChangeStateToWaiting();
            localTransform.DOLocalMove(localPosition-Vector3.up, 0.25f);
            spriteRenderer.sortingOrder -= 10;
        }

        public void CardFlip(bool flipped,Transform localTransform)
        {
            Vector3 newVec3 = -localTransform.localRotation.eulerAngles+new Vector3(0f,180f,0f);
            localTransform.DORotate(newVec3, 0.25f);
        }
    }
}
