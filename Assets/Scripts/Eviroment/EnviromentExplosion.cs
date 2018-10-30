using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentExplosion : MonoBehaviour
{

	public List<ParticleSystem> ParticleSystems;
	public float Lifetime = 1f;

	private float _timer = 0f;
	
	// Use this for initialization
	void Start () {
		for (int i = 0; i < ParticleSystems.Count; i++)
		{
			ParticleSystems[i].Play();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		_timer += Time.deltaTime;

		if (_timer > Lifetime)
		{
			Destroy(this.gameObject);
		}
	}
}
