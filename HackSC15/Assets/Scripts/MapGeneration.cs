using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {
	public int sizeX, sizeY, sizeZ;
	// Use this for initialization
	void Start () 
	{
		sizeX = 10;
		sizeY = 10;
		sizeZ = 10;

		for(int x = 0; x < sizeX; x++)
		{
			for(int y = 0; y < sizeY; y++)
			{
				for(int z = 0; z < sizeZ; z++)
				{
					GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);'
					temp.transform.position = new Vector3(x + 1, y + 1, z + 1);
				}
			}
		}

	}
}