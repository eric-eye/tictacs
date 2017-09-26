using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupController : NetworkBehaviour {
  public GameObject unitPrefab;

  private int setupIndex = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    //if(!NetworkServer.active){
      //return;
    //}

    //switch (setupIndex)
    //{
      //case 0:
        //CmdAddUnits();
        //break;
      //case 1:
        //TurnController.instance.CmdAdvanceTp();
        //break;
    //}
    //setupIndex++;
	}

  [Command]
  void CmdAddUnits(){
    AddUnit(0, 0, Color.magenta, 0);
    AddUnit(1, 3, Color.blue, 1);
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
    print(xPos);
    print(zPos);
    print("a2");
    return unit;
  }
}
