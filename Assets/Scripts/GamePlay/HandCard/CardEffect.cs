using System;
using DG.Tweening;
using UnityEngine;
using Singleton;
using UnityEngine.Serialization;

namespace GamePlay.HandCard
{
    public class CardEffect : Singleton<CardEffect>
    {
        [SerializeField] private BuildingSystem buildingSystem;
        private float _x;

        private void Start()
        {
            _x = 110 / 255f;
        }

        public void MouseEnter(Vector3 localPosition,Transform localTransform,SpriteRenderer spriteRenderer)
        {
            spriteRenderer.sortingOrder += 10;
            spriteRenderer.color =new Color(_x, _x, _x, 0.65f);
            buildingSystem.state = BuildingSystem.BuiltState.Building;
        }

        public void MouseExit(Vector3 localPosition,Transform localTransform,SpriteRenderer spriteRenderer)
        {
            spriteRenderer.sortingOrder -= 10;
            spriteRenderer.color = Color.white;
            buildingSystem.state = BuildingSystem.BuiltState.Waiting;
        }

        public void CardFlip(bool flipped,Transform localTransform)
        {
            transform.DORotate(new Vector3(0, flipped ? 180f : 0, 0), 0.25f);
        }
    }
}
