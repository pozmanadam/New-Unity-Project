using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;

public class PathRequestManager : MonoBehaviour {
    Queue<PathResult> results = new Queue<PathResult>();

    static PathRequestManager instance;
    PathFinder pathfinding;

    void Start() {
        instance = this;
        pathfinding = GetComponent<PathFinder>();
    }

    void Update() {
        if (results.Count > 0) {
            int itemsInQueue = results.Count;
            lock (results) {
                for (int i = 0; i < itemsInQueue; i++) {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback, Grid grid) {
        ThreadStart threadStart = delegate {
            instance.pathfinding.FindPath(pathStart, pathEnd,  callback,  grid, instance.ResultEnqueue);
        };
        threadStart.Invoke();
    }

    public void ResultEnqueue(PathResult result) {
        lock (results) {
            results.Enqueue(result);
        }
    }
}

public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback) {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }

}
