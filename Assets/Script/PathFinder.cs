using UnityEngine;
using System.Collections.Generic;
using System;

public class PathFinder : MonoBehaviour {

    public void FindPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> requestCallback, Grid grid, Action<PathResult> callback) { 

        bool pathSuccess = false;

        GridNode startNode = grid.NodeFromWorldPoint(pathStart);
        GridNode endNode = grid.NodeFromWorldPoint(pathEnd);
        startNode.parent = startNode;

        List<GridNode> OpenNodes = new List<GridNode>();
        List<GridNode> ClosedNodes = new List<GridNode>();
        OpenNodes.Add(startNode);

        while (OpenNodes.Count > 0) {

            GridNode currentNode = OpenNodes[0];

            for (int i = 1; i < OpenNodes.Count; i++) {
                if (OpenNodes[i].fScore <= currentNode.fScore) {
                    if (OpenNodes[i].hScore < currentNode.hScore)
                        currentNode = OpenNodes[i];
                }
            }
            OpenNodes.Remove(currentNode);
            ClosedNodes.Add(currentNode);
             if (currentNode == endNode) {
                 pathSuccess = true;
                 break;
             }

            foreach (GridNode neighbour in grid.GetNeighbours(currentNode)) {
                if (!neighbour.isWalkable || ClosedNodes.Contains(neighbour)) {
                    continue;
                }

                int newGScoreToNeighbour = currentNode.gScore + GetDistance(currentNode, neighbour);
                if (newGScoreToNeighbour < neighbour.gScore || !OpenNodes.Contains(neighbour)) {
                    neighbour.gScore = newGScoreToNeighbour;
                    neighbour.hScore = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if (!OpenNodes.Contains(neighbour))
                        OpenNodes.Add(neighbour);

                }
                
            }
        }
        Vector3[] points = new Vector3[0];
        if (pathSuccess) {
            points = GetPath(startNode, endNode,pathEnd);
            pathSuccess = points.Length > 0;
        }
        callback(new PathResult(points, pathSuccess, requestCallback));

    }
    int GetDistance(GridNode A, GridNode B) {
        int distanceX = Mathf.Abs(A.gridX - B.gridX);
        int distanceY = Mathf.Abs(A.gridY - B.gridY);

        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
    Vector3[] GetPath(GridNode startNode, GridNode endNode,Vector3 target) {
        List<GridNode> path = new List<GridNode>();
        GridNode currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] points = SetPoints(path,target);
        Array.Reverse(points);
        return points;

    }
    Vector3[] SetPoints(List<GridNode> path, Vector3 target) {
        List<Vector3> points = new List<Vector3>();

        points.Add(target);
        for (int i = 1; i < path.Count; i++) {
                points.Add(path[i].position);
        }

        return points.ToArray();
    }
}