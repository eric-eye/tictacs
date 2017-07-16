using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class Player : NetworkBehaviour {
  

  public class Turn
  {
    public List<System.Action> actions;
    public int playerIndex;
    public bool finished;
  }

  public static List<Turn> turns = new List<Turn>();

  public static Player player;
  public static List<Player> players = new List<Player>();

  [SyncVar] public bool readyToBattle = false;
  [SyncVar] public string unitActions;
  [SyncVar] public string unitStances;
  [SyncVar] public string unitNames;
  [SyncVar] public string unitClasses;

  public bool initialized = false;

  [SyncVar(hook = "OnPlayerIndexChanged")]
  public int playerIndex;

  public GameObject gameControllerPrefab;

  public static Player Get(int findIndex)
  {
    return players.Find((x) => x.playerIndex == findIndex);
  }

  void Awake(){
    DontDestroyOnLoad(gameObject);
  }
  
	public void SetUnitParams(string unitActions, string unitStances, string unitNames, string unitClasses)
	{
	  CmdSetUnitParams(unitActions, unitStances, unitNames, unitClasses);
	}

  [Command]
  public void CmdSetUnitParams(string unitActions, string unitStances, string unitNames, string unitClasses)
  {
		this.unitActions = unitActions;
		this.unitStances = unitStances;
		this.unitNames = unitNames;
		this.unitClasses = unitClasses;
		this.readyToBattle = true;
  }

  public bool TotallyReadyForBattle()
  {
    return(Player.players.Where((x) => !x.readyToBattle).Count() == 0);
  }

  public static Turn CurrentTurn(){
    return(turns[turns.Count - 1]);
  }

  public static Turn OldestTurn(){
    if(turns.Count > 0){
      return(turns[0]);
    }else{
      return null;
    }
  }

	// Use this for initialization
	void Start () {
	  print("should fire first");
    if(isLocalPlayer) player = this;
    if(NetworkServer.active) {
      playerIndex = players.Count;
    }
    players.Add(this);
	}

  public void BeginBattle(){
    if(isServer && isLocalPlayer){
      CmdSpawnGameController();
    }
  }

  void OnPlayerIndexChanged(int newPlayerIndex){
    playerIndex = newPlayerIndex;
    GameController.canLaunch = true;
    initialized = true;
  }

  public void SetPath(int x, int z)
  {
      List<int[]> path = Helpers.DeriveShortestPath(x, z, Unit.current.xPos, Unit.current.zPos, Unit.current);
      CursorController.moveEnabled = false;
      CursorController.Coordinate[] coordinates = new CursorController.Coordinate[path.Count];
      int c = 0;
      foreach (int[] array in path)
      {
          CursorController.Coordinate coordinate = new CursorController.Coordinate();
          coordinate.x = array[0];
          coordinate.z = array[1];
          coordinate.counter = array[2];
          coordinate.elevation = array[3];
          coordinates[c] = coordinate;
          c++;
      }
      Unit.current.SetPath(coordinates);
      CmdSetPathOnServer(coordinates, Player.player.playerIndex, Unit.current.stanceIndex);
  }

  [Command]
  private void CmdSetPathOnServer(CursorController.Coordinate[] path, int playerIndex, int stanceIndex){
    RpcSetPathOnClient(path, playerIndex, stanceIndex);
  }

  [ClientRpc]
  private void RpcSetPathOnClient(CursorController.Coordinate[] path, int playerIndex, int stanceIndex){
    if(playerIndex != Player.player.playerIndex) {
      AddTurn();
      CurrentTurn().actions.Add(() => {
          Unit.current.SetStance(stanceIndex);
          Unit.current.SetPath(path);
      });
      CurrentTurn().finished = CurrentTurn().actions.Count > 1;
    }
  }

  private void AddTurn(){
    if(turns.Count == 0 || turns[turns.Count - 1].finished){
      Player.Turn turn = new Player.Turn();
      print(Unit.current);
      turn.playerIndex = Unit.current.playerIndex;
      turn.finished = false;
      turn.actions = new List<System.Action>();
      Player.turns.Add(turn);
    }
  }

  public void EndTurn(){
    Unit.current.EndTurn();
    CmdEndTurnOnServer(Player.player.playerIndex);
  }

  [Command]
  private void CmdEndTurnOnServer(int playerIndex){
    RpcEndTurnOnClient(playerIndex);
  }

  [ClientRpc]
  private void RpcEndTurnOnClient(int playerIndex){
    if(playerIndex != Player.player.playerIndex) {
      AddTurn();
      CurrentTurn().actions.Add(() => {
          Unit.current.EndTurn();
      });
      CurrentTurn().finished = true;
    }
  }
	
	// Update is called once per frame
	void Update () {
    if(!isLocalPlayer){
      return;
    }

    if(InputController.InputConfirm()){
      if (CursorController.moveEnabled) {
        if (GameController.state == GameController.State.PickAction && Cursor.hovered){
          if(CursorController.selected && CursorController.selected == Cursor.hovered){
            SetPath(CursorController.selected.xPos, CursorController.selected.zPos);
          }else if(!Cursor.hovered.standingUnit && Cursor.hovered.movable){
            if(ModelController.inModelMode){
              return;
            }

            CursorController.instance.ShowPath();
          }
        }
      }

      if((GameController.state == GameController.State.PickTarget) && Cursor.hovered && Cursor.hovered.attack){
        GameController.ConfirmAction(Cursor.hovered);
      }else if((GameController.state == GameController.State.ConfirmTarget) && Cursor.hovered && !Cursor.hovered.attackConfirm && Cursor.hovered.attack){
        GameController.ConfirmAction(Cursor.hovered);
      }else if((GameController.state == GameController.State.ConfirmTarget) && Cursor.hovered && Cursor.hovered.attackConfirm){
        if(ModelController.inModelMode){
          return;
        }

        GameController.FreezeInputs();
        DoAction(Cursor.hovered.xPos, Cursor.hovered.zPos, GameController.selectedActionIndex);
      }
    }

    if(InputController.InputCancel()){
      if (GameController.state == GameController.State.PickAction && CursorController.selected){
        CursorController.ResetPath();
      }
      if (GameController.state == GameController.State.PickTarget){
        GameController.CancelAttack();
      }
    }
	}

  public void PickStance(Unit unit, int stanceIndex){
    if(ModelController.inModelMode){
      unit.GetComponent<ModelBehavior>().SetStance(stanceIndex);
    }else{
      unit.SetStance(stanceIndex);
    }
    GameController.FinishStanceChange();
  }

  public void DoAction(int x, int z, int actionIndex){
    CursorController.UnsetMovement();
    CmdDoAction(x, z, actionIndex, Player.player.playerIndex, Unit.current.stanceIndex);
    Unit.current.DoAction(x, z, actionIndex);
  }

  [Command]
  public void CmdDoAction(int x, int z, int actionIndex, int playerIndex, int stanceIndex){
    RpcDoAction(x, z, actionIndex, playerIndex, stanceIndex);
  }

  [ClientRpc]
  public void RpcDoAction(int x, int z, int actionIndex, int playerIndex, int stanceIndex){
    if(playerIndex != Player.player.playerIndex) {
      AddTurn();
      CurrentTurn().actions.Add(() => {
        Unit.current.SetStance(stanceIndex);
        Unit.current.DoAction(x, z, actionIndex);
      });
      CurrentTurn().finished = CurrentTurn().actions.Count > 1;
    };
  }

  [Command]
  void CmdSpawnGameController(){
    GameObject gameController = Instantiate(gameControllerPrefab, Vector3.zero, Quaternion.identity);
    NetworkServer.Spawn(gameController);
  }

  public List<Unit> Units(){
    return Unit.All().Where(unit => unit.playerIndex == playerIndex).ToList();
  }

  public int CurrentPoints(){
    return Units().Aggregate(0, (acc, x) => acc + x.Points());
  }

  public static Player ByIndex(int index){
    return(players.Find(player => player.playerIndex == index));
  }
}
