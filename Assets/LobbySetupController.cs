using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbySetupController : MonoBehaviour {

	public GameObject lobbyControllerPrefab;
	private bool spawned = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    if(!NetworkServer.active){
      return;
    }

		if(!spawned){
      GameObject lobby = Instantiate(lobbyControllerPrefab, Vector3.zero, Quaternion.identity);
      NetworkServer.Spawn(lobby);
      spawned = true;
    }
  }
}
