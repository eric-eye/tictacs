using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionAttack : Action, IAction {
  public int TpCost(){
    return(25);
  }

  public int MpCost(){
    return(0);
  }

  public int RadialDistance(){
    return(0);
  }

  public string Name(){
    return("Attack");
  }

  public int MaxDistance(){
    return(1);
  }
  public CursorModes CursorMode(){
    return(CursorModes.Radial);
  }

  public bool CanTargetSelf(){
    return(false);
  }

  public string Description(){
    return("A basic attack. You know, swipe your sword or whatever");
  }

  public void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      cursor.standingUnit.ReceiveDamage(15);
    }
    Unit().FinishAction();
  }

  public bool NeedsLineOfSight(){
    return(false);
  }
}
