using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour
{
  public enum State { PickAction, PickTarget, ConfirmTarget };

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
  public static bool refreshView = false;
  public static int pointsToWin = 1000;
  public static bool gameFinished = false;
  private int respawnIndex = 0;

  [SyncVar]
  private SyncListInt respawnList = new SyncListInt();

  private bool launched = false;

  [SyncVar]
  public int playerCount = 0;

  private int GetRespawnPointer(){
    int pointer = respawnList[respawnIndex];
    respawnIndex++;
    return(pointer);
  }

  public static Cursor GetRespawnTile(){
    Cursor tile = null;
    int pointer = instance.GetRespawnPointer();
    while(tile == null){
      int[] coordinates = VoxelController.respawnMarkerList[pointer];
      Cursor candidate = Helpers.GetTile(coordinates[0], coordinates[1]);
      if(!candidate.standingUnit) tile = candidate;
      if(pointer >= 7){
          pointer = 0;
      }else{
          pointer++;
      }
    }
    return(tile);
  }

  void Awake()
  {
    instance = this;
  }

  void Start()
  {
    if (NetworkServer.active) instance.CmdSpawnControllers();
    if (NetworkServer.active) instance.CmdGenerateRespawnList();
  }

  void Update()
  {
    if (canLaunch && !launched && Unit.current) Launch();

    if (InputController.InputCancel())
    {
      CursorController.Cancel();
    }

    if (refreshView)
    {
      RefreshPlayerView();
      refreshView = false;
    }
  }

  public void ResolveDeathPhase()
  {
    if (Unit.current.dead)
    {
      Unit.current.turnsDead++;
      if (Unit.current.turnsDead <= 1)
      {
        Unit.current.currentTp -= 50;
        StartCoroutine(GameController.SkipTurn(5));
      }
      else
      {
        Unit.current.Revive();
      }
    }
  }

  [Command]
  public void CmdBumpPlayerCount()
  {
    playerCount++;
  }

  [Command]
  private void CmdSpawnControllers()
  {
    Instantiate(instance.voxelControllerPrefab, Vector3.zero, Quaternion.identity);

    GameObject turnPrefab = Instantiate(instance.turnControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(turnPrefab);

    GameObject cursorPrefab = Instantiate(instance.cursorControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(cursorPrefab);

    GameObject setupPrefab = Instantiate(instance.setupControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(setupPrefab);
  }

  [Command]
  private void CmdGenerateRespawnList(){
      for(int i = 0; i < 100; i++){
        respawnList.Add(Random.Range(0, 8));
      }
  }

  public static void EndGame()
  {
    gameFinished = true;
    WinInformation.Show();
  }

  public static void PickAction(int actionIndex)
  {
    CursorController.Cancel();
    SetState(State.PickTarget);
    selectedActionIndex = actionIndex;
    CursorController.ShowActionCursors(actionIndex);
  }

  public static void ConfirmAction(Cursor cursor)
  {
    SetState(State.ConfirmTarget);
    CursorController.HideConfirmAttackCursors();
    CursorController.ShowConfirmActionCursors(cursor);
    CursorController.ShowActionRangeCursors(cursor, selectedActionIndex);
  }

  public static void StartMoving(Unit unit)
  {
    GameController.FreezeInputs();
    Menu.Refresh();
  }

  public static void FinishMoving()
  {
    CursorController.ShowMoveCells();
    CursorController.ResetPath();
    TurnController.Next();
    UnfreezeInputs();
    Menu.Refresh();
  }

  public static IEnumerator SkipTurn(float wait = 0)
  {
    yield return new WaitForSeconds(wait);
    GameController.FinishAction();
  }

  public static void FinishAction()
  {
    Menu.Refresh();
    CursorController.HideAttackCursors();
    CursorController.HideConfirmAttackCursors();
    SetState(State.PickAction);
    TurnController.Next();
  }

  public static void RefreshPlayerView()
  {
    CursorController.ShowMoveCells();
    SetState(State.PickAction);
    Menu.Refresh();
    DeathInformation.Refresh();
    MenuCamera.Refresh();
  }

  public static bool IsCurrentPlayer()
  {
    return (Unit.current && Unit.current.playerIndex == Player.player.playerIndex);
  }

  public static bool CurrentUnitIsAlive()
  {
    return (Unit.current && !Unit.current.dead);
  }

  public static void FinishStanceChange()
  {
    CursorController.Cancel();
    CursorController.ShowMoveCells();
    Menu.Refresh();
  }

  public static void CancelAttack()
  {
    SetState(State.PickAction);
    CursorController.HideAttackCursors();
    CursorController.HideConfirmAttackCursors();
    Menu.Refresh();
  }

  public static void FreezeInputs()
  {
    inputsFrozen = true;
  }

  public static void UnfreezeInputs()
  {
    inputsFrozen = false;
  }

  private static void SetState(State newState)
  {
    state = newState;
  }

  private void Launch()
  {
    if (Unit.All().Count > 0)
    {
      if (!NetworkServer.active) TurnController.instance.AdvanceTpToNext();
      RefreshPlayerView();
      launched = true;
    }
  }
}
