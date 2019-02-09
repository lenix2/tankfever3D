using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/**
 * Log network Bandwidth
 */
public class ShowBandwidth : NetworkBehaviour {

	private float _networkTotal = 0f;
    private float _networkRate = 0f;
	private float _tmpTotal = 0f;
	private float _timer = 0f;

	public Text BandwidthText;
	
	// Update is called once per frame
	void Update () {
		_timer += Time.deltaTime;
    		
		if (_timer > 1f)
		{
			_tmpTotal = _networkTotal;
			_networkTotal = NetworkTransport.GetOutgoingFullBytesCount();
			_networkRate = (_networkTotal - _tmpTotal) / _timer;
			
			_timer = 0f;
		}
    		
		BandwidthText.text = Mathf.Ceil(_networkRate).ToString() + " B/s";
	}
}
