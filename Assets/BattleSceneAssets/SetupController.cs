using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class SetupController : NetworkBehaviour
{
    public GameObject unitPrefab;

    private int setupIndex = 0;

    private int unitsRegistered = 0;
    private bool turnAdvanced = false;

    // Use this for initialization
    void Start()
    {
        WinInformation.Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (!NetworkServer.active && !turnAdvanced)
        {
            AdvanceTurn();
        }

        if (!NetworkServer.active)
        {
            return;
        }

        switch (setupIndex)
        {
            case 0:
                CmdAddUnits();
                setupIndex++;
                break;
            case 1:
                if (Unit.loaded)
                {
                    TurnController.instance.SetCurrentUnit();
                    setupIndex++;
                }

                break;
        }
    }

    [Command]
    void CmdAddUnits()
    {
        Player player = Player.Get(0);
        List<string> names = player.unitNames.Split(',').ToList();

        List<int[]> c = VoxelController.respawnMarkerList;

        List<int> turnIndexList = new List<int>{3, 2, 1, 0};

        if (Player.players.Count > 1)
        {
            turnIndexList = new List<int>{7, 1, 3, 5, 0, 2, 4, 6};
        }

        CmdAddUnit(c[0][0], c[0][1], Color.red, 0, names[0], turnIndexList[0], new Vector3(1, 0, 0), 0, player.unitActions, player.unitStances, player.unitClasses);
        CmdAddUnit(c[1][0], c[1][1], Color.red, 0, names[1], turnIndexList[1], new Vector3(0, 0, -1), 1, player.unitActions, player.unitStances, player.unitClasses);
        CmdAddUnit(c[2][0], c[2][1], Color.red, 0, names[2], turnIndexList[2], new Vector3(0, 0, -1), 2, player.unitActions, player.unitStances, player.unitClasses);
        CmdAddUnit(c[3][0], c[3][1], Color.red, 0, names[3], turnIndexList[3], new Vector3(0, 0, -1), 3, player.unitActions, player.unitStances, player.unitClasses);

        if (Player.players.Count > 1)
        {
            player = Player.Get(1);
            names = player.unitNames.Split(',').ToList();
            
            CmdAddUnit(c[4][0], c[4][1], Color.blue, 1, names[0], turnIndexList[4], new Vector3(0, 0, 1), 0, player.unitActions, player.unitStances, player.unitClasses);
            CmdAddUnit(c[5][0], c[5][1], Color.blue, 1, names[1], turnIndexList[5], new Vector3(0, 0, 1), 1, player.unitActions, player.unitStances, player.unitClasses);
            CmdAddUnit(c[6][0], c[6][1], Color.blue, 1, names[2], turnIndexList[6], new Vector3(-1, 0, 0), 2, player.unitActions, player.unitStances, player.unitClasses);
            CmdAddUnit(c[7][0], c[7][1], Color.blue, 1, names[3], turnIndexList[7], new Vector3(-1, 0, 0), 3, player.unitActions, player.unitStances, player.unitClasses);
        }
    }

    private void CmdAddUnit(int xPos, int zPos, Color color, int playerIndex, string unitName, int turnIndex,
        Vector3 lookDirection, int unitIndex, string unitActions, string unitStances, string unitClasses)
    {
        GameObject unitObject = Instantiate(unitPrefab, Vector3.zero, Quaternion.identity);

        Unit unit = unitObject.GetComponent<Unit>();
        unit.xPos = xPos;
        unit.zPos = zPos;
        unit.playerIndex = playerIndex;
        unit.SetColor(color);
        unit.unitName = unitName;
        unit.turnIndex = turnIndex;
        unit.lookDirection = lookDirection;
        unit.unitIndex = unitIndex;
        NetworkServer.Spawn(unitObject);
        RpcAddUnit(unitObject, turnIndex, xPos, zPos, unitIndex, unitActions, unitStances, unitClasses, lookDirection.x, lookDirection.y, lookDirection.z);
    }

    [ClientRpc]
    private void RpcAddUnit(GameObject unitObject, int turnIndex, int xPos, int zPos, int unitIndex, string actions, string stances, string classes, float lookX, float lookY, float lookZ)
    {
        Unit unit = unitObject.GetComponent<Unit>();
        unit.turnIndex = turnIndex;
        unit.xPos = xPos;
        unit.zPos = zPos;
        unit.unitIndex = unitIndex;
        unit.lookDirection = new Vector3(lookX, lookY, lookZ);
        unit.PopulateActionsAndStances(actions, stances, classes);
        unit.SetTransformPosition();
        unitsRegistered++;
    }

    private void AdvanceTurn()
    {
        if (Unit.loaded)
        {
            TurnController.instance.SetCurrentUnit();
            turnAdvanced = true;
        }
    }
}