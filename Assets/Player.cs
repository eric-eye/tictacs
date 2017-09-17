using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

  public GameObject gameControllerPrefab;

	// Use this for initialization
	void Start () {
    if(isServer && isLocalPlayer){
      CmdSpawnGameController();
    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  [Command]
  void CmdSpawnGameController(){
    GameObject gameController = Instantiate(gameControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(gameController);
  }
}
