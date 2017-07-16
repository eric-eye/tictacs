using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visual : MonoBehaviour {

  public Cursor cursor;
  public UnitAction action;

	// Use this for initialization

  public void DoAction(){
    action.ReceiveVisualFeedback(cursor);
  }

  public void Finish(){
    Destroy(gameObject);
  }
}
