using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionDelayAttack : Action, IAction {
  public int TpCost(){
    return(35);
  }

  public int MpCost(){
    return(0);
  }

  public string Name(){
    return("Delay Attack");
  }
  
  public string Description(){
    return("Attack target, reducing their TP by " + TpCost());
  }

  public int MaxDistance(){
    return(1);
  }

  public bool CanTargetSelf(){
    return(false);
  }

  public void DoAction(Cursor cursor){
    StartCoroutine(DoScript(cursor));
  }

  public bool NeedsLineOfSight(){
    return(false);
  }

  private IEnumerator DoScript(Cursor cursor){
    if(cursor.standingUnit){
      cursor.standingUnit.ReceiveDamage(5);
    }
    yield return new WaitForSeconds(0.5f);
    if(cursor.standingUnit){
      cursor.standingUnit.ReceiveTpDamage(30);
    }
    Unit().FinishAction();
  }
}
