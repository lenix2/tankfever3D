using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * object to store color information of players
 * needed to save this information for the game scene
 */
public class ColorControll : NetworkBehaviour
{

	// color-information is synced 
	[SyncVar]
	private int _color = 0;

	public void SetColor(int c)
	{
		_color = c;
	}

	public int GetColor()
	{
		return _color;
	}
}
