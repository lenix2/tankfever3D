using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankDrive : NetworkBehaviour
{

	private float _speed = 8f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
	{
		Drive();
	}

	private void Drive()
	{
		this.gameObject.transform.position += this.gameObject.transform.forward * Time.deltaTime * _speed;
	}
}
