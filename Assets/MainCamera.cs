using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

  private static bool cameraLocked = false;
  private static Vector3 nextPosition;
  private static bool moveToNextPosition = false;
  private static Transform instanceTransform;

	// Use this for initialization
	void Start () {
    instanceTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
    float speed = 0.2f;

    if(!cameraLocked){
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
    }else{
      if(moveToNextPosition) {
        transform.position = nextPosition;
        moveToNextPosition = false;
      }
    }
	}

  public static void Lock(){
    cameraLocked = true;
  }

  public static void Unlock(){
    cameraLocked = false;
  }

  public static void CenterOnWorldPoint(Vector3 newPosition){
    moveToNextPosition = true;
    nextPosition = GetTransformedPosition(newPosition);
  }

  private static Vector3 GetTransformedPosition(Vector3 newPosition){
    float x = newPosition.x + 5;
    float z = newPosition.z - 7;
    Vector3 convertedPosition = new Vector3(x, instanceTransform.position.y, z);
    return(convertedPosition);
  }
}
