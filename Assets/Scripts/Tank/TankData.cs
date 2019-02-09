using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * store tank information
 */
public class TankData : NetworkBehaviour
{

	// Renderer objects
	public MeshRenderer MeshRenderer1_1;
	public MeshRenderer MeshRenderer1_2;
	public MeshRenderer MeshRenderer2_1;
	public MeshRenderer MeshRenderer2_2;

	// List of materials for this tank (colors)
	public Material[] Materials;

	[SyncVar]
	private int _playercount;

	[SyncVar]
	private int _points = 0;
	
	[SyncVar]
	private int _tankColor = 0;

	private void OnEnable()
	{
		ApplyColor(); // set color to tank
	}

	// apply color to model
	private void ApplyColor()
	{
		int c1 = _tankColor;
		int c2 = _tankColor + 4;

	    Material[] tmpMats1 = new Material[1];
	    Material[] tmpMats2 = new Material[1];
		
		tmpMats1[0] = Materials[c1];
		tmpMats2[0] = Materials[c2];

		MeshRenderer1_1.materials = tmpMats1;
		MeshRenderer1_2.materials = tmpMats2;
		MeshRenderer2_1.materials = tmpMats1;
		MeshRenderer2_2.materials = tmpMats2;
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

	public void SetPlayerCount(int c)
	{
		_playercount = c;
	}

	public int GetPlayerCount()
	{
		return _playercount;
	}
}
