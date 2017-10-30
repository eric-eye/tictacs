using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceButton : MonoBehaviour {

  public int stanceIndex;

  public void OnMouseEnter() {
    Unit unit = Unit.current;
    IStance stance = unit.Stances()[stanceIndex].GetComponent<IStance>();
    StanceInformation.Show(stance.Name(), "0", "0", "");
  }

  public void OnMouseExit() {
    StanceInformation.Hide();
  }
}
