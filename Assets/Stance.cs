using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

abstract public class Stance : NetworkBehaviour {

  public bool used = false;

  [SyncVar]
  public NetworkInstanceId parentNetId;

  public virtual float NegotiateDamage(float damage, UnitAction action)
  {
    return(damage);
  }

  public virtual float NegotiateMoveLength(float moveLength)
  {
    return(moveLength);
  }
  
  abstract public string Name();

  abstract public string Description();
}
