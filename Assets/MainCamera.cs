using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    float speed = 0.2f;

    Vector3 newPosition = Vector3.zero;
    if(Input.GetKey(KeyCode.W)){
      newPosition = newPosition + new Vector3(-speed, 0, speed);
    }

    if(Input.GetKey(KeyCode.A)){
      newPosition = newPosition + new Vector3(-speed, 0, -speed);
    }

    if(Input.GetKey(KeyCode.S)){
      newPosition = newPosition + new Vector3(speed, 0, -speed);
    }

    if(Input.GetKey(KeyCode.D)){
      newPosition = newPosition + new Vector3(speed, 0, speed);
    }

    transform.position = transform.position + newPosition;
	}
}
