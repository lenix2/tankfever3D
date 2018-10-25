using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankDrive : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer)
		{
			Destroy(this);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		Drive();
	}

	private void Drive()
	{
		this.gameObject.transform.position += this.gameObject.transform.forward * Time.deltaTime * 6f;
	}
}
