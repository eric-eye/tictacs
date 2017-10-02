using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour {
  public enum State { PickAction, PickTarget };

  public GameObject playerPrefab;
  public GameObject unitPrefab;
  public GameObject voxelControllerPrefab;
  public GameObject cursorControllerPrefab;
  public GameObject turnControllerPrefab;
  public GameObject setupControllerPrefab;

  public static State state = State.PickAction;
  public static bool inputsFrozen = false;
  public static GameController instance;
  public static bool canLaunch = false;
  public static int selectedActionIndex;

  private bool launched = false;

  [SyncVar]
  public int playerCount = 0;

  void Awake(){
    instance = this;
  }

  void Start(){
    if(NetworkServer.active) instance.CmdSpawnControllers();
  }

  void Update(){
    if(canLaunch && !launched && Unit.current) Launch();

    if(InputController.InputCancel()){
      CursorController.Cancel();
    }
  }

  [Command]
  public void CmdMoveAlong(int x, int z){
    List<int[]> path = CursorController.DeriveShortestPath(x, z, Unit.current.xPos, Unit.current.zPos);
    CursorController.moveEnabled = false;
    CursorController.Coordinate[] coordinates = new CursorController.Coordinate[path.Count];
    int c = 0;
    foreach(int[] array in path){
      CursorController.Coordinate coordinate = new CursorController.Coordinate();
      coordinate.x = array[0];
      coordinate.z = array[1];
      coordinate.counter = array[2];
      coordinate.elevation = array[3];
      coordinates[c] = coordinate;
      c++;
    }
    Unit.current.CmdSetPath(coordinates);
  }

  [Command]
  public void CmdPickStance(int stanceIndex, GameObject player){
    Unit.current.CmdSetStance(stanceIndex, player);
  }

  [Command]
  public void CmdDoAction(int x, int z, int actionIndex){
    Unit.current.CmdDoAction(x, z, actionIndex);
  }

  [Command]
  public void CmdBumpPlayerCount(){
    playerCount++;
  }

  [Command]
  private void CmdSpawnControllers(){
    GameObject voxelPrefab = Instantiate(instance.voxelControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(voxelPrefab);

    GameObject turnPrefab = Instantiate(instance.turnControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(turnPrefab);

    GameObject cursorPrefab = Instantiate(instance.cursorControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(cursorPrefab);

    GameObject setupPrefab = Instantiate(instance.setupControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(setupPrefab);
  }

  public static void PickAction(int actionIndex){
    CursorController.Cancel();
    SetState(State.PickTarget);
    selectedActionIndex = actionIndex;
    CursorController.ShowActionCursors(actionIndex);
  }

  public static void StartMoving(Unit unit){
    GameController.FreezeInputs();
    Menu.Refresh();
  }

  public static void FinishMoving(){
    CursorController.ShowMoveCells();
    CursorController.ResetPath();
    TurnController.Next();
    UnfreezeInputs();
    Menu.Refresh();
  }

  public static void FinishAction(){
    Menu.Refresh();
    CursorController.HideAttackCursors();
    SetState(State.PickAction);
    TurnController.Next();
  }

  public static void RemoveUnit(Unit unit) {
    //instance.units.Remove(unit);
  }

  public static void RefreshPlayerView(){
    CursorController.ShowMoveCells();
    SetState(State.PickAction);
    Menu.Refresh();
    MenuCamera.Refresh();
  }

  public static bool IsCurrentPlayer(){
    return(Unit.current && Unit.current.playerIndex == Player.player.playerIndex);
  }

  public static void FinishStanceChange() {
    CursorController.Cancel();
    CursorController.ShowMoveCells();
    Menu.Refresh();
  }

  public static void CancelAttack(){
    SetState(State.PickAction);
    CursorController.HideAttackCursors();
    Menu.Refresh();
  }

  private static void FreezeInputs() {
    inputsFrozen = true;
  }

  private static void UnfreezeInputs() {
    inputsFrozen = false;
  }

  private static void SetState(State newState){
    state = newState;
  }

  private void Launch(){
    if(Unit.All().Count > 0){
      RefreshPlayerView();
      launched = true;
    }
  }
}
