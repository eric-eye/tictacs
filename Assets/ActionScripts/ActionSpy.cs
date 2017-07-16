using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionSpy : UnitAction {
  public override string Name(){
    return("Spy");
  }

  public override string Description(){
    return("Reverals current stance of target.");
  }

  public override int MaxDistance(){
    return(6);
  }

  public override int MpCost(){
      return(5);
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      cursor.standingUnit.RevealCurrentStance();
    }
    Unit().FinishAction();
  }

  public override bool NeedsLineOfSight(){
    return(true);
  }

  public override bool HeightAssisted(){
    return(true);
  }

  public override ActionType actionType() {
    return(ActionType.Ranged);
  }
}
