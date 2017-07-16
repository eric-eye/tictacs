using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualArrow : Visual {
  private Transform target;
	void Start () {
    print(cursor);
    target = cursor.transform;
    if(cursor.standingUnit) {
      target = cursor.standingUnit.transform.Find("Body").Find("Hittable");
    }
    transform.LookAt(target.transform);	
	}
	
	// Update is called once per frame
	void Update () {
    float speed = 15f;
    float step = speed * Time.deltaTime;
    transform.position = Vector3.MoveTowards(transform.position, target.position, step);

    if(Vector3.Distance(transform.position, target.position) < 0.005f){
      DoAction();
      Finish();
    }
	}
}
