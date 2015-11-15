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

	private HFTInput hftInput ;
	private int[,] map;
	private int[] neighbours = new int[4];
	private Vector2 currentCell;
	private Vector2 endCell;
	private int mapSize;
	private enum Dir {N=0, W, S, E};
	private bool restHorz, restVert, celebrated;

	// might have to change the script execution error for this
	// Use this for initialization
	void Start () {
		restHorz = true;
		restVert = true;
		celebrated = false;

		// Grab the Map Object from the Player
		GameObject mapObject = GameObject.Find ("Map");
		map = mapObject.GetComponent<MapGeneration>().Map; // grab the refrence from the map generation
		endCell = new Vector2(mapObject.GetComponent<MapGeneration>().getEndX(), mapObject.GetComponent<MapGeneration>().getEndY());
		onSpawn(ref currentCell);
		Debug.Log("Current Cell: " + currentCell.x + "," + currentCell.y);
		mapSize = mapObject.GetComponent<MapGeneration>().Size;

		hftInput = this.gameObject.GetComponent<HFTInput>();
		this.gameObject.transform.DOScale( new Vector3(0.7f,1f,0.7f), 0.9f).SetEase(Ease.OutBounce);
		this.gameObject.transform.DOJump(this.transform.position, 2.5f, 1, 0.40f, false);
	}
	
	// Update is called once per frame
	void Update () {
	
		int currentHeight = map[(int)currentCell.x, (int)currentCell.y];

		if(currentCell == endCell && celebrated == false)
		{
			CelebrationAnimation();
			celebrated = true;
		}

		if(hftInput.GetAxis("Horizontal") == 0)
			restHorz = true;
		if(hftInput.GetAxis("Vertical") == 0)
			restVert = true;



		if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.UpArrow) || hftInput.GetAxis("Horizontal") < 0 && restHorz == true)
		{
			restHorz = false;
			//			recalculateNeighbours();
			// Compare difference to see if it is an accetable jump
			if(currentCell.x > 0 && Math.Abs(currentHeight - map[(int)currentCell.x - 1, (int)currentCell.y]) <= 1)
			{
				currentCell.x -= 1;
				Vector3 vect = new Vector3(currentCell.x, (float)map[(int)currentCell.x, (int)currentCell.y] + 1f, currentCell.y);
				this.transform.DOJump(vect, 1f, 1, 0.15f, false);
			}
		}
		
		if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || hftInput.GetAxis("Horizontal") > 0 && restHorz == true)
		{
			restHorz = false;
			//			recalculateNeighbours();
			if(currentCell.x < mapSize - 1 && Math.Abs(currentHeight - map[(int)currentCell.x + 1, (int)currentCell.y]) <= 1)
			{
				currentCell.x += 1;
				Vector3 vect = new Vector3(currentCell.x, (float) map[(int)currentCell.x, (int)currentCell.y] + 1f, currentCell.y);
				this.transform.DOJump(vect, 1f, 1, 0.15f, false);
			}
		}
		if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow) || hftInput.GetAxis("Vertical") > 0 && restVert == true)
		{
			restVert = false;
			//			recalculateNeighbours();
			if(currentCell.y > 0 && Math.Abs(currentHeight - map[(int)currentCell.x, (int)currentCell.y - 1]) <= 1)
			{
				currentCell.y -= 1;
				Vector3 vect = new Vector3(currentCell.x, (float) map[(int)currentCell.x, (int)currentCell.y] + 1f, currentCell.y);
				this.transform.DOJump(vect, 1f, 1, 0.15f, false);
			}
		}
		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || hftInput.GetAxis ("Vertical") < 0 && restVert == true)
		{
			restVert = false;
			//			recalculateNeighbours();
			if(currentCell.y < mapSize - 1 && Math.Abs(currentHeight - map[(int)currentCell.x, (int)currentCell.y + 1]) <= 1)
			{
				currentCell.y += 1;
				Vector3 vect = new Vector3(currentCell.x, (float) map[(int)currentCell.x, (int)currentCell.y] + 1f, currentCell.y);
				this.transform.DOJump(vect, 1f, 1, 0.15f, false);
			}
		}
		
	}


	private void CelebrationAnimation()
	{

		this.transform.DOMoveY(this.transform.position.y + 14, 2f,false).SetDelay(0.5f);
		this.transform.eulerAngles += Vector3.right * 125 * Time.deltaTime;
		onWinEvent(); // also those who are interested in this event get notified
		Destroy(this.gameObject,2.5f);
	}
	//	private void recalculateNeighbours()
	//	{
	//		if(currentCell.y < mapSize - 1)
	//			neighbours[(int)Dir.N] = map[(int)currentCell.x, (int)currentCell.y + 1]; // UP
	//		if(currentCell.x < mapSize - 1)
	//			neighbours[(int)Dir.E] = map[(int)currentCell.x + 1, (int)currentCell.y]; // RIGHT
	//		if(currentCell.y < mapSize - 1)
	//			neighbours[(int)Dir.S] = map[(int)currentCell.x, (int)currentCell.y - 1]; // DOWN
	//		if(currentCell.x < mapSize - 1)
	//			neighbours[(int)Dir.W] = map[(int)currentCell.x - 1, (int)currentCell.y]; // LEFT
	//	}
}
