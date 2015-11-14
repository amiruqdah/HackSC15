using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {
	
	// Event System for the Map
	public delegate void CreateEvent(Vector3 position);
	public static event CreateEvent onCreate;
	public delegate void DestroyEvent();
	public static event DestroyEvent doDestroy;
	
	public int size, height;
	public double maxDrop;
	private int[,] map;
	private int step;
	enum Dir {N, W, S, E};

	void Start()
	{
		init ();
	}

	// Use this for initialization
	void init () 
	{
		size = 20;
		height = 20;
		maxDrop = 2.1;
		
		map = new int[size, size];
		//		
		map [size - 1, size - 1] = height - 1;
		map [0, 0] = -10;
		
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
			map[step, step] = Mathf.Min(Mathf.Min(map[step + 1, step], map[step, step + 1]), map[step + 1, step + 1]) - (int)(r);
			
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
					map[i,z] = -10;
			}
		}
		
		//Path Creation
		int pathX = size - 1;
		int pathY = size - 1;
		int count = 0;
		int dir = (int)Dir.S; 	//For start, can't go this dir anyways
		bool done = false;
		while (!done && count < size * 2) 
		{
			int currH = map[pathX, pathY];
			if(pathX - 1 >= 0 && pathY - 1 >= 0 && dir != (int)Dir.N && dir != (int)Dir.W && 
			   Mathf.Abs(currH - map[pathX - 1, pathY]) <= 1 && Mathf.Abs(currH - map[pathX, pathY - 1]) <= 1)
			{
				if(pathX > pathY)
				{
					pathX--;
					dir = (int)Dir.E;
				}
				else
				{
					pathY--;
					dir = (int)Dir.S;
				}
				count++;
			}
			else if(pathX - 1 > 0 && dir != (int)Dir.W && Mathf.Abs(currH - map[pathX - 1, pathY]) <= 1)
			{
				pathX--;
				dir = (int)Dir.E;
				count++;
			}
			else if(pathY - 1 >= 0 && dir != (int)Dir.N && Mathf.Abs(currH - map[pathX, pathY - 1]) <= 1)
			{
				pathY--;
				dir = (int)Dir.S;
				count++;
			}
			else if(pathX - 1 >= 0 && pathY + 1 < size && 
			        currH - map[pathX - 1, pathY] == 2 && currH - 1 <= map[pathX - 1, pathY + 1] &&
			        pathY - 1 >= 0 && pathX + 1 < size &&
			        currH - map[pathX, pathY - 1] == 2 && currH - 1 <= map[pathX + 1, pathY - 1])
			{
				if(pathX > pathY)
				{
					map[pathX - 1, pathY] = currH - 1;
					dir = (int)Dir.E;
				}
				else
				{
					map[pathX, pathY - 1] = currH - 1;
					dir = (int)Dir.S;
				}
				count++;
				Debug.Log("Added block");
			}
			else if(pathX - 1 >= 0 && pathY + 1 < size &&
			        currH - map[pathX - 1, pathY] == 2 && currH - 1 <= map[pathX - 1, pathY + 1])
			{
				map[pathX - 1, pathY] = currH - 1;
				dir = (int)Dir.E;
				count++;
				Debug.Log("Added block");
			}
			else if(pathY - 1 >= 0 && pathX + 1 < size &&
			        currH - map[pathX, pathY - 1] == 2 && currH - 1 <= map[pathX + 1, pathY - 1])
			{
				map[pathX, pathY - 1] = currH - 1;
				dir = (int)Dir.S;
				count++;
				Debug.Log("Added block");
			}
			//			else if (pathX + 1 < size && dir != (int)Dir.E && Mathf.Abs(currH - map[pathX + 1, pathY]) <= 1)
			//			{
			//				pathX++;
			//				dir = (int)Dir.W;
			//				count++;
			//			}
			//			else if (pathY + 1 < size && dir != (int)Dir.S && Mathf.Abs(currH - map[pathX, pathY + 1]) <= 1)
			//			{
			//				pathY++;
			//				dir = (int)Dir.N;
			//				count++;
			//			}
			else
			{
				done = true;
			}
		}
		
		if (count < size) 
		{
			init ();
			return;
		}
		
		Debug.Log(pathX);
		Debug.Log(pathY);
		Debug.Log(count);
		
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
						if(x == pathX && y == pathY && t == map[x,y] || 
						   x == size - 1 && y == size - 1 && t == height - 1)
						{
							Color r;
							if(x == size - 1 && y == size - 1 && t == height - 1)
								r = new Color(255,0,0,1);
							else
								r = new Color(0,0,255,1);
							
							MeshRenderer rend = temp.GetComponent<MeshRenderer>();
							Material mat = new Material(Shader.Find("Standard"));
							mat.color = r;
							rend.material = mat ;
						}
					}
				}
			}
		}

		// Badly attempt to combine all meshes 

		/*MeshFilter[] meshFilters = this.transform.gameObject.GetComponentsInChildren<MeshFilter>();
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

	*/
		int xSpawnPos;
		int ySpawnPos;

		do{
			xSpawnPos = Random.Range(0, size - 1);
			ySpawnPos = Random.Range(0, size - 1);
		}while(map[xSpawnPos, ySpawnPos] == -1);

		onCreate(new Vector3(xSpawnPos, map[xSpawnPos, ySpawnPos] + 1, ySpawnPos)); 
	}

	private void OnDestroy(){
	
		doDestroy();
		// Do whatever I need to do to the map in order to destroy it and then clean up whatever

	}

	// Get Set Accsesor thingies
	public int[,] Map
	{
		get{
			return map;
		}
	}
		                                                            
}
