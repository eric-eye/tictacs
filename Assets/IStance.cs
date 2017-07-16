using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStance {
  int NegotiateDamage(int damage);
  int NegotiateMoveLength(int moveLength);

  string Name();
}
