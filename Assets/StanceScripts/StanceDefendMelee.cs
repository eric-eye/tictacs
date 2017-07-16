using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceDefendMelee : Stance {
  public override float NegotiateDamage(float damage, UnitAction action){
    if (action.actionType() == UnitAction.ActionType.Melee)
    {
      return(damage / 2);
    }
    else if(action.actionType() == UnitAction.ActionType.Ranged)
    {
      return(damage * 2);
    }
    else
    {
      return(damage);
    }
  }

  public override string Name(){
    return("Defend vs. Melee");
  }

  public override string Description(){
    return("Reduces incoming melee damage by half, but ranged damage is doubled.");
  }
}
