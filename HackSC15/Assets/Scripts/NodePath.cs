using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class NodePath : MonoBehaviour {

	private static List<Vector3> nodePositions = new List<Vector3>();
	private static List<Vector3> nodePathPositions = new List<Vector3>();
	
	[Range(0.0f, 1.0f)]
	public float nodeSize;

	private void Start()
	{
		nodeSize = 0.3f;
	}

	private void OnDrawGizmos()
	{	
		Gizmos.color = Color.green;
		foreach(Vector3 pos in nodePositions)
		{
			Gizmos.DrawSphere(pos, nodeSize);
		}

		Debug.Log (nodePathPositions.Count);
	}

	public static void generateNode(Vector3 blockPos)
	{
		blockPos.y = blockPos.y + 1;
		nodePositions.Add(blockPos);
	}

	public static void generatePathNode(Vector3 blockPos)
	{
		blockPos.y = blockPos.y + 1;
		nodePathPositions.Add(blockPos);
	}
	
}

