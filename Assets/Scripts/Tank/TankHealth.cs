using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TankHealth : NetworkBehaviour
{

	public Slider HealthSlider;
	public GameObject Explosion;
	
	[SyncVar]
	public Boolean IsAlive;

	[SyncVar] 
	public float Hitpoints;
	
	private float _maxHP = 100f;

	private GameManager _gameManager;
	
	void Start ()
	{
	
	}

	public void SetGameManager(GameManager gm)
	{
		_gameManager = gm;
	}

	private void OnEnable()
	{
		Hitpoints = _maxHP;
		HealthSlider.maxValue = _maxHP;
		HealthSlider.minValue = 0f;
		IsAlive = true;

		UpdateHP();
	}

	// Update is called once per frame
	void Update ()
	{
		UpdateHP();
	}

	private void OnCollisionEnter(Collision other)
	{
		if(other.collider.CompareTag("Border"))
		{
			if (isServer)
			{
				DoDmg(1000f);
			}
		}
	}

	private void UpdateHP()
	{
		HealthSlider.value = Hitpoints;
	}

	/**
	 * DO Dmg only on server with later sync
	 */
	public void DoDmg(float dmg)
	{
		if (isServer)
		{
			if (IsAlive)
			{
				Hitpoints -= dmg;
	
				if (Hitpoints <= 0)
				{
					Hitpoints = 0;
					Death();
				}
			}
		}
		
	}

	/**
	 * Kill Player on Server
	 */
	private void Death()
	{
		if (isServer)
		{
			IsAlive = false;
			_gameManager.TankDied(this.gameObject);
			RpcDeath();
		}

		Explode();
		gameObject.SetActive(false);
	}
	
	/**
	 * Kill Player on Client
	 */
	[ClientRpc]
	private void RpcDeath()
	{
		IsAlive = false;

		Explode();
		gameObject.SetActive(false);
	}
	
	/**
	 * Spawn Explosion
	 */
	private void Explode()
	{
		Instantiate(Explosion, transform.position, transform.rotation);
	}
}
