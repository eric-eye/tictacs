using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionBlankStare : UnitAction {
  public GameObject effectPrefab;

  public override string Name(){
    return("Blank Stare");
  }

  public override string Description(){
    return("Force target to be blanked. Blanked units cannot use magic or support abilities.");
  }

  public override int MaxDistance(){
    return(6);
  }

  public override int MpCost(){
      return(5);
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
