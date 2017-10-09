using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionThrowStone : NetworkBehaviour, IAction {
  public int TpCost(){
    return(30);
  }

  public int MpCost(){
    return(0);
  }

  public string Name(){
    return("Throw Stone");
  }

  public int MaxDistance(){
    return(5);
  }

  public bool CanTargetSelf(){
    return(false);
  }

  [ClientRpc]
  public void RpcBeginAction(GameObject targetObject){
    //if(cursor.standingUnit){
      //cursor.standingUnit.ReceiveDamage(5);
    //}
  }

  public void DoAction(Cursor cursor){

  }

  public bool NeedsLineOfSight(){
    return(true);
  }
}
