using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	// Possible Player Events

	public delegate void SpawnEvent();
	public static event SpawnEvent onSpawn;
	public delegate void DieEvent();
	public static event DieEvent onDeath;
	public delegate void MoveEvent();
	public static event MoveEvent onMove;
	public delegate void KeyEvent();
	public delegate void WinEvent();
	public static event WinEvent onWinEvent;


	// Use this for initialization
	void Start () {
	
		// Spawn Shit Here




	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
