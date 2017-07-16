using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionBounty: UnitAction {
  public GameObject effectPrefab;

  public override string Name(){
    return("Bounty");
  }

  public override string Description(){
    return("The target will earn 20 points each turn. Lasts 3 turns. Only opponents may be marked.");
  }

  public override int MaxDistance(){
    return(6);
  }

  public override int MpCost(){
      return(2);
  }

  public override bool CanTargetOwnTeam(){
    return(false);
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
    CreateVisual(cursor, cursor.transform.position);
  }

  public override ActionType actionType() {
    return(ActionType.Ranged);
  }
}
