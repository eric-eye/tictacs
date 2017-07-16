using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionBlindShot : UnitAction {
  public GameObject effectPrefab;

  public override string Name(){
    return("Blind shot");
  }

  public override string Description(){
    return("Fire a irritant at your target's eyes, bliding them. Blind targets cannot use ranged abilities. Lasts 2 turns.");
  }

  public override int MaxDistance(){
    return(6);
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      GameObject effect = Instantiate(effectPrefab, Vector3.zero, Quaternion.identity);
      cursor.standingUnit.ReceiveBuff(effect);
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
