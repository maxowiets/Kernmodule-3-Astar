using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Astar
{
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path
    /// Note that you will probably need to add some helper functions
    /// from the startPos to the endPos
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    /// 

    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        List<Node> openNodes = new List<Node>();
        List<Node> closedNodes = new List<Node>();

        //Dictionary<Vector2Int, Node> openNodesDic = new Dictionary<Vector2Int, Node>();
        //Dictionary<Vector2Int, Node> closedNodesDic = new Dictionary<Vector2Int, Node>();

        openNodes.Add(new Node(startPos, null, 0, CalculateHScore(startPos, endPos)));
        //openNodesDic.Add(new Vector2Int(startPos.x, startPos.y), new Node(startPos, null, 0, CalculateHScore(startPos, endPos)));

        while (openNodes.Count > 0)
        {
            //Debug.Log(openNodes.Count);
            Node currentNode = GetLowestFCostNode(openNodes);
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            if (currentNode.position == endPos)
            {
                return ReturnPath(currentNode);
            }

            //Get all neighbour nodes
            //Add node if neighbourNode is not added yet
            foreach (Cell neighbourCell in grid[currentNode.position.x, currentNode.position.y].GetNeighbours(grid))
            {
                if (currentNode.position - neighbourCell.gridPosition == Vector2Int.left)
                {
                    if (neighbourCell.HasWall(Wall.LEFT))
                    {
                        continue;
                    }
                }
                else if (currentNode.position - neighbourCell.gridPosition == Vector2Int.right)
                {
                    if (neighbourCell.HasWall(Wall.RIGHT))
                    {
                        continue;
                    }
                }
                else if (currentNode.position - neighbourCell.gridPosition == Vector2Int.up)
                {
                    if (neighbourCell.HasWall(Wall.UP))
                    {
                        continue;
                    }
                }
                else if (currentNode.position - neighbourCell.gridPosition == Vector2Int.down)
                {
                    if (neighbourCell.HasWall(Wall.DOWN))
                    {
                        continue;
                    }
                }
                Node currentNeighbourNode = null;
                bool neighbourIsClosed = false;
                //check if neighbour exists in openNodes
                foreach (Node node in openNodes)
                {
                    if (node.position == neighbourCell.gridPosition)
                    {
                        currentNode = node;
                        break;
                    }
                }
                //check if neighbour exists in openNodes
                if (currentNeighbourNode == null)
                {
                    foreach (Node node in closedNodes)
                    {
                        if (node.position == neighbourCell.gridPosition)
                        {
                            currentNeighbourNode = node;
                            neighbourIsClosed = true;
                            break;
                        }
                    }
                    if (neighbourIsClosed) continue;
                }

                //Add New Node
                if (currentNeighbourNode == null)
                {
                    currentNeighbourNode = new Node(neighbourCell.gridPosition, null, (int)(currentNode.GScore + 1), CalculateHScore(neighbourCell.gridPosition, endPos));
                }

                //if new path to neighbour is shorter or not in open
                if (currentNeighbourNode.HScore < currentNode.HScore || !openNodes.Contains(currentNeighbourNode))
                {
                    Debug.Log("test");
                    currentNeighbourNode.parent = currentNode;
                    //currentNeighbourNode.parent = currentNode;
                    if (!openNodes.Contains(currentNeighbourNode))
                    {
                        openNodes.Add(currentNeighbourNode);
                    }
                }
            }

        }

        return null;
    }

    List<Vector2Int> ReturnPath(Node node)
    {
        List<Vector2Int> returnPathList = new List<Vector2Int>();
        Node currentNode = node;
        returnPathList.Add(node.position);
        while (currentNode.parent != null)
        {
            Debug.Log(currentNode.parent);
            currentNode = currentNode.parent;
            returnPathList.Add(currentNode.position);
        }
        returnPathList.Reverse();
        return returnPathList;
    }

    int CalculateHScore(Vector2Int startPos, Vector2Int endPos)
    {
        return  Mathf.Abs(startPos.x - endPos.x) + Mathf.Abs(startPos.y - endPos.y);
    }

    Node GetLowestFCostNode(List<Node> nodes)
    {
        Node lowestNode = nodes[0];
        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].FScore < lowestNode.FScore)
            {
                lowestNode = nodes[i];
            }
        }
        return lowestNode;
    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public float FScore { //GScore + HScore
            get { return GScore + HScore; }
        }
        public float GScore; //Current Travelled Distance
        public float HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, int GScore, int HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}
