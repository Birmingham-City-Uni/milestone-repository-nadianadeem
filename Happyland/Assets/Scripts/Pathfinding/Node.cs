using UnityEngine;
using System.Collections;

public class Node
{

	public bool walkable;
	public bool isWater;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;
	public Node parent;

	public Node(bool _walkable, bool _isWater, Vector3 _worldPos, int _gridX, int _gridY)
	{
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
		isWater = _isWater;

        if (_isWater)
        {
			gCost = 2;
        }
	}

	public int fCost
	{
		get
		{
			return gCost + hCost;
		}
	}
}