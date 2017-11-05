using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionThrowStone : Action, IAction {
  public int TpCost(){
    return(30);
  }

  public int MpCost(){
    return(0);
  }

  public string Name(){
    return("Throw Stone");
  }

  public string Description(){
    return("Throw a stone at your target. Needs line of sight");
  }

  public int MaxDistance(){
    return(5);
  }

  public bool CanTargetSelf(){
    return(false);
  }

  public int RadialDistance(){
    return(0);
  }

  protected override void DoAction(Cursor cursor){
    CreateVisual(cursor, Unit().transform.position);
  }

  public void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      cursor.standingUnit.ReceiveDamage(5);
    }
    Unit().FinishAction();
  }

  public bool NeedsLineOfSight(){
    return(true);
  }
}
