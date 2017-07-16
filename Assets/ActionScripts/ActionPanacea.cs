using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionPanacea : UnitAction {

  public override string Name(){
    return("Panacea");
  }
  
  public override string Description(){
    return("Removes all effects from target, otherwise restores all MP to the caster.");
  }

  public override int MaxDistance(){
    return(4);
  }

  public override int MpCost(){
      return(2);
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      if(cursor.standingUnit.Buffs().Count > 0){
        cursor.standingUnit.RemoveBuffs();
      }else{
        Unit().HealMp(10);
      }
    }
    Unit().FinishAction();
  }

  public override ActionType actionType(){
    return(ActionType.Support);
  }
}
