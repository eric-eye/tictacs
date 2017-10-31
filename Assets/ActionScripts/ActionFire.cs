using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionFire : Action, IAction {
  public int TpCost(){
    return(35);
  }

  public int MpCost(){
    return(5);
  }

  public string Name(){
    return("Fire");
  }
  
  public string Description(){
    return("Cast a fire spell. Direct damage.");
  }

  public int MaxDistance(){
    return(5);
  }

  public bool CanTargetSelf(){
    return(true);
  }

  public void DoAction(Cursor cursor){
    if(cursor.standingUnit){
      cursor.standingUnit.ReceiveDamage(12);
    }
    Unit().FinishAction();
  }

  public bool NeedsLineOfSight(){
    return(false);
  }
}
