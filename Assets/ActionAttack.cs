using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionAttack : Action, IAction {
  public GameObject slashPrefab;

  public int TpCost(){
    return(25);
  }

  public int MpCost(){
    return(0);
  }

  public string Name(){
    return("Attack");
  }

  public int MaxDistance(){
    return(1);
  }

  public bool CanTargetSelf(){
    return(false);
  }

  public void BeginAction(GameObject targetObject){
    Cursor cursor = targetObject.GetComponent<Cursor>();
    GameObject slashObject = Instantiate(slashPrefab, cursor.transform.position, Quaternion.identity);
    Slash slash = slashObject.transform.Find("Slash").GetComponent<Slash>();
    slash.action = this;
    slash.cursor = cursor;
  }

  public void DoAction(Cursor cursor){
    if(cursor.standingUnit){
      cursor.standingUnit.ReceiveDamage(15);
    }
    Unit().FinishAction();
  }

  public bool NeedsLineOfSight(){
    return(false);
  }
}
