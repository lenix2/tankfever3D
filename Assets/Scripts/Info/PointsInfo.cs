using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsInfo : MonoBehaviour
{

	public Text[] Pointtexts;

	private GameObject[] _tanks;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetTanks(GameObject[] tanks)
	{
		_tanks = tanks;
	}

	public void UpdatePoints()
	{
		for (int i = 0; i < Pointtexts.Length; i++)
		{
			if (i < _tanks.Length)
			{
				Pointtexts[i].text = _tanks[i].GetComponent<TankData>().GetPoints() + " ";
			}
			else
			{
				Pointtexts[i].text = " ";
			}
		}
	}
}
