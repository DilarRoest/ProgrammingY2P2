using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public Node parent;

    public bool isWall;
    public bool outOfMap;
    public Vector3 worldPosition;
    public int positionX, positionY;

    public int hCost;
    public int gCost;

    public int FCost { get { return gCost + hCost; } }

    //Constructor used in making the grid.
    public Node(bool _isWall, Vector3 _worldPosition, int _idX, int _idY, bool accesable)
    {
        isWall = _isWall;
        worldPosition = _worldPosition;
        positionX = _idX;
        positionY = _idY;
        outOfMap = accesable;
    }
}
