using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Remove Environment-Objects
 */
public class EnviromentDestroyable : MonoBehaviour
{

	public GameObject Explosion;

	// Play Explosion after collision and delete Object
	private void OnTriggerEnter(Collider other)
	{
		Instantiate(Explosion, transform.position, transform.rotation);
		Destroy(this.gameObject);
	}
}
