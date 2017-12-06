using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class Player : NetworkBehaviour {

  public static Player player;
  public static List<Player> players = new List<Player>();

  public static List<System.Action> queuedActions = new List<System.Action>();

  [SyncVar(hook = "OnPlayerIndexChanged")]
  public int playerIndex;

  public GameObject gameControllerPrefab;

  void Awake(){
    DontDestroyOnLoad(gameObject);
  }

	// Use this for initialization
	void Start () {
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
  }

  public void SetPath(int x, int z)
  {
      List<int[]> path = Helpers.DeriveShortestPath(x, z, Unit.current.xPos, Unit.current.zPos);
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
      queuedActions.Add(() => {
          Unit.current.SetStance(stanceIndex);
          Unit.current.SetPath(path);
      });
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
            CursorController.instance.ShowPath();
          }
        }
      }

      if((GameController.state == GameController.State.PickTarget) && Cursor.hovered && Cursor.hovered.attack){
        GameController.ConfirmAction(Cursor.hovered);
      }else if((GameController.state == GameController.State.ConfirmTarget) && Cursor.hovered && !Cursor.hovered.attackConfirm && Cursor.hovered.attack){
        GameController.ConfirmAction(Cursor.hovered);
      }else if((GameController.state == GameController.State.ConfirmTarget) && Cursor.hovered && Cursor.hovered.attackConfirm){
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

  public void PickStance(int stanceIndex){
    Unit.current.SetStance(stanceIndex);
    GameController.FinishStanceChange();
  }

  public void DoAction(int x, int z, int actionIndex){
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
      queuedActions.Add(() => {
        Unit.current.SetStance(stanceIndex);
        Unit.current.DoAction(x, z, actionIndex);
      });
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
    return Units().Aggregate(0, (acc, x) => acc + x.points);
  }

  public static Player ByIndex(int index){
    return(players.Find(player => player.playerIndex == index));
  }
}
