using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionKill : UnitAction {
  public override int MaxDistance(){
    return(30);
  }

  public override string Name(){
    return("Kill");
  }

  public override string Description(){
    return("God-kill");
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      SendDamage(100000, cursor.standingUnit);
    }
    Unit().FinishAction();
  }

  public override ActionType actionType() {
    return(ActionType.Melee);
  }
}
