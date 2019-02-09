using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

	public GameObject MainMenuPanel;
	public GameObject MainMenuHostPanel;
	public GameObject MainMenuJoinPanel;
	private MyLobbyManager _networkLobbyManager;
	

	// Use this for initialization
	void Start () {
		SwitchToPanel(MainMenuPanel);
		_networkLobbyManager = GameObject.Find("NetworkManager").GetComponent<MyLobbyManager>();
		LocalizationManager.Read();
		_networkLobbyManager.StartMatchMaker();
	}

	public void SwitchToPanel(GameObject panel)
	{
		MainMenuPanel.SetActive(false);
		MainMenuHostPanel.SetActive(false);
		MainMenuJoinPanel.SetActive(false);
		
		panel.SetActive(true);
	}

	public void ToggleLocalization()
	{
		if (LocalizationManager.Language.Contains("English"))
		{
			LocalizationManager.Language = "German";
		}
		else
		{
			LocalizationManager.Language = "English";
		}
		
	}

	public void HostGameLobby()
	{
		if (_networkLobbyManager.matchMaker != null)
		{
			_networkLobbyManager.matchMaker.CreateMatch("GameLobby",
            			4,
            			true,
            			"",
            			"",
            			"",
            			0,
            			0,
            			_networkLobbyManager.OnMatchCreate);
		}
		
	}

	public void StopLobby()
	{
		_networkLobbyManager.StopHost();
	}
	
}
