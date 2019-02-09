using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Log basic infos 
 */
public class GameInfo : MonoBehaviour
{

	public Text Infotext;

	private int _time;
	private String _txt;
	
	// Use this for initialization
	void Start ()
	{
		Infotext.text = "";
	}

	// Start a Countdown with given Ending-Text
	public void SetCountdown(int time, String txt)
	{
		_time = time;
		_txt = txt;
		Infotext.text = time + "";
		
		Invoke("DoCountdown", 1);
	}

	// Do Countdown steps
	private void DoCountdown()
	{
		_time--;

		// StepByStep
		if (_time <= 0)
		{
			if (_time <= -1)
			{
				Infotext.text = "";
			}
			else
			{
				Infotext.text = _txt;
				Invoke("DoCountdown", 1);
			}
		}
		else
		{ // Countdown End
			Infotext.text = _time + "";
			Invoke("DoCountdown", 1);
		}
	}
}
