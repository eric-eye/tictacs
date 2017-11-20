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
        TurnController.instance.CmdAdvanceTpToNext();
        break;
    }
    setupIndex++;
	}

  [Command]
  void CmdAddUnits(){
    AddUnit(4, 1, Color.red, 0, "Red", 90);
    AddUnit(8, 2, Color.red, 0, "Ash", 70);
    AddUnit(10, 1, Color.red, 0, "Ness", 30);
    AddUnit(17, 2, Color.red, 0, "Charizard", 50);
    AddUnit(4, 17, Color.blue, 1, "Blue", 80);
    AddUnit(8, 18, Color.blue, 1, "Gary", 40);
    AddUnit(10, 18, Color.blue, 1, "Porky", 60);
    AddUnit(18, 17, Color.blue, 1, "Squirtle", 20);
  }

  private void AddUnit(int xPos, int zPos, Color color, int playerIndex, string newName, int initialTp){
    GameObject unitObject = Instantiate(unitPrefab, Vector3.zero, Quaternion.identity);

    Unit unit = unitObject.GetComponent<Unit>();
    unit.xPos = xPos;
    unit.zPos = zPos;
    unit.playerIndex = playerIndex;
    unit.SetColor(color);
    unit.unitName = newName;
    unit.currentTp = initialTp;
    NetworkServer.Spawn(unitObject);
  }
}
