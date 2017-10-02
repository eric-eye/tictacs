using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

  public static Player player;

  [SyncVar(hook = "OnPlayerIndexChanged")]
  public int playerIndex;

  public GameObject gameControllerPrefab;

	// Use this for initialization
	void Start () {
    if(isServer && isLocalPlayer){
      CmdSpawnGameController();
    }
    if(isLocalPlayer){
      player = this;
      CmdSetPlayerIndex(GameController.instance.playerCount);
      GameController.instance.CmdBumpPlayerCount();
    }
	}

  void OnPlayerIndexChanged(int newPlayerIndex){
    playerIndex = newPlayerIndex;
    GameController.canLaunch = true;
  }

  [Command]
  void CmdSetPlayerIndex(int newIndex){
    playerIndex = newIndex;
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
}
