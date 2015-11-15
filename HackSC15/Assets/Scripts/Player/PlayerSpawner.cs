using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(MapGeneration))]
public class PlayerSpawner : MonoBehaviour {
	/// <summary>
	/// Spawns a player randomly on any optimal position on the map.	/// </summary>
	/// 
	public GameObject playerPrefab;
	// Need to somehow use HFT here Lucas
	public Stack<Transform> players = new Stack<Transform>();

	void Start()
	{
		// Subscribe to the Necessary Events 
		MapGeneration.onCreate += delegate(Vector3 position) {
			players.Push(Instantiate(playerPrefab, position, Quaternion.identity) as Transform);
		};

		MapGeneration.doDestroy += RemoveAllPlayers;
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
