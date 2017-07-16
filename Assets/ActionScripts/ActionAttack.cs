using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionAttack : UnitAction
{
  int damage = 20;

  public override string Name()
  {
    return("Attack");
  }

  public override string Description(){
    return("A basic, but powerful attack. Hits for " + damage + ".");
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      SendDamage(20, cursor.standingUnit);
    }
    Unit().FinishAction();
  }

  public override ActionType actionType(){
    return(ActionType.Melee);
  }

  public override int MaxHeightDifference(){
    return(1);
  }
}
