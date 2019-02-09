using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Update player Points
 */
public class PointsInfo : MonoBehaviour
{

	public Text[] Pointtexts;

	private GameObject[] _tanks;

	public void SetTanks(GameObject[] tanks)
	{
		_tanks = tanks;
	}

	/*
	 * Print every Score to the scoreboard
	 */
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
