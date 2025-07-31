using System;
using UnityEngine;

namespace GamePlay.Objects
{
    public class Road : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            print("Road");
        }
    }
}
