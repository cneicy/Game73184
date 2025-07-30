using UnityEngine;

namespace GamePlay
{
    public class Map : MonoBehaviour
    {
        
        public GameObject[,] MapSchema;
        
        [Tooltip("X为地图宽、Y为地图高")]
        public Vector2 widthAndHeight;

        private void Start()
        {
            MapSchema = new GameObject[(int)widthAndHeight.x, (int)widthAndHeight.y];//不同于C++，此处数组内为空
        }
        
    }
}
