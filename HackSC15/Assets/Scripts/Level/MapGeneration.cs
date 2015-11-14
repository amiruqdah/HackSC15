using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {
	//TODO: Combine meshes for performance optimization

	public int size, height;
	public double maxDrop;
	private int[,] map;
	private int step;
	// Use this for initialization
	void Start () 
	{
		size = 30;
		height = 30;
		maxDrop = 3.5;

		map = new int[size, size];

		map [size - 1, size - 1] = height - 1;
		map[0,0] = -1;

		step = size - 1;
		for(int x = step - 1; x >= 0; x--)
		{
			double rand = Random.value;
			map[x, step] = map[x + 1, step] - (int)(rand * maxDrop);
		}
		for(int y = step - 1; y >= 0; y--)
		{
			double rand = Random.value;
			map[step, y] = map[step, y + 1] - (int)(rand * maxDrop);
		}

		for (step = size - 2; step > 0; step--) 
		{
			double r = Random.value;
			map[step, step] = Mathf.Min(Mathf.Min(map[step + 1, step], map[step, step + 1]), map[step + 1, step + 1]) - (int)(r * maxDrop);

			for(int x = step - 1; x >= 0; x--)
			{
				double rand = Random.value;
				map[x, step] = Mathf.Min(map[x + 1, step], map[x, step + 1]) - (int)(rand * maxDrop);
			}
			for(int y = step - 1; y >= 0; y--)
			{
				double rand = Random.value;
				map[step, y] = Mathf.Min(map[step, y + 1], map[step + 1, y]) - (int)(rand * maxDrop);
			}
		}


		for (int i = 0; i < size; i++) 
		{
			for(int z = 0; z < size; z++)
			{
				if(map[i,z] < 0)
					map[i,z] = -1;
			}
		}

		for(int x = size - 1; x >= 0; x--)
		{
			for(int y = size - 1; y >= 0; y--)
			{
				int t = map[x, y];
				if(t >= 0)
				{
					for( ;t >= 0; t--)
					{
						GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
						temp.transform.position = new Vector3(x, t, y);
						temp.transform.SetParent(this.gameObject.transform);
					}
				}
			}
		}


		// Combine all Meshes

		MeshFilter[] meshFilters = this.transform.gameObject.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		int a = 0;

		while (a < meshFilters.Length) {
			combine[a].mesh = meshFilters[a].sharedMesh;
			combine[a].transform = meshFilters[a].transform.localToWorldMatrix;
			if(a!=0)
				Destroy(meshFilters[a].gameObject);
			a++;
		}

		transform.GetComponent<MeshFilter>().mesh = new Mesh();
		transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		transform.gameObject.active = true;
	}
		                                                            
}
