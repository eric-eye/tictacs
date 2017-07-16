using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
  public static bool InputConfirm () {
    return(!GameController.inputsFrozen && Input.GetMouseButtonDown(0));
  }

  public static bool InputCancel () {
    return(!GameController.inputsFrozen && Input.GetMouseButtonDown(1));
  }
}
