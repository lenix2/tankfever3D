using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankShoot : NetworkBehaviour
{

	public GameObject Bullet;
	
	private float _ammoCount;
	
	private float _timer = 0f;
	private float _shootCountdown = 0.5f;
	
	// Use this for initialization
	void Start () {
		_ammoCount = 300f;
	}

	// Update is called once per frame
	void Update ()
	{
		_timer += Time.deltaTime;
		
		float shoot = Input.GetAxis("Shoot");
		if ( shoot > 0.1f)
		{
			DoShoot();
		}
	}

	public void DoShoot()
	{
		if (_timer > _shootCountdown && _ammoCount > 0)
		{
			_timer = 0f;
			_ammoCount--;
			if (isLocalPlayer)
			{
				CmdShoot();
			}
		}
	}

	[Command]
	private void CmdShoot()
	{
		//Instantiate the prefab
		GameObject m_Bullet = Instantiate(Bullet);

		m_Bullet.transform.position = transform.position + transform.forward * 5;
		m_Bullet.transform.rotation = transform.rotation;
		NetworkServer.Spawn(m_Bullet);
	}
}
