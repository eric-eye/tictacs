using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceButton : MonoBehaviour {

  public int stanceIndex;

  public void OnMouseEnter() {
    Unit unit = Unit.current;
    Stance stance = unit.Stances()[stanceIndex].GetComponent<Stance>();
    StanceInformation.Show(stance.Name(), "0", "0", "");
  }

  public void OnMouseExit() {
    StanceInformation.Hide();
  }
}
