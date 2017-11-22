using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupController : NetworkBehaviour {
  public GameObject unitPrefab;

  private int setupIndex = 0;

	// Use this for initialization
	void Start () {
    WinInformation.Hide();
	}
	
	// Update is called once per frame
	void Update () {
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
    CmdAddUnit(8, 2, Color.red, 0, "Ash", 70);
    CmdAddUnit(10, 1, Color.red, 0, "Ness", 30);
    CmdAddUnit(17, 2, Color.red, 0, "Charizard", 50);
    CmdAddUnit(4, 17, Color.blue, 1, "Blue", 80);
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
    RpcAddUnit(unitObject, initialTp);
  }

  [ClientRpc]
  private void RpcAddUnit(GameObject unitObject, int tp) {
    unitObject.GetComponent<Unit>().currentTp = tp;
  }
}
