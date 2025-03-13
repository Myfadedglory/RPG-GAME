using System.Collections.Generic;
using UnityEngine;

namespace Script.Utilities
{
    public static class AStar
    {
        // 节点类，表示网格中的一个点
        private class Node
        {
            public readonly Vector2Int Position; // 网格坐标
            public float G; // 从起点到当前节点的代价
            public float H; // 启发式估计值（到终点的估计代价）
            public float F => G + H; // 总代价
            public Node Parent; // 父节点，用于回溯路径

            public Node(Vector2Int position, float g, float h, Node parent)
            {
                Position = position;
                G = g;
                H = h;
                Parent = parent;
            }
        }

        // A* 寻路算法
        public static List<Vector2> FindPath(Vector2 start, Vector2 target, LayerMask obstacleMask, float gridSize)
        {
            var startNode = WorldToGrid(start, gridSize);
            var targetNode = WorldToGrid(target, gridSize);

            var openSet = new List<Node>();
            var closedSet = new HashSet<Vector2Int>();

            openSet.Add(new Node(startNode, 0, Heuristic(startNode, targetNode), null));

            while (openSet.Count > 0)
            {
                // 找到 F 值最小的节点
                var currentNode = openSet[0];
                for (var i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].F < currentNode.F)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode.Position);

                // 如果到达目标节点，返回路径
                if (currentNode.Position == targetNode)
                {
                    return ReconstructPath(currentNode, gridSize);
                }

                // 遍历邻居节点
                foreach (var neighbor in GetNeighbors(currentNode.Position))
                {
                    if (closedSet.Contains(neighbor) || IsObstacle(neighbor, obstacleMask, gridSize))
                    {
                        continue;
                    }

                    var tentativeG = currentNode.G + Heuristic(currentNode.Position, neighbor);

                    var neighborNode = openSet.Find(n => n.Position == neighbor);
                    if (neighborNode == null || tentativeG < neighborNode.G)
                    {
                        if (neighborNode == null)
                        {
                            neighborNode = new Node(neighbor, tentativeG, Heuristic(neighbor, targetNode), currentNode);
                            openSet.Add(neighborNode);
                        }
                        else
                        {
                            neighborNode.G = tentativeG;
                            neighborNode.Parent = currentNode;
                        }
                    }
                }
            }

            return null; // 如果没有找到路径
        }

        // 获取邻居节点
        private static List<Vector2Int> GetNeighbors(Vector2Int position)
        {
            return new List<Vector2Int>
            {
                position + new Vector2Int(1, 0),
                position + new Vector2Int(-1, 0),
                position + new Vector2Int(0, 1),
                position + new Vector2Int(0, -1)
            };
        }

        // 判断是否为障碍物
        private static bool IsObstacle(Vector2Int position, LayerMask obstacleMask, float gridSize)
        {
            Vector2 worldPosition = GridToWorld(position, gridSize);
            return Physics2D.OverlapCircle(worldPosition, gridSize / 2, obstacleMask);
        }

        // 启发式函数（曼哈顿距离）
        private static float Heuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        // 从终点回溯路径
        private static List<Vector2> ReconstructPath(Node endNode, float gridSize)
        {
            var path = new List<Vector2>();
            Node currentNode = endNode;

            while (currentNode != null)
            {
                path.Add(GridToWorld(currentNode.Position, gridSize));
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return path;
        }

        // 世界坐标转网格坐标
        private static Vector2Int WorldToGrid(Vector2 worldPosition, float gridSize)
        {
            return new Vector2Int(
                Mathf.RoundToInt(worldPosition.x / gridSize),
                Mathf.RoundToInt(worldPosition.y / gridSize)
            );
        }

        // 网格坐标转世界坐标
        private static Vector2 GridToWorld(Vector2Int gridPosition, float gridSize)
        {
            return new Vector2(gridPosition.x * gridSize, gridPosition.y * gridSize);
        }
    }
}