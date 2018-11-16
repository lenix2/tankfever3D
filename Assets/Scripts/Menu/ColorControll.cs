using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ColorControll : NetworkBehaviour
{

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
