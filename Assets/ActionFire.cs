using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionFire : Action, IAction {
  public GameObject firePrefab;

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

  public void BeginAction(GameObject targetObject){
    used = true;

    Cursor cursor = targetObject.GetComponent<Cursor>();
    GameObject fireObject = Instantiate(firePrefab, cursor.transform.position, Quaternion.identity);
    Fire fire = fireObject.transform.Find("Fire").GetComponent<Fire>();
    fire.action = this;
    fire.cursor = cursor;
  }

  public void DoAction(Cursor cursor){
    if(cursor.standingUnit){
      cursor.standingUnit.ReceiveDamage(12);
    }
    Unit().FinishAction();
  }

  public bool NeedsLineOfSight(){
    return(false);
  }
}
