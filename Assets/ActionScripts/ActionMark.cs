using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionMark : UnitAction {
  public GameObject effectPrefab;

  public override string Name(){
    return("Mark");
  }

  public override string Description(){
    return("Afflict a unit with mark. Marked units can always be hit with ranged attacks.");
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

  public override ActionType actionType() {
    return(ActionType.Support);
  }
}

