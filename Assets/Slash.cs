using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour {

  public Cursor cursor;
  public IAction action;

	// Use this for initialization
	void Start () {
    transform.LookAt(Camera.main.transform.position);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void DoAction(){
    action.DoAction(cursor);
  }

  public void Finish(){
    Destroy(gameObject);
  }
}
