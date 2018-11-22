using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class JoinLobbyList : MonoBehaviour
{

	public GameObject ListPanel;
	public GameObject ListEntry;
	private MyLobbyManager _networkLobbyManager;
	
	public MenuManager MenuManager;
	public GameObject LandingPanel;

	private float _timer = 1f;
	
	// Use this for initialization
	void Start () {
		_networkLobbyManager = GameObject.Find("NetworkManager").GetComponent<MyLobbyManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		_timer += Time.deltaTime;

		if (_timer > 1f)
		{
			_timer = 0f;

			CreateList();
		}
	}

	private void CreateList()
	{
		_networkLobbyManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, _networkLobbyManager.OnMatchList);

		foreach (Transform child in ListPanel.transform)
		{
			Destroy(child.gameObject);
		}

		int offset = 0;
		int contentsize = 0;
		if (_networkLobbyManager.matches != null)
		{
			foreach (MatchInfoSnapshot match in _networkLobbyManager.matches)
			{
				GameObject tmpListEntry = Instantiate(ListEntry);

				tmpListEntry.transform.SetParent(ListPanel.transform, false);

				tmpListEntry.transform.position = new Vector3(tmpListEntry.transform.position.x, tmpListEntry.transform.position.y - offset, tmpListEntry.transform.position.z);		
				
				RectTransform rt = ListPanel.GetComponent<RectTransform>();
				rt.sizeDelta = new Vector2(rt.sizeDelta.x, contentsize);

				tmpListEntry.GetComponent<JoinLobbyListEntry>().SetMatch(match);

				tmpListEntry.GetComponent<JoinLobbyListEntry>().SetNetworkManager(_networkLobbyManager);
				
				tmpListEntry.GetComponent<JoinLobbyListEntry>().SetName(match.name);
				
				tmpListEntry.GetComponent<JoinLobbyListEntry>().SetUserCnt(match.currentSize + "/" + match.maxSize);
				
				tmpListEntry.GetComponent<JoinLobbyListEntry>().SetLandingPanel(LandingPanel);
				
				tmpListEntry.GetComponent<JoinLobbyListEntry>().SetMenuManager(MenuManager);
				
				if (Application.platform == RuntimePlatform.Android)
				{
					offset += 110;
				}
				else
				{
					offset += 55;
					contentsize += 155;
				}
			}
		}
	}
}
