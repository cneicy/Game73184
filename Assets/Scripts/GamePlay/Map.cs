using UnityEngine;
using System.Collections.Generic;

namespace GamePlay
{
    public class Map : MonoBehaviour
    {
        public GameObject[,] MapSchema;
        public GameObject road;
        [Header("X为地图宽、Y为地图高")]
        public Vector2 widthAndHeight;
        
        [Header("地图起始位置（左上角）")]
        public Vector2 startPosition = Vector2.zero;
        
        [Header("每个格子的大小")]
        public float gridSize = 1f;
        
        [Header("道路宽度（自动计算）")]
        public int roadWidth = 1;

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
            
            // 保存道路宽度供其他脚本使用
            roadWidth = bestRoadWidth;
            
            // 生成地图
            for (int x = 0; x < widthAndHeight.x; x++)
            {
                for (int y = 0; y < widthAndHeight.y; y++)
                {
                    // 判断是否在道路环上
                    if (x < bestRoadWidth || x >= widthAndHeight.x - bestRoadWidth ||
                        y < bestRoadWidth || y >= widthAndHeight.y - bestRoadWidth)
                    {
                        // 计算世界坐标位置（2D环境）
                        Vector3 position = new Vector3(
                            startPosition.x + x * gridSize,
                            startPosition.y - y * gridSize,  // 注意：这里减去y，因为y轴向下
                            0
                        );
                        
                        MapSchema[x, y] = Instantiate(road, position, Quaternion.identity, transform);
                    }
                }
            }
            
            // 输出调试信息
            float percentage = (float)bestInnerCells / totalCells * 100;
            Debug.Log($"地图生成完成! 道路宽度: {bestRoadWidth}, 内部区域: {bestInnerCells}个格子 ({percentage:F1}%)");
            Debug.Log($"地图起始位置: ({startPosition.x}, {startPosition.y}), 格子大小: {gridSize}");
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
        
        // 获取道路路径点（供Hero使用）
        public List<Vector2> GetRoadPathPoints()
        {
            List<Vector2> pathPoints = new List<Vector2>();
            
            int width = (int)widthAndHeight.x;
            int height = (int)widthAndHeight.y;
            float halfRoadWidth = roadWidth / 2f;
            
            // 上边：从左到右
            for (int x = roadWidth; x < width - roadWidth; x++)
            {
                float worldX = startPosition.x + x * gridSize;
                float worldY = startPosition.y;
                pathPoints.Add(new Vector2(worldX, worldY));
            }
            
            // 右边：从上到下
            for (int y = roadWidth; y < height - roadWidth; y++)
            {
                float worldX = startPosition.x + (width - 1) * gridSize;
                float worldY = startPosition.y - y * gridSize;
                pathPoints.Add(new Vector2(worldX, worldY));
            }
            
            // 下边：从右到左
            for (int x = width - roadWidth - 1; x >= roadWidth; x--)
            {
                float worldX = startPosition.x + x * gridSize;
                float worldY = startPosition.y - (height - 1) * gridSize;
                pathPoints.Add(new Vector2(worldX, worldY));
            }
            
            // 左边：从下到上
            for (int y = height - roadWidth - 1; y >= roadWidth; y--)
            {
                float worldX = startPosition.x;
                float worldY = startPosition.y - y * gridSize;
                pathPoints.Add(new Vector2(worldX, worldY));
            }
            
            return pathPoints;
        }
    }
}