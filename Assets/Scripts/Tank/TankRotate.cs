using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankRotate : NetworkBehaviour
{

	private float _turnspeed = 70f;

	public Joystick TurnStick;
	
	// Use this for initialization
	void Start () {
		if (!isLocalPlayer)
		{
			Destroy(this);
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
		float rot = Input.GetAxis("Horizontal");
		if (TurnStick != null)
		{
			rot += TurnStick.Horizontal;
		}
		if ( rot > 0.1f || rot < -0.1f)
		{
			Rotate(rot);
		}
	}

	private void Rotate(float r)
	{
		this.gameObject.transform.eulerAngles =  new Vector3(
			this.gameObject.transform.eulerAngles.x,
			this.gameObject.transform.eulerAngles.y + r * _turnspeed * Time.deltaTime,
			this.gameObject.transform.eulerAngles.z);
	}
}
