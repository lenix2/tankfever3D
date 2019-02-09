using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;

/**
 * Destroy this object if platform isn't Android
 */
public class DestroyOnNotMobile : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (Application.platform != RuntimePlatform.Android)
		{
			Destroy(this.gameObject);
		}
	}
}
