using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionNormalShot : UnitAction {
  private int damage = 15;

  public override string Name(){
    return("Normal shot");
  }

  public override string Description(){
    return("Fire an arrow at your target's vitals. Deals " + damage + ".");
  }

  public override int MaxDistance(){
    return(6);
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      SendDamage(damage, cursor.standingUnit);
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

  public override ActionType actionType() {
    return(ActionType.Ranged);
  }
}
