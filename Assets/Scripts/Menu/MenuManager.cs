using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

	public GameObject MainMenuPanel;
	public GameObject MainMenuHostPanel;
	public GameObject MainMenuJoinPanel;
	public MyLobbyManager NetworkLobbyManager;
	

	// Use this for initialization
	void Start () {
		SwitchToPanel(MainMenuPanel);
		NetworkLobbyManager.StartMatchMaker();
	}

	public void SwitchToPanel(GameObject panel)
	{
		MainMenuPanel.SetActive(false);
		MainMenuHostPanel.SetActive(false);
		MainMenuJoinPanel.SetActive(false);
		
		panel.SetActive(true);
	}

	public void HostGameLobby()
	{
		if (NetworkLobbyManager.matchMaker != null)
		{
			NetworkLobbyManager.matchMaker.CreateMatch("TESTNAME",
            			4,
            			true,
            			"",
            			"",
            			"",
            			0,
            			0,
            			NetworkLobbyManager.OnMatchCreate);
		}
		
	}

	public void StopLobby()
	{
		NetworkLobbyManager.StopHost();
		
		NetworkLobbyManager.StartMatchMaker();
		
	}
	
}
