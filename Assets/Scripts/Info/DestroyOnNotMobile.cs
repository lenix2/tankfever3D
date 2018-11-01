using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnNotMobile : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (Application.platform != RuntimePlatform.Android)
		{
			Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
