using DG.Tweening;
using UnityEngine;
using Singleton;

namespace GamePlay.HandCard
{
    public class CardHover : Singleton<CardHover>
    {
        public void MouseEnter(Vector3 localPosition,Transform localTransform)
        {
            Vector3 targetPosition = localPosition+Vector3.up;
            localTransform.DOLocalMove(targetPosition, 0.25f);
        }

        public void MouseExit(Vector3 localPosition,Transform localTransform)
        {
            Vector3 targetPosition = localPosition-Vector3.up;
            localTransform.DOLocalMove(targetPosition, 0.25f);
        }
    }
}
