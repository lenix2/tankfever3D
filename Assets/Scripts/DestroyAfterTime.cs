using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Destroy this object after given time
 */
public class DestroyAfterTime : MonoBehaviour
{

	public float Deathtimer;

	// Update is called once per frame
	void Update ()
	{
		Deathtimer -= Time.deltaTime;

		if (Deathtimer < 0)
		{
			Destroy(this.gameObject);
		}
	}
}
