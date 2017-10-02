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
  private int setupIndex = 0;

  [SyncVar]
  public int playerCount = 0;

  void Awake(){
    instance = this;
  }

  void Start(){
    if(NetworkServer.active) {
      instance.CmdSpawnControllers();
    }
  }

  [Command]
  public void CmdBumpPlayerCount(){
    playerCount++;
  }

  public static void StartMoving(Unit unit){
    GameController.FreezeInputs();
    Menu.Hide();
  }

  public static void FinishMoving(){
    CursorController.ShowMoveCells();
    CursorController.ResetPath();
    TurnController.Next();
    UnfreezeInputs();
    Menu.Show();
  }

  void Update(){
    if(canLaunch && !launched && Unit.current) Launch();

    if(InputController.InputCancel()){
      CursorController.Cancel();
    }
  }

  public void Launch(){
    if(Unit.All().Count > 0){
      SetStateForPlayer();
      launched = true;
    }
  }

  public static void RemoveUnit(Unit unit) {
    //instance.units.Remove(unit);
  }

  public static void SetStateForPlayer(){
    CursorController.ShowMoveCells();

    if(Unit.current && Unit.current.playerIndex == Player.player.playerIndex){
      SetState(State.PickAction);
      MenuCamera.Show();
      Menu.Show();
      print("showing MenuCamera");
    }else{
      print("hiding MenuCamera");
      MenuCamera.Hide();
    }
  }

  [Command]
  void CmdSpawnControllers(){
    GameObject voxelPrefab = Instantiate(instance.voxelControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(voxelPrefab);

    GameObject turnPrefab = Instantiate(instance.turnControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(turnPrefab);

    GameObject cursorPrefab = Instantiate(instance.cursorControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(cursorPrefab);

    GameObject setupPrefab = Instantiate(instance.setupControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(setupPrefab);
  }

  private static void SetState(State newState){
    state = newState;
  }

  [Command]
  public void CmdDoAction(int x, int z, int actionIndex){
    Unit.current.CmdDoAction(x, z, actionIndex);
  }

  public static void FinishAction(){
    Menu.Show();
    CursorController.HideAttackCursors();
    SetState(State.PickAction);
    TurnController.Next();
  }

  public static void PickAction(int actionIndex){
    CursorController.Cancel();
    SetState(State.PickTarget);
    selectedActionIndex = actionIndex;
    CursorController.ShowActionCursors(actionIndex);
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

  public static void PostStanceChange() {
    CursorController.Cancel();
    CursorController.ShowMoveCells();
    Menu.Hide();
    Menu.Show();
  }

  public static void CancelAttack(){
    SetState(State.PickAction);
    CursorController.HideAttackCursors();
    Menu.Show();
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
