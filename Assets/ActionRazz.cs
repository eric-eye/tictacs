using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionRazz : NetworkBehaviour, IAction {
  public GameObject effectPrefab;

  public int TpCost(){
    return(40);
  }

  public int MpCost(){
    return(5);
  }

  public string Name(){
    return("Razz");
  }

  public int MaxDistance(){
    return(4);
  }

  public bool CanTargetSelf(){
    return(false);
  }

  [ClientRpc]
  public void RpcBeginAction(GameObject targetObject){
    //if(cursor.standingUnit){
      //GameObject effect = Instantiate(effectPrefab, Vector3.zero, Quaternion.identity);
      //cursor.standingUnit.ReceiveBuff(effect);
    //}
  }

  public void DoAction(Cursor cursor){

  }

  public bool NeedsLineOfSight(){
    return(false);
  }
}
