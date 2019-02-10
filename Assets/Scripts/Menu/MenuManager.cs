using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

/*
 * manage main-menu and manuinteraction
 */
public class MenuManager : MonoBehaviour
{

	public GameObject MainMenuPanel;
	public GameObject MainMenuHostPanel;
	public GameObject MainMenuJoinPanel;
	private MyLobbyManager _networkLobbyManager;

	private int _quality = 1;
	

	// Use this for initialization
	void Start () {
		SwitchToPanel(MainMenuPanel); // show main menu on start
		_networkLobbyManager = GameObject.Find("NetworkManager").GetComponent<MyLobbyManager>(); // find networkmanager
		LocalizationManager.Read(); // find localizationmanager
		_networkLobbyManager.StartMatchMaker(); // enable unity-machtmaking service
		
		// check if System language is german
		if (Application.systemLanguage == SystemLanguage.German)
		{
			LocalizationManager.Language = "German";
		}
		else // else set language to english
		{
			LocalizationManager.Language = "English";
		}
		
		QualitySettings.SetQualityLevel(_quality, true);
	}

	// switch to other views
	public void SwitchToPanel(GameObject panel)
	{
		// disable all views
		MainMenuPanel.SetActive(false);
		MainMenuHostPanel.SetActive(false);
		MainMenuJoinPanel.SetActive(false);
		
		// enable target view
		panel.SetActive(true);
	}

	// change language
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
	
	// change graphics
	public void ToggleQuality()
	{
		_quality++;
		if (_quality > 2)
		{
			_quality = 0;
		}
		QualitySettings.SetQualityLevel(_quality, true);
	}

	// start a lobby
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

	// stop a lobby
	public void StopLobby()
	{
		_networkLobbyManager.StopHost();
	}
}
