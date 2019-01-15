using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
			_tanks = GameObject.FindGameObjectsWithTag("Tank");
			_spawns = GameObject.FindGameObjectsWithTag("Spawn");
			ItemManager.SetActive(false);
			
			if (_tanks.Length == 0)
			{
				_playercount = 1;
			}
			else
			{
				_playercount = _tanks[0].GetComponent<TankData>().GetPlayerCount();

				if (_playercount == 0)
				{
					_playercount = 1;
				}
			}
			
			foreach (var t in _tanks)
			{
				t.GetComponent<TankHealth>().SetGameManager(this);
			}
			
			PointsInfo.SetTanks(_tanks);
			PointsInfo.UpdatePoints();
			DisableTanks();
			SetTanksUnactive();
			EnableTanks();
			SpawnTanks();
	
			DebugText.text = _tanks.Length + "/" + _playercount;
			
			if (_playercount == _tanks.Length)
			{
				_startupReady = true;
				DebugText.text = "";
				StartRound();
			}	
		}
		
	}

	public void NewTankLoaded()
	{
		DoStartup();
	}

	private void StartRound()
	{
		SetTanksUnactive();
		EnableTanks();
		SpawnTanks();
		ItemManager.GetComponent<LootManager>().RemoveAllLootcrates();
		ItemManager.GetComponent<LootManager>().RemoveAllItemEffects();
		GameInfo.SetCountdown(3, "GO!");
		Invoke("SetTanksActive", 3);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	private void DisableTanks()
	{
		foreach (GameObject t in _tanks)
		{
			t.SetActive(false);
		}
	}

	private void EnableTanks()
	{
		foreach (GameObject t in _tanks)
		{
			t.SetActive(true);
		}
		
	}

	private void SetTanksActive()
	{
		foreach (GameObject t in _tanks)
		{
			t.GetComponent<TankDrive>().enabled = true;
			t.GetComponent<TankHealth>().enabled = true;
			t.GetComponent<TankShoot>().enabled = true;
			//t.GetComponent<TankRotate>().enabled = true;
		}
		
		ItemManager.SetActive(true);
	}

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

	private void SpawnTanks()
	{
		for (int i = 0; i < _tanks.Length; i++)
		{
			_tanks[i].transform.position = _spawns[i].transform.position;
			_tanks[i].transform.rotation = _spawns[i].transform.rotation;
		}
	}

	private void NextRound()
	{
		UpdatePoints();
		DisableTanks();
		
		GameObject winner = CheckForWinner();

		if (winner == null)
		{
			StartRound();
		}
		else
		{
			DebugText.text = "GameOver";
			Invoke("QuitGame", 5);
		}
	}

	// To Enable Invoke-use
	private void UpdatePoints()
	{
		PointsInfo.UpdatePoints();
	}

	public void TankDied(GameObject go)
	{
		if (isServer)
		{
			int alive = 0;
            		
			foreach (var t in _tanks)
			{
				if (t.GetComponent<TankHealth>().IsAlive)
				{
					t.GetComponent<TankData>().AddPoints(1);
					alive++;
				}
			}
	
			if (alive < 2)
			{
				Invoke("NextRound", 3);
				RpcNextRound();
			}
		}
		
		Invoke("UpdatePoints", 1);
	}

	[ClientRpc]
	private void RpcNextRound()
	{
		Invoke("NextRound", 3);
	}

	private GameObject CheckForWinner()
	{
		List<GameObject> winners = new List<GameObject>();
		
		GameObject winner = null;
		
		foreach (GameObject t in _tanks)
		{
			if (t.GetComponent<TankData>().GetPoints() >= (_tanks.Length - 1) * 10)
			{
				winners.Add(t);
			}
		}

		if (winners.Count == 1)
		{
			return winners[0];
		} else if (winners.Count > 1)
		{
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

	private void QuitGame()
	{
		MyLobbyManager networkLobbyManager = GameObject.Find("NetworkManager").GetComponent<MyLobbyManager>();
		
		networkLobbyManager.StopHost();
	}
}
