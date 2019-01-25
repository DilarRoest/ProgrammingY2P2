using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public Transform wanderer, target;
    public Transform gridObject;

    Grid grid;

    private void Awake()
    {
        grid = gridObject.GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(wanderer.position, target.position);
    }


    //The void that'll calculate the best path.
    void FindPath(Vector3 start, Vector3 destination)
    {

        //The nodes are send to the WorldPointToNode and their information is found.
        Node startNode = grid.WorldPointToNode(start);
        Node destinationNode = grid.WorldPointToNode(destination);

        //The open and closed list.
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        openList.Add(startNode);
        

        while(openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == destinationNode)
            {
                DrawPath(startNode, destinationNode);
                return;
            }

            foreach (Node neighbor in grid.GetNeighbours(currentNode))
            {
                if (neighbor.isWall || closedList.Contains(neighbor))
                    continue; ;


                int newMoveCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMoveCostToNeighbor < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newMoveCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, destinationNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }
    }

    //Draws the final path
    void DrawPath(Node startNode, Node endNode)
    {

        List<Node> finalPath = new List<Node>();

        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            finalPath.Add(currentNode.parent);
            currentNode = currentNode.parent;
        }

        finalPath.Reverse();

        grid.path = finalPath;
    }

    //Returns the distance of the two given nodes
    int GetDistance (Node a, Node b)    
    {
        int distX = Mathf.Abs(a.positionX - b.positionX);
        int distY = Mathf.Abs(a.positionY - b.positionY);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }
}