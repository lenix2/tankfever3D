using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class MyLobbyManager : NetworkLobbyManager
{
	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
	{
		Debug.Log("Player started");
		gamePlayer.gameObject.GetComponent<TankData>().SetColor(lobbyPlayer.gameObject.GetComponent<ColorControll>()
					.GetColor()); 
		
		return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
	}
}
