using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * Bullet Movement
 */
public class BulletMove : NetworkBehaviour
{

	private float _speed = 20f; // Speed
	
	// Update is called once per frame
	void Update ()
	{
		Move();
	}

	private void Move()
	{
		// Update Position
		this.gameObject.transform.position += this.gameObject.transform.forward * Time.deltaTime * _speed;
	}
}
