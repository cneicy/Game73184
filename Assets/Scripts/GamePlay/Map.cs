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
            ClearMap();
            
            // 计算总网格数
            var totalCells = (int)(widthAndHeight.x * widthAndHeight.y);
            
            // 计算目标内部区域大小 (75%-85%)
            var minInnerCells = Mathf.FloorToInt(totalCells * 0.75f);
            var maxInnerCells = Mathf.FloorToInt(totalCells * 0.85f);
            
            // 尝试不同的道路宽度
            var bestRoadWidth = 1;
            var bestInnerCells = 0;
            var bestDiff = int.MaxValue;
            
            // 道路宽度从1开始尝试，最大不超过地图尺寸的一半
            for (var roadWidth = 1; roadWidth <= Mathf.Min(widthAndHeight.x, widthAndHeight.y) / 2; roadWidth++)
            {
                // 计算当前道路宽度下的内部区域大小
                var innerWidth = (int)widthAndHeight.x - 2 * roadWidth;
                var innerHeight = (int)widthAndHeight.y - 2 * roadWidth;
                var innerCells = innerWidth * innerHeight;
                
                if (innerCells <= 0) break;
                
                int diff;
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
                    bestRoadWidth = roadWidth;
                    bestInnerCells = innerCells;
                    break;
                }
                
                if (diff >= bestDiff) continue;
                bestDiff = diff;
                bestRoadWidth = roadWidth;
                bestInnerCells = innerCells;
            }
            
            roadWidth = bestRoadWidth;
            
            for (var x = 0; x < widthAndHeight.x; x++)
            {
                for (var y = 0; y < widthAndHeight.y; y++)
                {
                    if (x >= bestRoadWidth && !(x >= widthAndHeight.x - bestRoadWidth) &&
                        y >= bestRoadWidth && !(y >= widthAndHeight.y - bestRoadWidth)) continue;
                    var position = new Vector3(
                        startPosition.x + x * gridSize,
                        startPosition.y - y * gridSize,
                        0
                    );
                        
                    MapSchema[x, y] = Instantiate(road, position, Quaternion.identity, transform);
                }
            }
            
            // 输出调试信息
            var percentage = (float)bestInnerCells / totalCells * 100;
            Debug.Log($"地图生成完成! 道路宽度: {bestRoadWidth}, 内部区域: {bestInnerCells}个格子 ({percentage:F1}%)");
            Debug.Log($"地图起始位置: ({startPosition.x}, {startPosition.y}), 格子大小: {gridSize}");
        }
        
        private void ClearMap()
        {
            if (MapSchema == null) return;
            
            for (var x = 0; x < widthAndHeight.x; x++)
            {
                for (var y = 0; y < widthAndHeight.y; y++)
                {
                    if (!MapSchema[x, y]) continue;
                    Destroy(MapSchema[x, y]);
                    MapSchema[x, y] = null;
                }
            }
        }
        
        public List<Vector2> GetRoadPathPoints()
        {
            var pathPoints = new List<Vector2>();
            
            var width = (int)widthAndHeight.x;
            var height = (int)widthAndHeight.y;
            
            // 上边：从左到右
            for (var x = roadWidth; x < width - roadWidth; x++)
            {
                var worldX = startPosition.x + x * gridSize;
                var worldY = startPosition.y;
                pathPoints.Add(new Vector2(worldX, worldY));
            }
            
            // 右边：从上到下
            for (var y = roadWidth; y < height - roadWidth; y++)
            {
                var worldX = startPosition.x + (width - 1) * gridSize;
                var worldY = startPosition.y - y * gridSize;
                pathPoints.Add(new Vector2(worldX, worldY));
            }
            
            // 下边：从右到左
            for (var x = width - roadWidth - 1; x >= roadWidth; x--)
            {
                var worldX = startPosition.x + x * gridSize;
                var worldY = startPosition.y - (height - 1) * gridSize;
                pathPoints.Add(new Vector2(worldX, worldY));
            }
            
            // 左边：从下到上
            for (var y = height - roadWidth - 1; y >= roadWidth; y--)
            {
                var worldX = startPosition.x;
                var worldY = startPosition.y - y * gridSize;
                pathPoints.Add(new Vector2(worldX, worldY));
            }
            
            return pathPoints;
        }
    }
}