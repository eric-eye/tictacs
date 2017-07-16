using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffRazz : MonoBehaviour, IBuff {

  public int turnsLeft;
  private Unit unit;

  public void Up(Unit targetUnit){
    unit = targetUnit;
    unit.physicalResistModifier = 0.75f;
    unit.attackModifier = 1.25f;
  }

  public void Down(){
    unit.physicalResistModifier = 1;
    unit.attackModifier = 1;
  }

  public void DeductTurn(){
    turnsLeft--;
  }

  public int TurnsLeft(){
    return(turnsLeft);
  }
}
