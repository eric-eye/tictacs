using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction {
  int TpCost();
  int MpCost();
  int MaxDistance();
  int RadialDistance();

  string Name();
  string Description();

  bool CanTargetSelf();
  bool NeedsLineOfSight();
  void ReceiveVisualFeedback(Cursor cursor);
  Action.CursorModes CursorMode();
}
