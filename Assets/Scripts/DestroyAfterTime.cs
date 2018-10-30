using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{

	public float Deathtimer;
	
	// Use this for initialization
	void Start () {
		
	}
	
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
