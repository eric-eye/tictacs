using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionAttack : Action {

  public override string Name(){
    return("Attack");
  }

  public override string Description(){
    return("A basic attack. You know, swipe your sword or whatever");
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      cursor.standingUnit.ReceiveDamage(15, Unit());
    }
    Unit().FinishAction();
  }
}
