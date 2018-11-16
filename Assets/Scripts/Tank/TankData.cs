using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankData : NetworkBehaviour
{

	public MeshRenderer MeshRenderer;

	public Material[] Materials;

	[SyncVar]
	private int _points = 0;
	
	[SyncVar]
	private int _tankColor = 0;
	
	// Use this for initialization
	void Start () {
		Debug.Log(isClient);
		Debug.Log(isServer);
		Debug.Log(isLocalPlayer);
	}

	private void OnEnable()
	{
		ApplyColor();
	}

	// Update is called once per frame
	void Update () {
		
	}

	// apply color to model
	private void ApplyColor()
	{
		Debug.Log("APPLY COLOR" + _tankColor);
		
		int c1 = _tankColor;
		int c2 = _tankColor + 4;

	    Material[] tmpMats = MeshRenderer.materials;
		
		tmpMats[0] = Materials[c1];
		tmpMats[2] = Materials[c2];
		tmpMats[4] = Materials[c1];
		tmpMats[7] = Materials[c1];
		tmpMats[8] = Materials[c1];

		MeshRenderer.materials = tmpMats;
	}
	
	public void AddPoints(int p)
	{
		_points += p;
	}

	public int GetPoints()
	{
		return _points;
	}
	
	public void SetColor(int c)
	{
		_tankColor += c;
	}

	public int GetColor()
	{
		return _tankColor;
	}
}
