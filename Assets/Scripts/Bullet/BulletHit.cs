using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * Manager bullet collisions
 */
public class BulletHit : NetworkBehaviour
{

	public GameObject Explosion;

	private float _dmg = 25f;
	
	// On Trigger Collision
	private void OnTriggerEnter(Collider other)
	{
		// Collision with border
		if(other.CompareTag("Border"))
		{
			Explode();
			if (isServer)
			{
				NetworkServer.Destroy(gameObject);
			}
		} else if (other.CompareTag("Tank")) // Collision with Tank
		{
			Explode();
			if (isServer)
			{
				NetworkServer.Destroy(gameObject);
				other.gameObject.GetComponent<TankHealth>().DoDmg(_dmg); // Do Damage
			}
		} else if (other.CompareTag("Bullet")) // Collision with other bullets
		{
			Explode();
			
			if (isServer)
			{
				NetworkServer.Destroy(gameObject);
				NetworkServer.Destroy(other.gameObject);
			}
		} else if (other.CompareTag("Loot")) // Collision with Lootboxes
		{
			Explode();
			
			if (isServer)
			{
				NetworkServer.Destroy(gameObject);
				NetworkServer.Destroy(other.gameObject);
			}
		}
	}

	// Play Explosion
	private void Explode()
	{
		Instantiate(Explosion, transform.position, transform.rotation);
	}
}
