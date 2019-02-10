using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * manage the lobby-view
 */
public class HostLobbyManager : MonoBehaviour
{

	public List<GameObject> PlayerPanels;
	private MyLobbyManager _networkLobbyManager;

	private float _timer;


	// Use this for initialization
	void Start()
	{
		// find lobby manager
		_networkLobbyManager = GameObject.Find("NetworkManager").GetComponent<MyLobbyManager>();
		_timer = 0f;
	}

	// Update is called once per frame
	void Update ()
	{
		_timer += Time.deltaTime;

		// do task every 0.3 seconds
		if (_timer > 0.3f)
		{
			_timer = 0f;

			// Check for Lobby-Players
			for (int i = 0; i < _networkLobbyManager.lobbySlots.Length; i++)
			{
				// if a player is in the lobby
				if (_networkLobbyManager.lobbySlots[i] != null)
				{
					// show player
					PlayerPanels[i].SetActive(true);
					Text[] texts;
					texts = PlayerPanels[i].GetComponentsInChildren<Text>();
					
					// check if player is ready
					if (_networkLobbyManager.lobbySlots[i].readyToBegin)
					{
						// set 'ready'-text visible
						if (texts.Length > 0)
						{
							texts[0].enabled = true;
						}
					}
					else // if player is not ready
					{
						// set 'ready'-text invisible
						if (texts.Length > 0)
						{
							texts[0].enabled = false;
						}
					}
					
					// Set Colorindex
					_networkLobbyManager.lobbySlots[i].gameObject.GetComponent<ColorControll>().SetColor(i);
				}
				else // if player isn't connected, don't show him
				{
					PlayerPanels[i].SetActive(false);
				}
			}
		}
	}

	// toggle ready state
	public void SwitchReady()
	{
		// find local player
		for (int i = 0; i < _networkLobbyManager.lobbySlots.Length; i++)
		{
			if (_networkLobbyManager.lobbySlots[i] != null) // handle nullpointer
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
