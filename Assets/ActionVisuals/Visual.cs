using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visual : MonoBehaviour {

  public Cursor cursor;
  public IAction action;

	// Use this for initialization
	void Start () {
    transform.LookAt(Camera.main.transform.position);	
	}

  public void DoAction(){
    action.ReceiveVisualFeedback(cursor);
  }

  public void Finish(){
    Destroy(gameObject);
  }
}
