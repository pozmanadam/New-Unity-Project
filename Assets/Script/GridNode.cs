using UnityEngine;
using System.Collections.Generic;

public class GridNode  {

    public bool isWalkable;
    public Vector3 position;
    public int gridX;
    public int gridY;

    public int gScore;
    public int hScore;
    public GridNode parent;

    public GridNode(bool isWalkable, Vector3 position, int gridX, int gridY) {
        this.isWalkable = isWalkable;
        this.position = position;
        this.gridX = gridX;
        this.gridY = gridY;
    }
    public int CompareTo(GridNode nodeToCompare) {
        int compare = fScore.CompareTo(nodeToCompare.fScore);
        if (compare == 0) {
            compare = hScore.CompareTo(nodeToCompare.hScore);
        }
        return -compare;
    }
    public int fScore {
        get
        {
            return gScore + hScore;
        }
    }

}