using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyStatusText : MonoBehaviour {

	private Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

		string message = "";
		if(NetworkServer.connections.Count == 0){
			message = "Please connect or host.";
		}else{
			message = "Players in lobby: " + Player.players.Count;
		}
		text.text = message;
	}
}
