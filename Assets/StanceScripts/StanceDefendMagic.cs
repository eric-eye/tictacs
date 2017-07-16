using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceDefendMagic : Stance {
  public override float NegotiateDamage(float damage, UnitAction action){
    if (action.actionType() == UnitAction.ActionType.Magic)
    {
      return(damage / 2);
    }
    else if(action.actionType() == UnitAction.ActionType.Melee)
    {
      return(damage * 2);
    }
    else
    {
      return(damage);
    }
  }

  public override string Name(){
    return("Defend vs. Magic");
  }

  public override string Description(){
    return("Reduces incoming magic damage by half, but melee damage is doubled.");
  }
}
