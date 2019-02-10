using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

/**
 * Custom Networklobbymanager
 */
public class MyLobbyManager : NetworkLobbyManager
{
	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
	{
		// after a player is loaded into the main game, set the color he had in the lobby
		Debug.Log("Player started");
		gamePlayer.gameObject.GetComponent<TankData>().SetColor(lobbyPlayer.gameObject.GetComponent<ColorControll>().GetColor()); 
		
		// number of lobby players and number of game players have to be the same 
		// send this information to the game player
		gamePlayer.gameObject.GetComponent<TankData>().SetPlayerCount(this.numPlayers);
		
		// do normal OnLobbyServerSceneLoadedForPlayer method
		return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
	}
}
