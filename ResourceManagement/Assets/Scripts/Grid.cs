using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public Transform player;
    public LayerMask obstacles;
    public LayerMask ground;
    public Vector2 gridWorldSize;
    Node[,] grid;

    private int gridSizeX, gridSizeY;
    private float nodeDiameter;
    public float nodeRadius;

    // Use this for initialization
    void Start ()
    {
        nodeDiameter = nodeRadius * 2;

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        //Creates the node array.
        grid = new Node[gridSizeX, gridSizeY];

        //Finds the most lower-left corner of the grid.
        Vector3 bottemLeft = transform.position - Vector3.right * gridWorldSize.y / 2 - Vector3.forward * gridWorldSize.x / 2;
        
        //ForLoop to create the grid.
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //Retrieves the upper-right corner of the grid (using the lower-left corner).
                Vector3 worldPoint = bottemLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool isWall = true;
                bool inaccesable = false;

                if (Physics.CheckSphere(worldPoint, nodeRadius, ground))
                    isWall = false;
                if (Physics.CheckSphere(worldPoint, nodeRadius, obstacles))
                    isWall = true;
                if (Physics.CheckSphere(worldPoint, nodeRadius, obstacles) && Physics.CheckSphere(worldPoint, nodeRadius, ground))
                    inaccesable = true;
                
                //Creates a new node every loop on the X and Y coordinates, essentialy making the grid.
                grid[x, y] = new Node(isWall, worldPoint, x, y, inaccesable);
            }
        }
    }

    //The void used to identify the surounding of the given Node;
    public List<Node> GetNeighbours(Node node)
    {

        // Creates a list for the neighbors.
        List<Node> neighbours = new List<Node>(); 

        //Loops through the nodes surrounding the given node (That's why it skips x and y both being 0, because that's the given node)
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {

                if (x == 0 && y == 0)
                    continue;

                //Checks the nodes position with the value of x (the node is in position 16 on the x-axis, 16 + x (x being -1) = 15, so it effectively gets the node left of the selected one)
                int checkX = node.positionX + x;
                int checkY = node.positionY + y;

                //Checks if the nodes are within the gridSize and afterwards adds them to the neighbours list.
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }


        return neighbours;
    }

    //Used to convert world coordinates to nodes.
    public Node WorldPointToNode(Vector3 worldPos)
    {
        //Percentages used to show where the entity is on the map (on a grid of 30,    (13 + 30 / 2) / 30 = 30 / 2 = 15 + 13 = 28, 28 / 30 is about 0,93 on X)
        float percentageX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentageY = (worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y;

        //Clamping the percentages to ints between 0 and 1.
        percentageX = Mathf.Clamp01(percentageX);
        percentageY = Mathf.Clamp01(percentageY);

        // (30 - 1 = 29 * 0,93 = 26,97  so it'll be rounded to 27, now repeat the process for the y coordinate and you have the entity's location on the grid). 
        int x = Mathf.RoundToInt((gridWorldSize.x - 1) * percentageX);
        int y = Mathf.RoundToInt((gridWorldSize.y - 1) * percentageY);

        //Returns the X and Y node locations of the entitiy's position.
        return grid[x, y];
    }


    public List<Node> path;
    //Drawing the gizmos to help visualize theh grid.
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, .2f, gridWorldSize.y));

        if (grid != null)
        {
            //Converts the player position to a node.
            Node playerNode = WorldPointToNode(player.transform.position);

            foreach (Node node in grid)
            {
                //Obstacles are red and walkables are black.
                if (node.isWall == true)
                    Gizmos.color = Color.black;
                else
                    Gizmos.color = Color.green;

                //If the node is the current playerNode
                if (node == playerNode)
                    Gizmos.color = Color.cyan;

                if (node.outOfMap == true)
                    Gizmos.color = Color.red;

                //If the final path has been found.
                if (path != null)
                {
                    if (path.Contains(node))
                    {
                        Gizmos.color = Color.magenta;
                    }
                }

                Gizmos.DrawWireCube(node.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
