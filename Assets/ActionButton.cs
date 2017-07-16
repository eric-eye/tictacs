using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour {
  public int actionIndex;

  public void OnMouseEnter() {
    Unit unit = Unit.current;
    UnitAction unitAction = unit.Actions()[actionIndex].GetComponent<UnitAction>();
    StanceInformation.Show(unitAction.Name(), unitAction.MpCost().ToString(), unitAction.actionType().ToString() + " -- " + unitAction.Description());
  }

  public void OnMouseExit() {
    StanceInformation.Hide();
  }
}
