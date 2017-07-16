using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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

  GameObject nextTurnMenu;

  private List<int> respawnList = new List<int>();

  private bool launched = false;

  private bool playingTurn = false;
  public static bool readyToReplay = false;
  private float playingTurnTimer = 0f;

  public static int treasureCount = 0;

  [SyncVar]
  public int playerCount = 0;

  private int GetRespawnPointer(){
    int pointer = respawnList[respawnIndex];
    respawnIndex++;
    return(pointer);
  }

  public void DoNextTurn(){
    playingTurn = true;
  }

  public void NextTurnLoop(Player.Turn turn){
    if(turn.actions.Count > 0){
      if(!inputsFrozen){
        System.Action nextAction = turn.actions[0];
        nextAction();
        turn.actions.Remove(nextAction);
      }
    }else{
      Player.turns.RemoveAt(0);
      playingTurn = false;
      readyToReplay = false;
    }
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

  public static void EndTurn(){
    Player.player.EndTurn();
  }

  void Awake()
  {
    instance = this;
  }

  void Start()
  {
    if (NetworkServer.active) instance.CmdSpawnControllers();
    Instantiate(instance.voxelControllerPrefab, Vector3.zero, Quaternion.identity);
    Instantiate(instance.turnControllerPrefab, Vector3.zero, Quaternion.identity);
    Instantiate(instance.cursorControllerPrefab, Vector3.zero, Quaternion.identity);
    instance.GenerateRespawnList();

    nextTurnMenu = GameObject.Find("NextTurnMenu");
    nextTurnMenu.transform.Find("Panel").Find("NextTurn").GetComponent<Button>().onClick.AddListener(
      () => DoNextTurn()
    );
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

    if(playingTurn){
      playingTurnTimer += Time.deltaTime;

      if(playingTurnTimer > 2){
        NextTurnLoop(Player.OldestTurn());
        playingTurnTimer = 0;
      }
    }

    if(Unit.current){
      nextTurnMenu.transform.Find("Panel").Find("PlayerInfo").GetComponent<Text>().text = "Currently Player " + (Unit.current.playerIndex + 1) + "'s turn";
      nextTurnMenu.transform.Find("Panel").Find("NextTurn").gameObject.SetActive(!playingTurn && Player.OldestTurn() != null && Player.OldestTurn().finished);
      nextTurnMenu.GetComponent<Canvas>().enabled = Unit.current.playerIndex != Player.player.playerIndex;
    }else{
      nextTurnMenu.GetComponent<Canvas>().enabled = false;
    }
  }

  public void ResolveDeathPhase()
  {
    if (Unit.current.dead)
    {
      if (Unit.current.turnsDead < Unit.current.MaxTurnsDead())
      {
        StartCoroutine(GameController.SkipTurn(5));
        Unit.current.turnsDead++;
      }
      else
      {
        Unit.current.turnsDead = 0;
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
    // Instantiate(instance.voxelControllerPrefab, Vector3.zero, Quaternion.identity);
    GameObject setupPrefab = Instantiate(instance.setupControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(setupPrefab);
  }

  private void GenerateRespawnList(){
      for(int i = 0; i < 100; i++){
        respawnList.Add(Random.Range(0, 8));
      }
  }

  public static void EndGame()
  {
    gameFinished = true;
    WinInformation.Show();
  }

  public static void PickAction(Unit unit, int actionIndex)
  {
    CursorController.Cancel();
    SetState(State.PickTarget);
    selectedActionIndex = actionIndex;
    CursorController.ShowActionCursors(unit, actionIndex);
  }

  public static void ConfirmAction(Cursor cursor)
  {
    UnitAction action = Unit.Subject().Actions()[selectedActionIndex].GetComponent<UnitAction>();
    if(action.actionType() == UnitAction.ActionType.Ranged){
      foreach(Buff buff in Unit.Subject().Buffs()){
        if(!buff.CanUseRanged()){
          Alert.Show("Cannot use ranged abilities");
          return;
        }
      }
    }
    if(action.actionType() == UnitAction.ActionType.Support){
      foreach(Buff buff in Unit.Subject().Buffs()){
        if(!buff.CanUseSupport()){
          Alert.Show("Cannot use support abilities");
          return;
        }
      }
    }
    if(action.actionType() == UnitAction.ActionType.Magic){
      foreach(Buff buff in Unit.Subject().Buffs()){
        if(!buff.CanUseMagic()){
          Alert.Show("Cannot use magic abilities");
          return;
        }
      }
    }
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

  public static void ShowEndTurnMenu(){
    EndTurnMenu.Show();
  }

  public static void FinishAction()
  {
    // CursorController.ShowMoveCells();
    CursorController.ResetPath();
    CursorController.HideAttackCursors();
    CursorController.HideConfirmAttackCursors();
    instance.Whatever();
  }

  private void Whatever(){
    StartCoroutine(PostFinishAction());
  }

  private IEnumerator PostFinishAction(){
    yield return new WaitForSeconds(3f);
    // Menu.Refresh();
    SetState(State.PickAction);
    RefreshPlayerView();
    TurnController.Next();
  }

  public static void RefreshPlayerView()
  {
    CursorController.HideAttackCursors();
    CursorController.HideConfirmAttackCursors();
    CursorController.ShowMoveCells();
    CursorController.RefreshAlarmCursors();
    SetState(State.PickAction);
    Menu.Refresh();
    ActionInformation.Hide();
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
      if (!NetworkServer.active) TurnController.instance.SetCurrentUnit();
      RefreshPlayerView();
      launched = true;
    }
  }
}
