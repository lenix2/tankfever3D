using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Destroy this Object if Platform is Unity-Editor
 */
public class DestroyOnProductive : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!Application.isEditor)
		{
			Destroy(this.gameObject);
		}
	}
}
