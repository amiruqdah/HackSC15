using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour {

	// Possible Player Events

	public delegate void SpawnEvent(ref Vector2 currentCell);
	public static event SpawnEvent onSpawn;
	public delegate void DieEvent();
	public static event DieEvent onDeath;
	public delegate void MoveEvent();
	public static event MoveEvent onMove;
	public delegate void KeyEvent();
	public delegate void WinEvent();
	public static event WinEvent onWinEvent;

	private int[,] map;
	private int[] neighbours = new int[4];
	private Vector2 currentCell;
	private int mapSize;
	private enum Dir {N=0, W, S, E};

	// might have to change the script execution error for this
	// Use this for initialization
	void Start () {
		// Grab the Map Object from the Player
		GameObject mapObject = GameObject.Find ("Map");
		map = mapObject.GetComponent<MapGeneration>().Map; // grab the refrence from the map generation
		// interestingly, we might also want to guarantee that this always executes in a certain order. 
		onSpawn(ref currentCell);
		Debug.Log("Current Cell: " + currentCell.x + "," + currentCell.y);
		mapSize = mapObject.GetComponent<MapGeneration>().Size;

		this.gameObject.transform.DOScale( new Vector3(0.7f,1f,0.7f), 0.9f).SetEase(Ease.OutBounce);
		this.gameObject.transform.DOJump(this.transform.position, 2.5f, 1, 0.40f, false);
	}
	
	// Update is called once per frame
	void Update () {

		int currentHeight = map[(int)currentCell.x, (int)currentCell.y];

		if(Input.GetKeyDown(KeyCode.A))
		{
			recalculateNeighbours();
			// Compare difference to see if it is an accetable jump
			if(Math.Abs(currentHeight - map[(int)currentCell.x - 1, (int)currentCell.y]) <= 1)
			{
				currentCell.x -= 1;
				this.transform.position = new Vector3(currentCell.x, (float)map[(int)currentCell.x, (int)currentCell.y] + 1f, currentCell.y);
			}
		}

		if(Input.GetKeyDown(KeyCode.D))
		{
			recalculateNeighbours();
			if(Math.Abs(currentHeight - map[(int)currentCell.x + 1, (int)currentCell.y]) <= 1)
			{
				currentCell.x += 1;
				this.transform.position = new Vector3(currentCell.x, (float) map[(int)currentCell.x, (int)currentCell.y] + 1f, currentCell.y);
			}
		}
		if(Input.GetKeyDown(KeyCode.S))
		{
			recalculateNeighbours();
			if(Math.Abs(currentHeight - map[(int)currentCell.x, (int)currentCell.y - 1]) <= 1)
			{
				currentCell.y -= 1;
				this.transform.position = new Vector3(currentCell.x, (float) map[(int)currentCell.x, (int)currentCell.y] + 1f, currentCell.y);
			}
		}
		if(Input.GetKeyDown(KeyCode.W))
		{
			recalculateNeighbours();
			if(Math.Abs(currentHeight - map[(int)currentCell.x, (int)currentCell.y + 1]) <= 1)
			{
				currentCell.y += 1;
				this.transform.position = new Vector3(currentCell.x, (float) map[(int)currentCell.x, (int)currentCell.y] + 1f, currentCell.y);		
			}
		}

	}

	private void recalculateNeighbours()
	{
		if(currentCell.y < mapSize - 1)
			neighbours[(int)Dir.N] = map[(int)currentCell.x, (int)currentCell.y + 1]; // UP
		if(currentCell.x < mapSize - 1)
			neighbours[(int)Dir.E] = map[(int)currentCell.x + 1, (int)currentCell.y]; // RIGHT
		if(currentCell.y < mapSize - 1)
			neighbours[(int)Dir.S] = map[(int)currentCell.x, (int)currentCell.y - 1]; // DOWN
		if(currentCell.x < mapSize - 1)
			neighbours[(int)Dir.W] = map[(int)currentCell.x - 1, (int)currentCell.y]; // LEFT
	}
}
