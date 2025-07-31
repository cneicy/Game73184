using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace GamePlay
{
    public class Map : MonoBehaviour
    {
        public GameObject[,] MapSchema;
        public GameObject road;
        public GameObject slotPrefab;

        [Header("X为地图宽、Y为地图高")]
        public Vector2Int size = new Vector2Int(20, 20);

        [Header("地图左上角起始点")]
        public Vector2 startPosition = Vector2.zero;

        [Header("每格大小")]
        public float gridSize = 1f;

        [Header("道路离边缘的距离")]
        public int outerMargin = 2;

        [Header("道路宽度")]
        public int roadWidth = 1;
        
        [SerializeField] private float innerAreaPercentage;

        private void Start()
        {
            MapSchema = new GameObject[size.x, size.y];
            GenerateMap();
            StartCoroutine(ReGen());
        }

        private IEnumerator ReGen()
        {
            yield return new WaitForSeconds(2f);
            GenerateMap();
            StartCoroutine(ReGen());
        }

        public void GenerateMap()
        {
            ClearMap();
            
            // 计算内部区域尺寸
            var innerWidth = size.x - 2 * (outerMargin + roadWidth);
            var innerHeight = size.y - 2 * (outerMargin + roadWidth);

            // 计算内部区域占比
            var totalCells = size.x * size.y;
            var innerCells = innerWidth * innerHeight;
            innerAreaPercentage = (float)innerCells / totalCells * 100;

            // 生成地图
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    var pos = new Vector3(
                        startPosition.x + x * gridSize,
                        startPosition.y - y * gridSize,
                        0f);

                    // 判断是否在道路区域
                    var isRoad = IsInRoadArea(x, y);

                    var cell = isRoad
                        ? Instantiate(road, pos, Quaternion.identity, transform)
                        : Instantiate(slotPrefab, pos, Quaternion.identity, transform);

                    MapSchema[x, y] = cell;

                    // 给 slot 存网格坐标
                    var slot = cell.GetComponent<Slot>();
                    if (slot)
                        slot.gridPosition = new Vector2Int(x, y);
                }
            }

            Debug.Log($"地图生成完成！道路宽度={roadWidth}，道路离边缘={outerMargin}格，内部区域占比={innerAreaPercentage:F1}%");
        }

        // 判断一个点是否在道路区域内
        private bool IsInRoadArea(int x, int y)
        {
            // 道路区域：从边缘向内outerMargin格开始，宽度为roadWidth
            // 上边道路
            if (y >= outerMargin && y < outerMargin + roadWidth && 
                x >= outerMargin && x < size.x - outerMargin)
                return true;
            
            // 下边道路
            if (y >= size.y - outerMargin - roadWidth && y < size.y - outerMargin && 
                x >= outerMargin && x < size.x - outerMargin)
                return true;
            
            // 左边道路
            if (x >= outerMargin && x < outerMargin + roadWidth && 
                y >= outerMargin && y < size.y - outerMargin)
                return true;

            // 右边道路
            return x >= size.x - outerMargin - roadWidth && x < size.x - outerMargin && 
                   y >= outerMargin && y < size.y - outerMargin;
        }

        private void ClearMap()
        {
            if (MapSchema == null) return;
            foreach (var go in MapSchema)
                if (go) DestroyImmediate(go);
        }

        // 获取道路中心线路径点
        public List<Vector2> GetRoadPathPoints()
        {
            var list = new List<Vector2>();
            var m = outerMargin + roadWidth / 2; // 道路中心离边缘格数
            var w = size.x;
            var h = size.y;

            // 上边：从左到右
            for (var x = m; x < w - m; x++)
                list.Add(new Vector2(startPosition.x + x * gridSize,
                                     startPosition.y - m * gridSize));
            
            // 右边：从上到下（跳过起点，避免重复）
            for (var y = m + 1; y < h - m; y++)
                list.Add(new Vector2(startPosition.x + (w - 1 - m) * gridSize,
                                     startPosition.y - y * gridSize));
            
            // 下边：从右到左（跳过起点，避免重复）
            for (var x = w - 2 - m; x >= m; x--)
                list.Add(new Vector2(startPosition.x + x * gridSize,
                                     startPosition.y - (h - 1 - m) * gridSize));
            
            // 左边：从下到上（跳过起点和终点，避免重复）
            for (var y = h - 2 - m; y > m; y--)
                list.Add(new Vector2(startPosition.x + m * gridSize,
                                     startPosition.y - y * gridSize));

            return list;
        }
    }
}