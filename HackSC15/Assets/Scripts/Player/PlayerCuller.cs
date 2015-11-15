using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(MapGeneration))]
public class PlayerCuller : MonoBehaviour {
	/// <summary>
	/// Spawns a player randomly on any optimal position on the map.	/// </summary>
	public Stack<Transform> players = new Stack<Transform>();

	void Start()
	{
		// Subscribe to the Necessary Events 
		HFTGamepad.onCreate += delegate(GameObject obj) {
			players.Push(obj.GetComponent<Transform>());
		};

//		MapGeneration.doDestroy += RemoveAllPlayers;
	}


	private void RemoveAllPlayers()
	{
		foreach(Transform player in players)
		{
			// Somehow Call Death Event From Here which will then initate the death animation or whatever
			Destroy(player);
		}

		players.Clear(); // clear all refrences to the player object;

	}
}
