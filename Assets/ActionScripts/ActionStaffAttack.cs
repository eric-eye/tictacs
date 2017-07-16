using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionStaffAttack : UnitAction {
  int amount = 10;

  public override string Name(){
    return("Heal Rod");
  }

  public override string Description(){
    return("Thwack your target with a healing rod. Damages opponents, but heals friendlies for " + amount + " HP.");
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    Unit standingUnit = cursor.standingUnit;
    if(standingUnit){
      if(standingUnit.playerIndex != Unit().playerIndex){
        SendDamage(amount, standingUnit);
      }else{
        standingUnit.HealDamage(amount);
      }
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
