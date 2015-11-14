using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {
	//TODO: Combine meshes for performance optimization

	public int sizeX, sizeY, sizeZ;
	private int[,] map;
	// Use this for initialization
	void Start () 
	{
		sizeX = 10;
		sizeY = 10;
		sizeZ = 10;

		map = new int[sizeX, sizeY];

		for(int x = sizeX; x > 0; x--)
		{
			for(int y = sizeY; y > 0; y--)
			{

				for(int z = sizeZ; z > 0; z--)
				{
					GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
					temp.transform.position = new Vector3(x - 1, y - 1, z - 1);
				}
			}
		}

	}
}