using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

abstract public class Stance : NetworkBehaviour {

  public bool used = false;

  [SyncVar]
  public NetworkInstanceId parentNetId;

  void Start(){
    if (parentNetId != null) { 
      GameObject parentObject = ClientScene.FindLocalObject(parentNetId);
      transform.SetParent(parentObject.transform.Find("Stances"));
    }
  }

  abstract public int NegotiateDamage(int damage);
  abstract public int NegotiateMoveLength(int moveLength);
  abstract public string Name();
}
