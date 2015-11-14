using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {
	//TODO: Combine meshes for performance optimization

	public int size, height, maxDrop;
	private int[,] map;
	private int step;
	// Use this for initialization
	void Start () 
	{
		size = 10;
		height = 15;
		maxDrop = 3;

		map = new int[size, size];
		for (int i = 0; i < size; i++) 
		{
			for(int z = 0; z < size; z++)
			{
				map[i,z] = -1;
			}
		}
		map [size - 1, size - 1] = height - 1;

		for (step = size - 1; step > 0; step--) 
		{
			for(int x = size - 1; x >= step; x--)
			{
				double rand = Random.value;
				map[x, step - 1] = map[x, step] - (int)(rand * maxDrop);
				if(x == step)
				{
					map[x - 1, step - 1] = map[x, step] - (int)(rand * maxDrop);
				}
			}
			for(int y = size - 1; y >= step; y--)
			{
				double rand = Random.value;
				map[step - 1, y] = map[step, y] - (int)(rand * maxDrop);
				if(y == step)
				{
					if(Random.value > 0.5)
						map[step - 1, y - 1] = map[step, y] - (int)(rand * maxDrop);
				}
			}
		}

		for (int i = 0; i < size; i++) 
		{
			for(int z = 0; z < size; z++)
			{
				if(map[i,z] < 0)
					map[i,z] = 0;
			}
		}

		for(int x = size - 1; x >= 0; x--)
		{
			for(int y = size - 1; y >= 0; y--)
			{
				int t = map[x, y];
				for( ;t >= 0; t--)
				{
					GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
					temp.transform.position = new Vector3(x, t, y);
				}
			}
		}

	}
}