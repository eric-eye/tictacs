using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff {
  void Up(Unit unit);
  void Down();
  int TurnsLeft();
  void DeductTurn();

}
