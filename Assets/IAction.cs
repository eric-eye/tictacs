using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction {
  int TpCost();
  int MpCost();
  int MaxDistance();

  string Name();

  bool CanTargetSelf();
  bool NeedsLineOfSight();
  void RpcBeginAction(GameObject targetObject);
  void DoAction(Cursor cursor);
}
