using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour {

  public Cursor cursor;
  public IAction action;

	// Use this for initialization
	void Start () {
    transform.LookAt(Camera.main.transform.position);	
	}
	
	// Update is called once per frame
	void Update () {
    float speed = 5f;
    float step = speed * Time.deltaTime;
    Transform target = cursor.transform;
    if(cursor.standingUnit) target = cursor.standingUnit.transform;
    transform.position = Vector3.MoveTowards(transform.position, cursor.transform.position, step);
    print(transform.position);

    if(Vector3.Distance(transform.position, cursor.transform.position) < 0.005f){
      DoAction();
      Finish();
    }
	}

  public void DoAction(){
    action.DoAction(cursor);
  }

  public void Finish(){
    Destroy(gameObject);
  }
}
