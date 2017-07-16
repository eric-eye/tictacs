using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFire : MonoBehaviour, IAction {
  public int TpCost(){
    return(35);
  }

  public int MpCost(){
    return(5);
  }

  public string Name(){
    return("Fire");
  }

  public int MaxDistance(){
    return(5);
  }

  public bool CanTargetSelf(){
    return(true);
  }

  public void DoAction(Cursor cursor){
    if(cursor.standingUnit){
      cursor.standingUnit.ReceiveDamage(12);
    }
  }

  public bool NeedsLineOfSight(){
    return(false);
  }
}
