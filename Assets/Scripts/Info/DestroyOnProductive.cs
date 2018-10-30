using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnProductive : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (Application.isEditor)
		{
			Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
