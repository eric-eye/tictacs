using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionHaste : UnitAction {
  public GameObject effectPrefab;
  int amount = 2;

  public override string Name(){
    return("Haste");
  }
  
  public override string Description(){
    return("Target receives " + amount + " TP.");
  }

  public override int MaxDistance(){
    return(4);
  }

  public override int MpCost(){
    return(5);
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      cursor.standingUnit.IncreaseTurnPoint(amount);
    }
    Unit().FinishAction();
  }

  public override ActionType actionType(){
    return(ActionType.Support);
  }
}
