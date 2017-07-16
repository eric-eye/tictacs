using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

  private static bool cameraLocked = false;
  private static Vector3 nextPosition;
  private static bool moveToNextPosition = false;
  public static Transform instanceTransform;
  private Vector3 pivot;
  private float rotationCounter;
  private float pitchingCounter;
  private bool rotating = false;
  private static int rotationState = 0;
  private int rotationDirection = 1;
  private static bool birdsEye = false;
  private bool togglingBirdsEye = false;

  // Use this for initialization
  void Start()
  {
    instanceTransform = transform;
    GetComponent<Camera>().backgroundColor = Color.gray;
    pivot = new Vector3(10, 0, 10);
  }

  public void Reset()
  {
    GetComponent<Camera>().backgroundColor = Color.gray;
  }

  public void ModelMode()
  {
    GetComponent<Camera>().backgroundColor = Color.cyan;
  }

  private void Rotate()
  {
    float angleTarget = 90;
    rotating = true;
    if (rotationCounter >= angleTarget)
    {
      rotating = false;
      rotationCounter = 0;
      return;
    }
    float interval = 150 * Time.deltaTime;
    rotationCounter += interval;
    if(rotationCounter > angleTarget) interval -= (rotationCounter - angleTarget);
    transform.RotateAround(pivot, Vector3.up, interval * rotationDirection);
  }

  private void ToggleBirdsEye()
  {
    float angleTarget = 45;
    togglingBirdsEye = true;
    if(pitchingCounter >= angleTarget){
      pitchingCounter = 0;
      togglingBirdsEye = false;
      birdsEye = !birdsEye;
      return;
    }
    print("birdsEye: " + birdsEye);
    int direction = birdsEye ? -1 : 1;
    print("direction: " + direction);
    float interval = 150 * Time.deltaTime;
    pitchingCounter += interval;
    if(pitchingCounter > angleTarget) interval -= (pitchingCounter - angleTarget);
    transform.Rotate(new Vector3(interval * direction, 0, 0));
  }

  // Update is called once per frame
  void Update()
  {
    if (rotating)
    {
      Rotate();
      return;
    }

    if (togglingBirdsEye)
    {
      ToggleBirdsEye();
      return;
    }

    float speed = 0.2f;

    if (!cameraLocked)
    {
      Vector3 newPosition = Vector3.zero;

      if (rotationState == 0)
      {
        if (Input.GetKey(KeyCode.A))
        {
          newPosition = newPosition + new Vector3(-speed, 0, speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
          newPosition = newPosition + new Vector3(-speed, 0, -speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
          newPosition = newPosition + new Vector3(speed, 0, -speed);
        }

        if (Input.GetKey(KeyCode.W))
        {
          newPosition = newPosition + new Vector3(speed, 0, speed);
        }
      }

      if (rotationState == 1)
      {
        if (Input.GetKey(KeyCode.S))
        {
          newPosition = newPosition + new Vector3(-speed, 0, speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
          newPosition = newPosition + new Vector3(-speed, 0, -speed);
        }

        if (Input.GetKey(KeyCode.W))
        {
          newPosition = newPosition + new Vector3(speed, 0, -speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
          newPosition = newPosition + new Vector3(speed, 0, speed);
        }
      }

      if (rotationState == 2)
      {
        if (Input.GetKey(KeyCode.D))
        {
          newPosition = newPosition + new Vector3(-speed, 0, speed);
        }

        if (Input.GetKey(KeyCode.W))
        {
          newPosition = newPosition + new Vector3(-speed, 0, -speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
          newPosition = newPosition + new Vector3(speed, 0, -speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
          newPosition = newPosition + new Vector3(speed, 0, speed);
        }
      }

      if (rotationState == 3)
      {
        if (Input.GetKey(KeyCode.W))
        {
          newPosition = newPosition + new Vector3(-speed, 0, speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
          newPosition = newPosition + new Vector3(-speed, 0, -speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
          newPosition = newPosition + new Vector3(speed, 0, -speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
          newPosition = newPosition + new Vector3(speed, 0, speed);
        }
      }

      if (Input.GetKey(KeyCode.Q))
      {
        rotationDirection = 1;
        rotationState += 1;
        if (rotationState > 3) rotationState = 0;
        Rotate();
      }

      if (Input.GetKey(KeyCode.E))
      {
        rotationDirection = -1;
        rotationState -= 1;
        if (rotationState < 0) rotationState = 3;
        Rotate();
      }

      if (Input.GetKey(KeyCode.F))
      {
        ToggleBirdsEye();
      }

      transform.position = transform.position + newPosition;
    }
    else
    {
      if (moveToNextPosition)
      {
        transform.position = nextPosition;
        moveToNextPosition = false;
      }
    }
  }

  public static void Lock()
  {
    cameraLocked = true;
  }

  public static void Unlock()
  {
    cameraLocked = false;
  }

  public static void GoImmediately(Vector3 newPosition)
  {
    instanceTransform.position = GetTransformedPosition(newPosition);
  }

  public static void CenterOnWorldPoint(Vector3 newPosition)
  {
    moveToNextPosition = true;
    nextPosition = GetTransformedPosition(newPosition);
  }

  private static Vector3 GetTransformedPosition(Vector3 newPosition)
  {
    int distance = 0;
    if(!birdsEye){
      distance = 6;
    }

    Vector3 convertedPosition = Vector3.zero;
    if (rotationState == 0){
      convertedPosition = new Vector3(newPosition.x - distance, instanceTransform.position.y, newPosition.z - distance);
    }
    if (rotationState == 1){
      convertedPosition = new Vector3(newPosition.x - distance, instanceTransform.position.y, newPosition.z + distance);
    }
    if (rotationState == 2){
      convertedPosition = new Vector3(newPosition.x + distance, instanceTransform.position.y, newPosition.z + distance);
    }
    if (rotationState == 3){
      convertedPosition = new Vector3(newPosition.x + distance, instanceTransform.position.y, newPosition.z - distance);
    }
    return (convertedPosition);
  }
}
