using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TankHealth : NetworkBehaviour
{

	public Slider HealthSlider;
	
	[SyncVar]
	public Boolean IsAlive;

	[SyncVar] 
	public float Hitpoints;
	
	private float _maxHP = 100f;
	
	void Start ()
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

	private void UpdateHP()
	{
		HealthSlider.value = Hitpoints;
	}

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

	private void Death()
	{
		if (isServer)
		{
			IsAlive = false;
		}
	}
}
