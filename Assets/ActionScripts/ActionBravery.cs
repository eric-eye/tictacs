using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionBravery : UnitAction {
  public GameObject effectPrefab;

  public override string Name(){
    return("Bravery");
  }
  
  public override string Description(){
    return("Increases the defense of the target");
  }

  public override int MaxDistance(){
    return(4);
  }

  public override int MpCost(){
      return(2);
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
