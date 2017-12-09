using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupController : NetworkBehaviour {
  public GameObject unitPrefab;

  private int setupIndex = 0;
  private int unitsRegistered = 0;
  private bool turnAdvanced = false;

	// Use this for initialization
	void Start () {
    WinInformation.Hide();
	}
	
	// Update is called once per frame
	void Update () {
    if(!NetworkServer.active && !turnAdvanced){
      AdvanceTurn();
    }

    if(!NetworkServer.active){
      return;
    }

    switch (setupIndex)
    {
      case 0:
        CmdAddUnits();
        break;
      case 1:
        TurnController.instance.AdvanceTpToNext();
        break;
    }
    setupIndex++;
	}

  [Command]
  void CmdAddUnits(){
    CmdAddUnit(4, 1, Color.red, 0, "Red", 90);
    CmdAddUnit(8, 2, Color.red, 0, "Ash", 80);
    CmdAddUnit(10, 1, Color.red, 0, "Ness", 30);
    CmdAddUnit(17, 2, Color.red, 0, "Charizard", 50);
    CmdAddUnit(4, 17, Color.blue, 1, "Blue", 70);
    CmdAddUnit(8, 18, Color.blue, 1, "Gary", 40);
    CmdAddUnit(10, 18, Color.blue, 1, "Porky", 60);
    CmdAddUnit(18, 17, Color.blue, 1, "Squirtle", 20);
  }

  [Command]
  private void CmdAddUnit(int xPos, int zPos, Color color, int playerIndex, string newName, int initialTp){
    GameObject unitObject = Instantiate(unitPrefab, Vector3.zero, Quaternion.identity);

    Unit unit = unitObject.GetComponent<Unit>();
    unit.xPos = xPos;
    unit.zPos = zPos;
    unit.playerIndex = playerIndex;
    unit.SetColor(color);
    unit.unitName = newName;
    unit.currentTp = initialTp;
    NetworkServer.Spawn(unitObject);
    RpcAddUnit(unitObject, initialTp, xPos, zPos);
  }

  [ClientRpc]
  private void RpcAddUnit(GameObject unitObject, int tp, int xPos, int zPos) {
    Unit unit = unitObject.GetComponent<Unit>();
    unit.currentTp = tp;
    unit.xPos = xPos;
    unit.zPos = zPos;
    unitsRegistered++;
  }

  private void AdvanceTurn(){
    if(Unit.All().Count >= 8){
      TurnController.instance.AdvanceTpToNext();
      turnAdvanced = true;
    }
  }
}
