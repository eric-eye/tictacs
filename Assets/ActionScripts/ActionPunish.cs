using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionPunish : Action {

  public override string Name(){
    return("Punish");
  }

  public override string Description(){
    return("A basic attack with decent range. If the target is using a learned stance, more damage is incurred.");
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      Stance stance = cursor.standingUnit.Stance();
      // int damage = stance.used ? 20 : 5;
        cursor.standingUnit.ReceiveDamage(15);
    }
    Unit().FinishAction();
  }
}
