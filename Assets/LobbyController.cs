using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LobbyController : NetworkBehaviour {

	public static LobbyController instance;
	[SyncVar]
	public int seed;

	// Use this for initialization
	void Start () {
		instance = this;
    Random.InitState(seed);
		print("seed: " + seed);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartBattle(){
		SceneManager.LoadScene("BattleScene");
		RpcLoadBattle();
	}
	
	public void ReadyForBattle()
	{
		string unitActions = string.Join(",", UnitConfig.AllActions().ToArray());
		string unitStances = string.Join(",", UnitConfig.AllStances().ToArray());
		string unitNames = string.Join(",", UnitConfig.AllNames().ToArray());
		string unitClasses = string.Join(",", UnitConfig.AllClasses().ToArray());
		
		Player.player.SetUnitParams(unitActions, unitStances, unitNames, unitClasses);
	}

	[Command]
	public void CmdSetParams(GameObject playerObject, string unitActions, string unitStances, string unitNames)
	{
		Player player = playerObject.GetComponent<Player>();
		
		player.unitActions = unitActions;
		player.unitStances = unitStances;
		player.unitNames = unitNames;
		player.readyToBattle = true;
	}

	[ClientRpc]
	public void RpcLoadBattle(){
		SceneManager.LoadScene("BattleScene");
	}
}
