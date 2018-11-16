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
	public MyLobbyManager NetworkLobbyManager;

	private float Timer;


	// Use this for initialization
	void Start()
	{
		Timer = 0f;
	}

	// Update is called once per frame
	void Update ()
	{
		Timer += Time.deltaTime;

		if (Timer > 0.3f)
		{
			Timer = 0f;

			// Check for Lobby-Players
			for (int i = 0; i < NetworkLobbyManager.lobbySlots.Length; i++)
			{
				if (NetworkLobbyManager.lobbySlots[i] != null)
				{
					PlayerPanels[i].SetActive(true);

					if (NetworkLobbyManager.lobbySlots[i].readyToBegin)
					{
						PlayerPanels[i].GetComponentsInChildren<Text>()[0].enabled = true;
					}
					else
					{
						PlayerPanels[i].GetComponentsInChildren<Text>()[0].enabled = false;
					}
					
					// Set Colorindex
					NetworkLobbyManager.lobbySlots[i].gameObject.GetComponent<ColorControll>().SetColor(i);
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
		for (int i = 0; i < NetworkLobbyManager.lobbySlots.Length; i++)
		{
			if (NetworkLobbyManager.lobbySlots[i] != null)
			{
				if (NetworkLobbyManager.lobbySlots[i].hasAuthority) // Get Local LobbyPlayer
				{
					
					// Set Ready/Not Ready
					if (!NetworkLobbyManager.lobbySlots[i].readyToBegin)
					{
						NetworkLobbyManager.lobbySlots[i].SendReadyToBeginMessage();
					}
					else
					{
						NetworkLobbyManager.lobbySlots[i].SendNotReadyToBeginMessage();
					}
				}
			}
		}
	}
}
