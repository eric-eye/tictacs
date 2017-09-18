using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour {
  public GameObject unitPrefab;
  public GameObject playerPrefab;
  public GameObject voxelControllerPrefab;
  public GameObject cursorControllerPrefab;

  private List<Unit> units = new List<Unit>();
  public static List<Player> players = new List<Player>();

  public static int unitIndex = 0;
  public static bool inputsFrozen = false;
  public enum State { PickAction, PickTarget };
  public static State state = State.PickAction;
  public static IAction selectedAction;
  public static GameController instance;

  private bool initialized = false;

  void Start(){
    instance = this;

    if(NetworkServer.active) {
      BeginHosting();
    }
  }

  public static void RemoveUnit(Unit unit) {
    instance.units.Remove(unit);
  }

  public static void BeginHosting(){
    //order of operations
    //VoxelController
    //CursorController
    //Cursor
    //Menu
    //Gamecontroller
    //
    //GameObject voxelPrefab = Instantiate(instance.voxelControllerPrefab, Vector3.zero, Quaternion.identity);
    //NetworkServer.Spawn(voxelPrefab);
    instance.CmdStrap();
  }

  private void StrapDownstream(){
    CmdAddUnits();

    //if(false){
      //Unit.SetCurrent(AdvanceTpAndSelectUnit());
      //CursorController.ShowMoveCells();

      //Menu.Show();
    //}

    initialized = true;
  }

  [Command]
  void CmdStrap(){
    GameObject voxelPrefab = Instantiate(instance.voxelControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(voxelPrefab);

    GameObject cursorPrefab = Instantiate(instance.cursorControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(cursorPrefab);
  }

  [Command]
  void CmdAddUnits(){
    instance.units.Add(instance.AddUnit(0, 0, Color.magenta));
    instance.units.Add(instance.AddUnit(1, 3, Color.blue));
  }

  void Update () {
    if(NetworkServer.active && !initialized){
      StrapDownstream();
    }

    if(InputController.InputConfirm()){
      CursorController.Confirm();
    }

    if(InputController.InputCancel()){
      CursorController.Cancel();
    }
  }

  private Unit AddUnit(int xPos, int zPos, Color color){
    GameObject unitObject = Instantiate(unitPrefab, Vector3.zero, Quaternion.identity);

    NetworkServer.Spawn(unitObject);

    unitObject.transform.parent = GameObject.Find("Units").transform;
    Unit unit = unitObject.GetComponent<Unit>();
    unit.xPos = xPos;
    unit.zPos = zPos;
    unit.CmdSetColor(color);
    unit.currentTp = Random.Range(50, 100);
    unit.stance = unit.stances[0].GetComponent<IStance>();
    CursorController.cursorMatrix[xPos][zPos].standingUnit = unit;
    return unit;
  }

  private static void SetState(State newState){
    state = newState;
  }

  public static void DoAction(Cursor cursor){
    Unit.current.DoAction(cursor, selectedAction);
    Menu.Show();
    CursorController.HideAttackCursors();
    SetState(State.PickAction);
    Next();
  }

  public static void PickAction(IAction action){
    CursorController.Cancel();
    SetState(State.PickTarget);
    selectedAction = action;
    CursorController.ShowActionCursors(action);
  }

  public static void PickStance(IStance stance){
    Unit.current.stance = stance;

    CursorController.Cancel();
    CursorController.UnsetMovement();
    CursorController.ShowMoveCells();
  }

  public static void CancelAttack(){
    SetState(State.PickAction);
    CursorController.HideAttackCursors();
    Menu.Show();
  }

  public static Unit AdvanceTpAndSelectUnit(){
    instance.units.Sort((a, b) => a.TpDiff().CompareTo(b.TpDiff()));
    int difference = instance.units[0].TpDiff();
    foreach(Unit unit in instance.units){
      unit.currentTp = unit.currentTp + difference;
    }
    return(instance.units[0]);
  }

  public static void Next() {
    SetState(State.PickAction);

    if(Unit.current.DoneWithTurn()){
      Unit.current.ReadyNextTurn();
      CursorController.moveEnabled = true;
      Unit.SetCurrent(AdvanceTpAndSelectUnit());
      Unit.current.AdvanceBuffs();
      CursorController.ShowMoveCells();
      Menu.Show();
    }
  }

  public static void FreezeInputs() {
    inputsFrozen = true;
  }

  public static void UnfreezeInputs() {
    inputsFrozen = false;
  }

  public static void ShowProfile(Unit unit){
    Profile.Show(unit);
  }

  public static void HideProfile(){
    Profile.Hide();
  }
}
