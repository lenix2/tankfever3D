using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// manage button input
public class ShootInput : NetworkBehaviour {
	public void Shoot()
	{
		GameObject[] go = GameObject.FindGameObjectsWithTag("Tank");
        
		foreach (GameObject tank in go)
		{
			if (tank.GetComponent<NetworkIdentity>().isLocalPlayer)
			{
				// lets local player tank shoot
				tank.GetComponent<TankShoot>().DoShoot();
			}
		}
	}
}
