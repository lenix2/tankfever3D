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
				Pointtexts[i].color = GetColorById(_tanks[i].GetComponent<TankData>().GetColor());
			}
			else
			{
				Pointtexts[i].text = " ";
			}
		}
	}

	// return color by id
	public Color GetColorById(int i)
	{
		if (i == 0)
		{
			return Color.red;
		} if (i == 1)
		{
			return Color.green;
		} if (i == 2)
		{
			return Color.blue;
		} if (i == 3)
		{
			return Color.magenta;
		}

		return Color.black;
	}
}
