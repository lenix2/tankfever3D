using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentDestroyable : MonoBehaviour
{

	public GameObject Explosion;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		Instantiate(Explosion, transform.position, transform.rotation);
		Destroy(this.gameObject);
	}
}
