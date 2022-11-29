using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
	public enum PathfindingAlgorithm
	{
		AStar,
		GreedyBestFirstSearch,
		BFS,
		DFS
	}

	[Header("Pathfinding Settings")]
	public PathfindingAlgorithm pathfindingType = PathfindingAlgorithm.AStar;
	public Transform seeker, target;
	public Grid grid;
	public List<Node> path;

	void Awake()
	{
		grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
		path = new List<Node>();
	}

	public void UpdatePathfinding( Vector3 targetPosition)
	{
        switch (pathfindingType)
        {
            case PathfindingAlgorithm.AStar:
                FindPathAStar(seeker.position, targetPosition);
                break;
            case PathfindingAlgorithm.GreedyBestFirstSearch:
                FindPathGreedyBestFirstSearch(seeker.position, targetPosition);
                break;
            case PathfindingAlgorithm.BFS:
                FindPathBFS(seeker.position, targetPosition);
                break;
            case PathfindingAlgorithm.DFS:
                FindPathDFS(seeker.position, targetPosition);
                break;
        }
    }

	void FindPathAStar(Vector3 startPos, Vector3 targetPos)
	{
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			Node node = openSet.RemoveFirst();
			
			closedSet.Add(node);

			if (node == targetNode)
			{
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(node))
			{
				if (!neighbour.walkable || closedSet.Contains(neighbour))
				{
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}

	void FindPathGreedyBestFirstSearch(Vector3 startPos, Vector3 targetPos)
	{
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
				{
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode)
			{
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(node))
			{
				if (!neighbour.walkable || closedSet.Contains(neighbour))
				{
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (!openSet.Contains(neighbour))
				{
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}

	void FindPathDFS(Vector3 startPos, Vector3 targetPos)
	{
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		Stack<Node> frontier = new Stack<Node>();
		List<Node> explored = new List<Node>();

		frontier.Push(startNode);

		while (frontier.Count != 0)
		{
			Node state = frontier.Pop();
			explored.Add(state);

			if (state == targetNode)
			{
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(state))
			{
				if (!neighbour.walkable || explored.Contains(neighbour))
				{
					continue;
				}

                if (!frontier.Contains(neighbour))
                {
					frontier.Push(neighbour);
					neighbour.parent = state;
                }
			}
		}
	}

	void FindPathBFS(Vector3 startPos, Vector3 targetPos)
    {
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		Queue<Node> frontier = new Queue<Node>();
		List<Node> explored = new List<Node>();

		frontier.Enqueue(startNode);

		while (frontier.Count != 0)
		{
			Node state = frontier.Dequeue();
			explored.Add(state);

			if (state == targetNode)
			{
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(state))
			{
				if (!neighbour.walkable || explored.Contains(neighbour))
				{
					continue;
				}

				if (!frontier.Contains(neighbour))
				{
					frontier.Enqueue(neighbour);
					neighbour.parent = state;
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode)
	{
		path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();
		grid.path = path;
	}

	int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}