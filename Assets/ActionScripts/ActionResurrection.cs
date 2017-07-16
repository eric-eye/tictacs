using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionResurrection : UnitAction {
  public GameObject effectPrefab;

  public override string Name(){
    return("Resurrection");
  }
  
  public override string Description(){
    return("Upon death, target will revive twice as quickly as usual.");
  }

  public override int MpCost(){
    return(5);
  }

  public override int MaxDistance(){
    return(4);
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      GameObject effect = Instantiate(effectPrefab, Vector3.zero, Quaternion.identity);
      cursor.standingUnit.ReceiveBuff(effect);
    }
    Unit().FinishAction();
  }

  public override ActionType actionType(){
    return(ActionType.Support);
  }
}
