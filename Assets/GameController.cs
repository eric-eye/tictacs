using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour {
  public GameObject unitPrefab;
  public GameObject playerPrefab;
  public GameObject voxelControllerPrefab;
  public GameObject cursorControllerPrefab;
  public GameObject turnControllerPrefab;

  private List<Unit> units = new List<Unit>();
  public static List<Player> players = new List<Player>();

  public static bool inputsFrozen = false;
  public enum State { PickAction, PickTarget };
  public static State state = State.PickAction;
  public static int selectedActionIndex;
  public static GameController instance;

  private bool unitsAdded = false;
  private bool tpInitialized = false;
  private bool launched = false;
  public static bool canLaunch = false;

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

  public void DoSetupSteps(){
    switch (setupIndex)
    {
      case 0:
        instance.CmdAddUnits();
        break;
      case 1:
        TurnController.instance.CmdAdvanceTp();
        break;
    }
    setupIndex++;
  }

  void Update(){
    if(NetworkServer.active) DoSetupSteps();

    if(canLaunch && !launched) Launch();

    if(InputController.InputCancel()){
      CursorController.Cancel();
    }
  }

  public void Launch(){
    if(Units().Count > 0){
      SetCurrentUnit();
      CursorController.ShowMoveCells();
      Menu.Show();
      launched = true;
    }
  }

  public static List<Unit> Units(){
    List<Unit> units = new List<Unit>();

    foreach(Transform child in GameObject.Find("Units").transform){
      units.Add(child.GetComponent<Unit>());
    }

    return(units);
  }

  public static void RemoveUnit(Unit unit) {
    //instance.units.Remove(unit);
  }

  public static void SetCurrentUnit(){
    SetState(State.PickAction);
    List<Unit> units = Units();
    units.Sort((a, b) => a.TpDiff().CompareTo(b.TpDiff()));
    Unit unit = units[0];
    Unit.SetCurrent(unit);
    if(unit.playerIndex == Player.player.playerIndex){
      MenuCamera.Show();
    }else{
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
  }

  [Command]
  void CmdAddUnits(){
    units.Add(instance.AddUnit(0, 0, Color.magenta, 0));
    units.Add(instance.AddUnit(1, 3, Color.blue, 1));
  }

  private Unit AddUnit(int xPos, int zPos, Color color, int playerIndex){
    GameObject unitObject = Instantiate(unitPrefab, Vector3.zero, Quaternion.identity);

    NetworkServer.Spawn(unitObject);

    Unit unit = unitObject.GetComponent<Unit>();
    unit.xPos = xPos;
    unit.zPos = zPos;
    unit.playerIndex = playerIndex;
    unit.CmdSetColor(color);
    unit.CmdSetTp(Random.Range(50, 100));
    CursorController.cursorMatrix[xPos][zPos].standingUnit = unit;
    return unit;
  }

  private static void SetState(State newState){
    state = newState;
  }

  [Command]
  public void CmdDoAction(int x, int z, int actionIndex){
    Unit.current.CmdDoAction(x, z, actionIndex);
  }

  [ClientRpc]
  public void RpcDoActionResponse(){
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
  public void CmdPickStance(int stanceIndex, GameObject player){
    Unit.current.CmdSetStance(stanceIndex, player);
  }

  public static void PostStanceChange() {
    CursorController.Cancel();
    CursorController.UnsetMovement();
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
