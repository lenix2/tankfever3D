using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletMove : NetworkBehaviour
{

	private float _speed = 20f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
	{
		Move();
	}

	private void Move()
	{
		this.gameObject.transform.position += this.gameObject.transform.forward * Time.deltaTime * _speed;
	}
}
