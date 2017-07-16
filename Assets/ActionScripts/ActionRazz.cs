using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionRazz : UnitAction {
  public GameObject effectPrefab;

  public override string Name(){
    return("Razz");
  }
  
  public override string Description(){
    return("Annoy your target. Reduces attack for three turns.");
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
    return(ActionType.Magic);
  }
}
