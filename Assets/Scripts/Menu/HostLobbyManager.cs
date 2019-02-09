using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HostLobbyManager : MonoBehaviour
{

	public List<GameObject> PlayerPanels;
	private MyLobbyManager _networkLobbyManager;

	private float _timer;


	// Use this for initialization
	void Start()
	{
		_networkLobbyManager = GameObject.Find("NetworkManager").GetComponent<MyLobbyManager>();
		_timer = 0f;
	}

	// Update is called once per frame
	void Update ()
	{
		_timer += Time.deltaTime;

		if (_timer > 0.3f)
		{
			_timer = 0f;

			// Check for Lobby-Players
			for (int i = 0; i < _networkLobbyManager.lobbySlots.Length; i++)
			{
				if (_networkLobbyManager.lobbySlots[i] != null)
				{
					PlayerPanels[i].SetActive(true);
					Text[] texts;
					texts = PlayerPanels[i].GetComponentsInChildren<Text>();
					
					if (_networkLobbyManager.lobbySlots[i].readyToBegin)
					{
						if (texts.Length > 0)
						{
							texts[0].enabled = true;
						}
					}
					else
					{
						texts = PlayerPanels[i].GetComponentsInChildren<Text>();
						if (texts.Length > 0)
						{
							texts[0].enabled = false;
						}
					}
					
					// Set Colorindex
					_networkLobbyManager.lobbySlots[i].gameObject.GetComponent<ColorControll>().SetColor(i);
				}
				else
				{
					PlayerPanels[i].SetActive(false);
				}
			}
		}
	}

	public void SwitchReady()
	{
		for (int i = 0; i < _networkLobbyManager.lobbySlots.Length; i++)
		{
			if (_networkLobbyManager.lobbySlots[i] != null)
			{
				if (_networkLobbyManager.lobbySlots[i].hasAuthority) // Get Local LobbyPlayer
				{
					
					// Set Ready/Not Ready
					if (!_networkLobbyManager.lobbySlots[i].readyToBegin)
					{
						_networkLobbyManager.lobbySlots[i].SendReadyToBeginMessage();
					}
					else
					{
						_networkLobbyManager.lobbySlots[i].SendNotReadyToBeginMessage();
					}
				}
			}
		}
	}
}
