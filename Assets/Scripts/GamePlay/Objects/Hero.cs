using System;
using UnityEngine;

namespace GamePlay.Objects
{
    public class Hero : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            print("Hero");
        }
    }
}