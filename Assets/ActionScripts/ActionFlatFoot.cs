using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionFlatFoot : UnitAction {
  public GameObject effectPrefab;
  int damage = 5;

  public override string Name(){
    return("Flat Foot");
  }

  public override string Description(){
    return("Target unit will not be able to jump. Lasts 2 turns. Causes " + damage + " damage.");
  }

  public override int MpCost(){
    return(2);
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      SendDamage(damage, cursor.standingUnit);
      GameObject effect = Instantiate(effectPrefab, Vector3.zero, Quaternion.identity);
      cursor.standingUnit.ReceiveBuff(effect);
    }
    Unit().FinishAction();
  }

  public override ActionType actionType(){
    return(ActionType.Melee);
  }

  public override int MaxHeightDifference()
  {
    return (1);
  }
}
