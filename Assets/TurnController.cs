using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurnController : NetworkBehaviour {

  public static TurnController instance;

  void Awake(){
    instance = this;
  }

  [Command]
  public void CmdAdvanceTp(){
    List<Unit> sudoUnits = Unit.All();
    sudoUnits.Sort((a, b) => a.TpDiff().CompareTo(b.TpDiff()));
    int difference = sudoUnits[0].TpDiff();
    foreach(Unit unit in sudoUnits){
      unit.CmdAddTp(difference);
    }
  }

  [Command]
  public void CmdAdvanceTpToNext(){
    CmdAdvanceTp();
    CmdSetCurrentUnit();
  }

  [Command]
  public void CmdSetCurrentUnit(){
    List<Unit> units = Unit.All();
    units.Sort((a, b) => a.TpDiff().CompareTo(b.TpDiff()));
    Unit unit = units[0];
    if(Unit.current) Unit.current.CmdUnsetCurrent();
    unit.CmdSetCurrent();


    unit.currentMp += 2;
    if(unit.currentMp > unit.maxMp){
      unit.currentMp = unit.maxMp;
    }
  }

  public static void Next() {
    if(Unit.current.DoneWithTurn()){
      Unit.current.ReadyNextTurn();
      CursorController.moveEnabled = true;
      if(NetworkServer.active) instance.CmdAdvanceTpToNext();
    }
  }
}
