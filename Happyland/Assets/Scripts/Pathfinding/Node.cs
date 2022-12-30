using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>
{

	public bool walkable;
	public bool isWater;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;
	public Node parent;
	public int movementPenalty;
	int heapIndex;

	public Node(bool _walkable, bool _isWater, Vector3 _worldPos, int _gridX, int _gridY)
	{
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
		isWater = _isWater;

        if (isWater)
        {
			movementPenalty = 2;
        }
        else
        {
			movementPenalty = 0;
        }
	}

	public int fCost
	{
		get
		{
			return gCost + hCost;
		}
	}

	public int HeapIndex
    {
        get
        {
			return heapIndex;
        }
        set
        {
			heapIndex = value;
        }
    }

	public int CompareTo(Node nodeToCompare)
    {
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if(compare == 0)
        {
			compare = hCost.CompareTo(nodeToCompare.hCost);
        }

		return -compare;
    }
}