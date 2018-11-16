﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{


	private GameObject[] _tanks;
	private GameObject[] _spawns;

	private float _time;
	
	// Use this for initialization
	void Start () {
		_tanks = GameObject.FindGameObjectsWithTag("Tank");
		_spawns = GameObject.FindGameObjectsWithTag("Spawn");
		_time = 0;
		
		DisableTanks();
		StartRound();
	}

	private void StartRound()
	{
		EnableTanks();
		SpawnTanks();
		SetTanksUnactive();
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
		}
	}

	private void SetTanksUnactive()
	{
		foreach (GameObject t in _tanks)
		{
			t.GetComponent<TankDrive>().enabled = false;
			t.GetComponent<TankHealth>().enabled = false;
			t.GetComponent<TankShoot>().enabled = false;
		}
	}

	private void SpawnTanks()
	{
		for (int i = 0; i < _tanks.Length; i++)
		{
			_tanks[i].transform.position = _spawns[i].transform.position;
		}
	}

	private void NextRound()
	{
		DisableTanks();
		StartRound();
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
				Invoke("NextRound", 2);
				RpcNextRound();
			}
		}
	}

	[ClientRpc]
	private void RpcNextRound()
	{
		Invoke("NextRound", 2);
	}
}
