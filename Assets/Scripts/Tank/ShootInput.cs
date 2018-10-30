using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShootInput : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Shoot()
	{
		GameObject[] go = GameObject.FindGameObjectsWithTag("Tank");
        
		foreach (GameObject tank in go)
		{
			if (tank.GetComponent<NetworkIdentity>().isLocalPlayer)
			{
				tank.GetComponent<TankShoot>().DoShoot();
			}
		}
	}
}
