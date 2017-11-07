using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionDelayAttack : Action {

  public override string Name(){
    return("Delay Attack");
  }
  
  public override string Description(){
    return("Attack target, reducing their TP by " + TpCost());
  }

  public override int MaxDistance(){
    return(1);
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    StartCoroutine(DoScript(cursor));
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
