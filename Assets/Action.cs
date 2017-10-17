using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Action : NetworkBehaviour {

  [SyncVar]
  public bool used = false;

  [SyncVar]
  public NetworkInstanceId parentNetId;

  void Start(){
    if (parentNetId != null) { 
      GameObject parentObject = ClientScene.FindLocalObject(parentNetId);
      transform.SetParent(parentObject.transform.Find("Actions"));
    }
  }

  public Unit Unit(){
    return(transform.parent.transform.parent.GetComponent<Unit>());
  }
}