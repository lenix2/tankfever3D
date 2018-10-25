using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking.Match;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyListEntry : MonoBehaviour {

	public Text LobbyName;
	public Text LobbyUserCnt;

	private MatchInfoSnapshot _match;
	private MyLobbyManager _networkLobbyManager;
	private MenuManager _menuManager;
	private GameObject _landingPanel;

	// Use this for initialization
	void Start () {
		
	}

	public void SetMatch(MatchInfoSnapshot m) {
		_match = m;
	}
	
	public void SetLandingPanel(GameObject p) {
		_landingPanel = p;
	}
	
	public void SetMenuManager(MenuManager m) {
		_menuManager = m;
	}

	public void SetNetworkManager(MyLobbyManager m) {
		_networkLobbyManager = m;
	}

	public void OnClick() {
		_networkLobbyManager.matchMaker.JoinMatch (_match.networkId, "", "", "", 0, 0, _networkLobbyManager.OnMatchJoined);
		_menuManager.SwitchToPanel(_landingPanel);
	}

	public void SetName(String n)
	{
		LobbyName.text = n;
	}
	
	public void SetUserCnt(String c)
	{
		LobbyUserCnt.text = c;
	}
}
