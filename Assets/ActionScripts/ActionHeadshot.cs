using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionHeadshot : UnitAction {
  int damage = 15;
  int damagePerMp = 2;

  public override string Name(){
    return("Headshot");
  }

  public override string Description(){
    return("Making a precision shot at your target, doing increased damage based on caster's current MP. Does " + damage + " damage plus "+ damagePerMp +" HP per MP used. Always uses all MP.");
  }

  public override int MaxDistance(){
    return(6);
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      float mod = currentMp * damagePerMp;
      SendDamage(damage + Mathf.RoundToInt(mod), cursor.standingUnit);
    }
    Unit().FinishAction();
  }

  public override bool VariableMp(){
    return(true);
  }

  public override int MpCost(){
    return(Mathf.Clamp(Unit().currentMp, 1, Unit().currentMp));
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
