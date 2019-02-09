using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// rotate tank info canvas to camera
public class RotateCanvas : MonoBehaviour
{

	public Transform Canvas;
	
	// Update is called once per frame
	void Update () {
		Canvas.eulerAngles = new Vector3(90,0,0);
	}
}
