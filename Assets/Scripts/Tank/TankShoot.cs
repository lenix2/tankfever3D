using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/*
 * Hanlde tank shooting
 */
public class TankShoot : NetworkBehaviour
{

	public GameObject Bullet;
	public Text AmmoText;
	
	[SyncVar]
	private float _ammoCount;
	
	private float _timer = 0f;
	private float _shootCountdown = 0.5f;
	
	// Use this for initialization
	void Start () {
		_ammoCount = 3f;
	}

	// reset ammo every game
	private void OnEnable()
	{
		_ammoCount = 3f;
	}

	// Update is called once per frame
	void Update ()
	{
		_timer += Time.deltaTime;
		
		float shoot = Input.GetAxis("Shoot");
		if (isLocalPlayer)
		{
			
			// check if player is shooting
			if ( shoot > 0.1f && isLocalPlayer)
			{
				DoShoot();
			}
	
			AmmoText.text = "" + _ammoCount;
		}
	}

	/*
	 * perform shot
	 */
	public void DoShoot()
	{
		if (_timer > _shootCountdown && _ammoCount > 0)
		{
			_timer = 0f;
			_ammoCount--;
			if (isLocalPlayer)
			{
				// shotgun
				if (gameObject.GetComponent<TankItemManager>().effectShotgun)
				{
					CmdShoot(-13);
					CmdShoot(0);
					CmdShoot(13);
				}
				else // normal
				{
					CmdShoot(0);
				}
			}
		}
	}

	public void AddAmmo(float a)
	{
		_ammoCount += a;
	}

	// send shooting request to server
	[Command]
	private void CmdShoot(float rot)
	{
		// spawn bullet serverside
		GameObject m_Bullet = Instantiate(Bullet);

		Vector3 forward = transform.forward;
		forward = Quaternion.Euler(0, rot, 0) * forward;
		
		m_Bullet.transform.position = transform.position + forward * 7;
		m_Bullet.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + rot, transform.eulerAngles.z);
		NetworkServer.Spawn(m_Bullet);
	}
}
