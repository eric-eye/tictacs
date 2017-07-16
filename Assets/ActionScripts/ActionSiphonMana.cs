using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionSiphonMana : UnitAction {

  int hpAmount = 5;
  int mpAmount = 10;

  public override string Name(){
    return("Siphon Mana");
  }

  public override string Description(){
    return("Bonk your target, siphoning " + mpAmount + " MP and damaging for " + hpAmount + " HP.");
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      SendDamage(hpAmount, cursor.standingUnit);
      int mp = cursor.standingUnit.DamageMp(mpAmount);
      Unit().HealMp(mp);
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
