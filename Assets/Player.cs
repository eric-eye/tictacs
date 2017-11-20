using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class Player : NetworkBehaviour {

  public static Player player;
  public static List<Player> players = new List<Player>();

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
	
	// Update is called once per frame
	void Update () {
    if(!isLocalPlayer){
      return;
    }

    if(InputController.InputConfirm()){
      if (CursorController.moveEnabled) {
        if (GameController.state == GameController.State.PickAction && Cursor.hovered){
          if(CursorController.selected && CursorController.selected == Cursor.hovered){
            CmdMoveAlong(CursorController.selected.xPos, CursorController.selected.zPos);
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
        CmdDoAction(Cursor.hovered.xPos, Cursor.hovered.zPos, GameController.selectedActionIndex);
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
    CmdPickStance(stanceIndex, gameObject);
  }

  [Command]
  public void CmdPickStance(int stanceIndex, GameObject player){
    GameController.instance.CmdPickStance(stanceIndex, player);
  }

  [Command]
  public void CmdDoAction(int x, int z, int actionIndex){
    GameController.instance.CmdDoAction(x, z, actionIndex);
  }

  [Command]
  public void CmdMoveAlong(int x, int z) {
    GameController.instance.CmdMoveAlong(x, z);
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
