using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/**
 * Manage Tank rotation
 */
public class TankRotate : NetworkBehaviour
{

	private float _turnspeed = 70f;

	public Joystick TurnStick;
	
	// Use this for initialization
	void Start () {
		if (!isLocalPlayer)
		{
			Destroy(this); // only rotate ur own tank
		}
	
		GameObject[] go = GameObject.FindGameObjectsWithTag("MobileController");
        
		foreach (GameObject mc in go)
		{
			TurnStick = mc.GetComponent<Joystick>();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		float rot = Input.GetAxis("Horizontal"); // rotate with buttons
		if (TurnStick != null)
		{
			rot += TurnStick.Horizontal; // rotate with stick
		}
		if ( rot > 0.1f || rot < -0.1f)
		{
			Rotate(rot); // do rotation
		}
	}

	/*
	 * Do rotation relative to deltatime
	 */
	private void Rotate(float r)
	{
		this.gameObject.transform.eulerAngles =  new Vector3(
		this.gameObject.transform.eulerAngles.x,
		this.gameObject.transform.eulerAngles.y + r * _turnspeed * Time.deltaTime,
		this.gameObject.transform.eulerAngles.z);
	}
}
