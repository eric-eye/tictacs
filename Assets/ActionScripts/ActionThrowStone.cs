using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionThrowStone : Action {
  public override string Name(){
    return("Throw Stone");
  }

  public override string Description(){
    return("Throw a stone at your target. Needs line of sight");
  }

  public override int MaxDistance(){
    return(5);
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      cursor.standingUnit.ReceiveDamage(5, Unit());
    }
    Unit().FinishAction();
  }

  public override bool NeedsLineOfSight(){
    return(true);
  }

  public override bool HeightAssisted(){
    return(true);
  }

  protected override void DoAction(Cursor cursor){
    CreateVisual(cursor, Unit().transform.position);
  }
}
