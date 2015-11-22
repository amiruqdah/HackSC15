using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class MapGeneration : MonoBehaviour {
	
	// Event System for the Map
	public delegate void DestroyEvent();
	public static event DestroyEvent doDestroy;
	public delegate void CreateEvent(Vector3 position);
	public static event CreateEvent onCreateBlock;
	public static event CreateEvent onCreatePathBlock;
	public Material mat;
	
	public AudioSource source;
	public AudioClip suspense;
	public AudioClip[] sounds;
	
	public int size, height;
	public double maxDrop;
	private int[,] map;
	private List<Index> path = new List<Index>();
	private int step;
	private int endX, endY;
	enum Dir {N, W, S, E};
	private struct Index
	{
		int x,y;
		public Index(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public int xPos{
			get{return x;}
		}
		public int yPos
		{
			get{return y;}
		}

		public bool isIdentical(int x, int y)
		{
			if(this.x == x && this.y == y)
				return true;
			else
				return false;
		}

	};

	private void Start()
	{
		Debug.Log("Called");
		source = this.gameObject.GetComponent<AudioSource> ();
		init ();
	}
	
	// Use this for initialization
	private void init ()
	{
		size = 15;
		height = 15;
		maxDrop = 2.01;
		
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
			double r = UnityEngine.Random.value;
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
		int dir = (int)Dir.S;   //For start, can't go this dir anyways
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
				path.Add(new Index(pathX,pathY));
				count++;
			}
			else if(pathX - 1 > 0 && dir != (int)Dir.W && Mathf.Abs(currH - map[pathX - 1, pathY]) <= 1)
			{
				pathX--;
				dir = (int)Dir.E;
				count++;
				path.Add(new Index(pathX,pathY));
			}
			else if(pathY - 1 >= 0 && dir != (int)Dir.N && Mathf.Abs(currH - map[pathX, pathY - 1]) <= 1)
			{
				pathY--;
				dir = (int)Dir.S;
				count++;
				path.Add(new Index(pathX,pathY));
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
					path.Add(new Index(pathX-1,pathY));
				}
				else
				{
					map[pathX, pathY - 1] = currH - 1;
					dir = (int)Dir.S;
					path.Add(new Index(pathX,pathY-1));
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
				path.Add(new Index(pathX-1, pathY));
				Debug.Log("Added block");
			}
			else if(pathY - 1 >= 0 && pathX + 1 < size &&
			        currH - map[pathX, pathY - 1] == 2 && currH - 1 <= map[pathX + 1, pathY - 1])
			{
				map[pathX, pathY - 1] = currH - 1;
				dir = (int)Dir.S;
				count++;
				path.Add(new Index(pathX,pathY-1));
				Debug.Log("Added block");
			}
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
	
		
		for(int x = size - 1; x >= 0; x--)
		{
			for(int y = size - 1; y >= 0; y--)
			{
				int t = map[x, y];
				path.Where(node => node.isIdentical(x, y)).Select(pathNode => {
					onCreatePathBlock(new Vector3( x, t, y ));
					return 1;
				});
				if(t >= 0)
				{
					int end;
					if(x - 1 < 0 || y - 1 < 0 || map[x - 1, y] <= 0 || map[x, y - 1] <= 0)
						end = 0;
					else
						end = Mathf.Min(map[x - 1, y], map[x, y - 1]);
					for( ;t >= end; t--)
					{
						GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
						onCreateBlock(new Vector3(x,t,y));
						temp.transform.position = new Vector3(UnityEngine.Random.Range(500,-500),Random.Range(500,-500), Random.Range(500,-500));
						temp.transform.DOMove( new Vector3(x, t, y), 2.2f,false).SetEase(Ease.OutSine).SetDelay((float)Random.Range (0.5f,1f));
						temp.transform.SetParent(this.transform);
						if(x == pathX && y == pathY && t == map[x,y])
						{
							Color r = new Color(255,0,0,1);
							MeshRenderer rend = temp.GetComponent<MeshRenderer>();
							Material mat = new Material(Shader.Find("Standard"));
							mat.color = r;
							rend.material = mat;
						}
					}
				}
			}
		}
		source.PlayOneShot(suspense);
		
		endX = pathX;
		endY = pathY;


		PlayerMovement.onWinEvent += Splice;

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
		//		int xSpawnPos;
		//		int ySpawnPos;
		//		
		//		do{
		//			xSpawnPos = Random.Range(0, size - 1);
		//			ySpawnPos = Random.Range(0, size - 1);
		//		}while(map[xSpawnPos, ySpawnPos] <= 0);
		//		Debug.Log(map[xSpawnPos, ySpawnPos]);
		//		onCreate(new Vector3(xSpawnPos, map[xSpawnPos, ySpawnPos] + 1, ySpawnPos));
		//		PlayerMovement.onSpawn += delegate(ref Vector2 currentCell) {
		//			currentCell.x = xSpawnPos;
		//			currentCell.y = ySpawnPos;
		//		};
	}


	private void Splice()
	{
		foreach(Transform go in GetComponentsInChildren<Transform>()){
			Sequence destroySequence = DOTween.Sequence();
			destroySequence.Append(go.DOMoveY(go.transform.position.y - 45,0.9f, false).SetEase(Ease.InQuart).SetDelay((float)UnityEngine.Random.Range(0.6f,1)));
			//if(go.gameObject.tag != "Map")
				//Destroy(go.gameObject,2f);
		}
		int i = (int)((UnityEngine.Random.value - 0.001) * sounds.Length);
		source.PlayOneShot(sounds[i]);

		doDestroy ();
		StartCoroutine(Inst());
	}

	IEnumerator Inst()
	{
		yield return new WaitForSeconds(3.5f);
		foreach (Transform go in GetComponentsInChildren<Transform>()) {
			if(go.gameObject.tag != "Map")
				Destroy(go.gameObject);
		}
		init ();
	}
	
	// Get Set Accsesor thingies
	public int[,] Map
	{
		get{
			return map;
		}
	}
	public int Size
	{
		get{
			return size;
		}
	}
	
	public int getEndX(){return endX;}
	
	public int getEndY(){return endY;}
	
	public int getHeight(int x, int y)
	{
		return map[x, y];
	}

}