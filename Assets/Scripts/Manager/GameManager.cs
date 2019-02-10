using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Assets.SimpleLocalization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/**
 * Handle game
 */
public class GameManager : NetworkBehaviour
{

	public GameInfo GameInfo;
	public PointsInfo PointsInfo;
	public Text DebugText;
	public GameObject ItemManager;

	private GameObject[] _tanks;
	private GameObject[] _spawns;

	private int _playercount;
	private bool _startupReady;
	
	
	// Use this for initialization
	void Start ()
	{
		_startupReady = false;
		DoStartup();
	}

	/**
	 * Prepare all players
	 */
	private void DoStartup()
	{
		if (!_startupReady)
		{
			// find tanks and spawns
			_tanks = GameObject.FindGameObjectsWithTag("Tank");
			_spawns = GameObject.FindGameObjectsWithTag("Spawn");
			
			// deactivate item spawn
			ItemManager.SetActive(false);
			
			// if 0 players set to 1 player for debugging without tank
			if (_tanks.Length == 0)
			{
				_playercount = 1;
			}
			else
			{
				// get Playercount
				_playercount = _tanks[0].GetComponent<TankData>().GetPlayerCount();

				// if 0 players set to 1 player for debugging with given tank
				if (_playercount == 0)
				{
					_playercount = 1;
				}
			}
			
			// link this object to tanks to make calls
			foreach (var t in _tanks)
			{
				t.GetComponent<TankHealth>().SetGameManager(this);
			}
			
			// Setup scoreboard
			PointsInfo.SetTanks(_tanks);
			PointsInfo.UpdatePoints();
			
			// Remove all Tanks and spawn them to new spawns
			DisableTanks(); 
			SetTanksUnactive();
			EnableTanks();
			SpawnTanks();
	
			// show number of allready loaded players
			DebugText.text = _tanks.Length + "/" + _playercount;
			
			// all players loaded
			if (_playercount == _tanks.Length)
			{
				_startupReady = true;
				DebugText.text = "";
				
				// Start
				StartRound();
			}	
		}
	}

	// Do Startup for every tank, last tank starts the round.
	public void NewTankLoaded()
	{
		DoStartup();
	}

	private void StartRound()
	{
		// Reset tanks
		SetTanksUnactive();
		EnableTanks();
		SpawnTanks();
		
		// Serverside reset loot
		if (isServer)
		{
			ItemManager.GetComponent<LootManager>().RemoveAllLootcrates();
			ItemManager.GetComponent<LootManager>().RemoveAllItemEffects();
		}
		
		// Start countdown
		GameInfo.SetCountdown(3, LocalizationManager.Localize("Game.Start"));
		Invoke("SetTanksActive", 3);
	}

	// disable all tanks
	private void DisableTanks()
	{
		foreach (GameObject t in _tanks)
		{
			t.SetActive(false);
		}
	}

	// enable all tanks
	private void EnableTanks()
	{
		foreach (GameObject t in _tanks)
		{
			t.SetActive(true);
		}
	}

	// set tanks controllable
	private void SetTanksActive()
	{
		foreach (GameObject t in _tanks)
		{
			t.GetComponent<TankDrive>().enabled = true;
			t.GetComponent<TankHealth>().enabled = true;
			t.GetComponent<TankShoot>().enabled = true;
			// t.GetComponent<TankRotate>().enabled = true;
		}
		
		ItemManager.SetActive(true);
	}

	// set tanks uncontrollable
	private void SetTanksUnactive()
	{
		foreach (GameObject t in _tanks)
		{
			t.GetComponent<TankDrive>().enabled = false;
			t.GetComponent<TankHealth>().enabled = false;
			t.GetComponent<TankShoot>().enabled = false;
			//t.GetComponent<TankRotate>().enabled = false;
		}
		
		ItemManager.SetActive(false);
	}

	// spawn all tanks
	private void SpawnTanks()
	{
		for (int i = 0; i < _tanks.Length; i++)
		{
			_tanks[i].transform.position = _spawns[i].transform.position;
			_tanks[i].transform.rotation = _spawns[i].transform.rotation;
		}
	}

	// start next round
	private void NextRound()
	{
		// update scores
		UpdatePoints();
		
		// block tanks
		DisableTanks();
		
		// check if game is over
		GameObject winner = CheckForWinner();

		if (winner == null)
		{
			// start new round 
			StartRound();
		}
		else
		{
			// end game 
			DebugText.text = LocalizationManager.Localize("Game.GameOver") + ": " + "\n" + LocalizationManager.Localize(winner.GetComponent<TankData>().GetPlayerName());
			Invoke("QuitGame", 5);
		}
	}

	// To Enable Invoke-use
	// Trigger update
	private void UpdatePoints()
	{
		PointsInfo.UpdatePoints();
	}

	// Tank was destroyed callback
	public void TankDied(GameObject go)
	{
		if (isServer)
		{
			int alive = 0;
            		
			// Server gives all other alive players a point
			foreach (var t in _tanks)
			{
				if (t.GetComponent<TankHealth>().IsAlive)
				{
					t.GetComponent<TankData>().AddPoints(1);
					alive++;
				}
			}
	
			// start new round if 1 or less tanks are living
			if (alive < 2)
			{
				// Invoke("NextRound", 3);
				RpcNextRound();
			}
		}
		
		Invoke("UpdatePoints", 1);
	}

	// Start new round on clients
	[ClientRpc]
	private void RpcNextRound()
	{
		Invoke("NextRound", 3);
	}

	// check if a tank has enough points to win the match
	private GameObject CheckForWinner()
	{
		List<GameObject> winners = new List<GameObject>();
		
		GameObject winner = null;
		
		// get winners
		foreach (GameObject t in _tanks)
		{
			if (t.GetComponent<TankData>().GetPoints() >= (_tanks.Length - 1) * 10)
			{
				winners.Add(t);
			}
		}

		// if 1 winner -> he has won
		if (winners.Count == 1)
		{
			return winners[0];
		} else if (winners.Count > 1)
		{ // if more winners -> one has to be 2 points ahead
			int maxPoints = 0;
			
			foreach (GameObject t in winners)
			{
				if (t.GetComponent<TankData>().GetPoints() >= maxPoints + 2)
				{
					maxPoints = t.GetComponent<TankData>().GetPoints();
					winner = t;
				} else if (t.GetComponent<TankData>().GetPoints() >= maxPoints - 2)
				{
					winner = null;
				}
			}
		}

		return winner;
	}

	// go back to main menu
	private void QuitGame()
	{
		MyLobbyManager networkLobbyManager = GameObject.Find("NetworkManager").GetComponent<MyLobbyManager>();
		
		networkLobbyManager.StopHost();
	}
}
