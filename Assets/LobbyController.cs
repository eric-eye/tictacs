using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LobbyController : NetworkBehaviour {

	public static LobbyController instance;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartBattle(){
		SceneManager.LoadScene("BattleScene");
		RpcLoadBattle();
	}

	[ClientRpc]
	public void RpcLoadBattle(){
		SceneManager.LoadScene("BattleScene");
	}
}
