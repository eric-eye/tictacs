using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionKill : Action {
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
      cursor.standingUnit.ReceiveDamage(100000, Unit());
    }
    Unit().FinishAction();
  }
}
