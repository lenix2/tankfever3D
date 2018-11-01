using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletHit : NetworkBehaviour
{

	public GameObject Explosion;

	private float _dmg = 25f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Border"))
		{
			Explode();

			if (isServer)
			{
				NetworkServer.Destroy(gameObject);
			}
		} else if (other.CompareTag("Tank"))
		{
			Explode();
			
			if (isServer)
			{
				NetworkServer.Destroy(gameObject);
				other.gameObject.GetComponent<TankHealth>().DoDmg(_dmg);
			}
		}
	}

	private void Explode()
	{
		Instantiate(Explosion, transform.position, transform.rotation);
	}
}
