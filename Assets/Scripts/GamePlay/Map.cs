using UnityEngine;

namespace GamePlay
{
    public class Map : MonoBehaviour
    {
        public GameObject[,] MapSchema;
        public GameObject road;
        [Header("X为地图宽、Y为地图高")]
        public Vector2 widthAndHeight;

        private void Start()
        {
            MapSchema = new GameObject[(int)widthAndHeight.x, (int)widthAndHeight.y];
            GenerateMap();
        }

        public void GenerateMap()
        {
            // 清空现有地图
            ClearMap();
            
            // 计算总网格数
            int totalCells = (int)(widthAndHeight.x * widthAndHeight.y);
            
            // 计算目标内部区域大小 (75%-85%)
            int minInnerCells = Mathf.FloorToInt(totalCells * 0.75f);
            int maxInnerCells = Mathf.FloorToInt(totalCells * 0.85f);
            
            // 尝试不同的道路宽度
            int bestRoadWidth = 1;
            int bestInnerCells = 0;
            int bestDiff = int.MaxValue;
            
            // 道路宽度从1开始尝试，最大不超过地图尺寸的一半
            for (int roadWidth = 1; roadWidth <= Mathf.Min(widthAndHeight.x, widthAndHeight.y) / 2; roadWidth++)
            {
                // 计算当前道路宽度下的内部区域大小
                int innerWidth = (int)widthAndHeight.x - 2 * roadWidth;
                int innerHeight = (int)widthAndHeight.y - 2 * roadWidth;
                int innerCells = innerWidth * innerHeight;
                
                // 如果内部区域太小，停止尝试
                if (innerCells <= 0) break;
                
                // 计算与目标范围的差距
                int diff = 0;
                if (innerCells < minInnerCells)
                {
                    diff = minInnerCells - innerCells;
                }
                else if (innerCells > maxInnerCells)
                {
                    diff = innerCells - maxInnerCells;
                }
                else
                {
                    // 如果在目标范围内，直接使用
                    bestRoadWidth = roadWidth;
                    bestInnerCells = innerCells;
                    break;
                }
                
                // 记录最接近目标范围的配置
                if (diff < bestDiff)
                {
                    bestDiff = diff;
                    bestRoadWidth = roadWidth;
                    bestInnerCells = innerCells;
                }
            }
            
            // 生成地图
            for (int x = 0; x < widthAndHeight.x; x++)
            {
                for (int y = 0; y < widthAndHeight.y; y++)
                {
                    // 判断是否在道路环上
                    if (x < bestRoadWidth || x >= widthAndHeight.x - bestRoadWidth ||
                        y < bestRoadWidth || y >= widthAndHeight.y - bestRoadWidth)
                    {
                        Vector3 position = new Vector3(x, y, 0);
                        MapSchema[x, y] = Instantiate(road, position, Quaternion.identity, transform);
                    }
                }
            }
            
            // 输出调试信息
            float percentage = (float)bestInnerCells / totalCells * 100;
            Debug.Log($"地图生成完成! 道路宽度: {bestRoadWidth}, 内部区域: {bestInnerCells}个格子 ({percentage:F1}%)");
        }
        
        private void ClearMap()
        {
            if (MapSchema == null) return;
            
            for (int x = 0; x < widthAndHeight.x; x++)
            {
                for (int y = 0; y < widthAndHeight.y; y++)
                {
                    if (MapSchema[x, y] != null)
                    {
                        Destroy(MapSchema[x, y]);
                        MapSchema[x, y] = null;
                    }
                }
            }
        }
    }
}