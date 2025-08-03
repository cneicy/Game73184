using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Event;

namespace GamePlay
{
    public class Map : MonoBehaviour
    {
        public GameObject[,] MapSchema;
        public GameObject road;
        public GameObject slotPrefab;

        [Header("地图左上角起始点")] public Vector2 startPosition = Vector2.zero;

        [Header("每格大小")] public float gridSize = 1f;

        [Header("地图选择")] [Range(0, 3)] public int currentMapIndex;

        [Header("调试可视化")] public bool showBfsPath = true;
        public bool showOptimizedPath = true;

        [SerializeField] private float innerAreaPercentage;
        [SerializeField] private List<Vector2Int> bfsPath;
        [SerializeField] private List<Vector2Int> optimizedPath;
        
        public List<Slot> allSlots = new List<Slot>();

        private readonly int[,,] _schema =
        {
            // 地图0
            {
                { 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0 },
                { 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 0 },
                { 1, 1, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0 },
                { 0, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 0 },
                { 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }
            },
            // 地图1
            {
                { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0 },
                { 1, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 0 },
                { 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0 },
                { 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            },
            // 地图2
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0 },
                { 1, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 0 },
                { 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1 },
                { 1, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 1 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 }
            },
            // 地图3
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
                { 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1 },
                { 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1 },
                { 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1 }
            }
        };

        private Vector2Int _size;

        //用于防御塔索敌
        public Slot[,] Region;
        
        private void OnEnable()
        {
            if(EventManager.Instance)
                EventManager.Instance.RegisterEventHandlersFromAttributes(this);
        }

        private void OnDisable()
        {
            if(EventManager.Instance)
                EventManager.Instance.UnregisterAllEventsForObject(this);
        }
        
        [EventSubscribe("PowerOn")]
        public object GameStart(string anyway)
        {
            if(GameManager.Instance.GameState == GameState.GameOver) return null;
            _size = new Vector2Int(_schema.GetLength(2), _schema.GetLength(1));

            Debug.Log($"地图尺寸: {_size.x}x{_size.y}, 当前地图索引: {currentMapIndex}");
            Debug.Log($"Schema数组维度: [{_schema.GetLength(0)}, {_schema.GetLength(1)}, {_schema.GetLength(2)}]");

            Region = new Slot[_size.x,_size.y];
            MapSchema = new GameObject[_size.x, _size.y];
            var rand = Random.Range(0, _schema.GetLength(0));
            SwitchToMap(rand);
            return null;
        }

        public void SwitchMap()
        {
            SwitchToMap(currentMapIndex);
        }

        public void GenerateMap()
        {
            ClearMap();

            var roadCount = 0;

            for (var y = 0; y < _size.y; y++)
            {
                for (var x = 0; x < _size.x; x++)
                {
                    var pos = new Vector3(
                        startPosition.x + x * gridSize,
                        startPosition.y - y * gridSize,
                        0f);

                    var cellType = _schema[currentMapIndex, y, x];
                    var isRoad = cellType == 1;

                    if (isRoad) roadCount++;

                    var cell = isRoad
                        ? Instantiate(road, pos, Quaternion.identity, transform)
                        : Instantiate(slotPrefab, pos, Quaternion.identity, transform);
                    
                    Region[x,y] = cell.GetComponent<Slot>();
                    
                    if (!isRoad)
                    {
                        allSlots.Add(cell.GetComponent<Slot>());
                    }

                    MapSchema[x, y] = cell;

                    var slot = cell.GetComponent<Slot>();
                    if (slot)
                        slot.gridPosition = new Vector2Int(x, y);
                }
            }

            var totalCells = _size.x * _size.y;
            var innerCells = totalCells - roadCount;
            innerAreaPercentage = (float)innerCells / totalCells * 100;

            Debug.Log($"地图 {currentMapIndex + 1} 生成完成！道路格子数={roadCount}，内部区域占比={innerAreaPercentage:F1}%");
        }

        public List<Vector2> GetRoadPathPoints()
        {
            var pathPoints = new List<Vector2>();

            var startPoint = FindStartPoint();
            if (startPoint == Vector2Int.one * -1)
            {
                Debug.LogError("没有找到道路起始点");
                return pathPoints;
            }

            bfsPath = BfsGetAllRoadCells(startPoint);
            Debug.Log($"BFS找到 {bfsPath.Count} 个道路格子");

            optimizedPath = GenerateCircularPath(bfsPath, startPoint);
            Debug.Log($"生成连续路径，共 {optimizedPath.Count} 个路径点");

            pathPoints.AddRange(optimizedPath.Select(gridPoint =>
                new Vector2(startPosition.x + gridPoint.x * gridSize, startPosition.y - gridPoint.y * gridSize)));

            return pathPoints;
        }

        private Vector2Int FindStartPoint()
        {
            Debug.Log($"寻找地图 {currentMapIndex} 的起始点, 地图尺寸: {_size.x}x{_size.y}");

            for (var y = 0; y < _size.y; y++)
            {
                for (var x = 0; x < _size.x; x++)
                {
                    var cellValue = _schema[currentMapIndex, y, x];
                    if (cellValue != 1) continue;
                    Debug.Log($"找到起始点: ({x}, {y}), 值={cellValue}");
                    return new Vector2Int(x, y);
                }
            }

            Debug.LogError("没有找到道路格子！打印当前地图数据：");
            for (var y = 0; y < _size.y; y++)
            {
                var row = "";
                for (var x = 0; x < _size.x; x++)
                {
                    row += _schema[currentMapIndex, y, x] + " ";
                }

                Debug.Log($"Row {y}: {row}");
            }

            return Vector2Int.one * -1;
        }

        private List<Vector2Int> BfsGetAllRoadCells(Vector2Int startPoint)
        {
            var roadCells = new List<Vector2Int>();
            var visited = new HashSet<Vector2Int>();
            var queue = new Queue<Vector2Int>();

            queue.Enqueue(startPoint);
            visited.Add(startPoint);

            var directions = new Vector2Int[]
            {
                new(0, -1), // 上
                new(0, 1), // 下
                new(-1, 0), // 左
                new(1, 0) // 右
            };

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                roadCells.Add(current);

                foreach (var direction in directions)
                {
                    var neighbor = current + direction;

                    if (neighbor.x < 0 || neighbor.x >= _size.x ||
                        neighbor.y < 0 || neighbor.y >= _size.y)
                        continue;

                    if (visited.Contains(neighbor))
                        continue;

                    if (_schema[currentMapIndex, neighbor.y, neighbor.x] != 1)
                        continue;

                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }

            return roadCells;
        }

        private List<Vector2Int> GenerateCircularPath(List<Vector2Int> allRoadCells, Vector2Int startPoint)
        {
            if (allRoadCells.Count == 0) return new List<Vector2Int>();

            var path = new List<Vector2Int>();
            var remaining = new HashSet<Vector2Int>(allRoadCells);
            var current = startPoint;

            path.Add(current);
            remaining.Remove(current);

            while (remaining.Count > 0)
            {
                var nextPoint = current;
                var minDistance = float.MaxValue;
                var foundAdjacent = false;

                foreach (var candidate in remaining)
                {
                    var distance = Vector2Int.Distance(current, candidate);
                    if (!(distance <= 1.5f)) continue;
                    if (!(distance < minDistance)) continue;
                    minDistance = distance;
                    nextPoint = candidate;
                    foundAdjacent = true;
                }

                if (!foundAdjacent)
                {
                    foreach (var candidate in remaining)
                    {
                        var distance = Vector2Int.Distance(current, candidate);
                        if (!(distance < minDistance)) continue;
                        minDistance = distance;
                        nextPoint = candidate;
                    }
                }

                path.Add(nextPoint);
                remaining.Remove(nextPoint);
                current = nextPoint;
            }

            return path;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            if (showBfsPath && bfsPath is { Count: > 0 })
            {
                Gizmos.color = Color.red;
                foreach (var worldPos in bfsPath.Select(gridPoint => new Vector3(
                             startPosition.x + gridPoint.x * gridSize,
                             startPosition.y - gridPoint.y * gridSize,
                             0)))
                {
                    Gizmos.DrawCube(worldPos, Vector3.one * 0.3f);
                }
            }

            if (!showOptimizedPath || optimizedPath is not { Count: > 0 }) return;
            {
                Gizmos.color = Color.green;

                foreach (var worldPos in optimizedPath.Select(gridPoint => new Vector3(
                             startPosition.x + gridPoint.x * gridSize,
                             startPosition.y - gridPoint.y * gridSize,
                             0)))
                {
                    Gizmos.DrawSphere(worldPos, 0.2f);
                }

                Gizmos.color = Color.yellow;
                for (var i = 0; i < optimizedPath.Count; i++)
                {
                    var current = optimizedPath[i];
                    var next = optimizedPath[(i + 1) % optimizedPath.Count];

                    var currentWorld = new Vector3(
                        startPosition.x + current.x * gridSize,
                        startPosition.y - current.y * gridSize,
                        0);
                    var nextWorld = new Vector3(
                        startPosition.x + next.x * gridSize,
                        startPosition.y - next.y * gridSize,
                        0);

                    Gizmos.DrawLine(currentWorld, nextWorld);
                }
            }
        }

        [EventSubscribe("GameOver")]
        public object OnGameOver(string sa)
        {
            ClearMap();
            return null;
        }

        [EventSubscribe("RePlay")]
        public object OnRePlay(string a)
        {
            ClearMap();
            return null;
        }
        
        
        public void ClearMap()
        {
            if (MapSchema == null) return;

            foreach (var go in MapSchema)
                if (go)
                    Destroy(go);
            foreach (var go in allSlots.Where(go => go))
            {
                Destroy(go.gameObject);
            }
            allSlots.Clear();
        }

        public void SwitchToMap(int mapIndex)
        {
            if (mapIndex >= 0 && mapIndex < _schema.GetLength(0))
            {
                currentMapIndex = mapIndex;
                GenerateMap();
            }
        }

        private int GetCellType(int x, int y)
        {
            if (x >= 0 && x < _size.x && y >= 0 && y < _size.y)
                return _schema[currentMapIndex, y, x];
            return -1;
        }

        public bool IsRoad(int x, int y)
        {
            return GetCellType(x, y) == 1;
        }

        public bool IsSlot(int x, int y)
        {
            return GetCellType(x, y) == 0;
        }
    }
}