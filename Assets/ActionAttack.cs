using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionAttack : NetworkBehaviour, IAction {
  public int TpCost(){
    return(25);
  }

  public int MpCost(){
    return(0);
  }

  public string Name(){
    return("Attack");
  }

  public int MaxDistance(){
    return(1);
  }

  public bool CanTargetSelf(){
    return(false);
  }

  [ClientRpc]
  public void RpcBeginAction(GameObject targetObject){
    //if(cursor.standingUnit){
      //cursor.standingUnit.ReceiveDamage(15);
    //}
  }

  public void DoAction(Cursor cursor){

  }

  public bool NeedsLineOfSight(){
    return(false);
  }
}
