using UnityEngine;

namespace GamePlay.Objects.Heros
{
    public class Hero : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            print("Hero");
        }
    }
}