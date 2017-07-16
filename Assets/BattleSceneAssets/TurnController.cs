using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurnController : NetworkBehaviour {

  public static TurnController instance;

  void Awake(){
    instance = this;
  }

  public void AdvanceTurnIndex()
  {
    print("advancing...");
    foreach(Unit unit in Unit.All()){
      if(unit.turnIndex == 0){
        unit.turnIndex = Unit.All().Count - 1;
      }else{
        unit.turnIndex--;
      }
    }
  }

  public void AdvanceTpToNext(){
    AdvanceTurnIndex();
    SetCurrentUnit();
  }

  public void SetCurrentUnit(){
    List<GameObject> units = new List<GameObject>();
    foreach(Transform unitObject in GameObject.Find("Units").transform){
      units.Add(unitObject.gameObject);
    }
    units.Sort((a, b) => b.GetComponent<Unit>().turnIndex.CompareTo(a.GetComponent<Unit>().turnIndex));
    print("setting current to..." + units[0].GetComponent<Unit>().unitName);
    Unit unit = units[0].GetComponent<Unit>();
    unit.SetCurrent();
    MainCamera.GoImmediately(unit.transform.position);

    unit.RecoverTurnMp();
    if(unit.currentMp > unit.maxMp){
      unit.currentMp = unit.maxMp;
    }

    foreach(Buff buff in unit.Buffs()){
      buff.OnTurnStart();
    };
  }

  public static void Next() {
    instance.Nextish();
  }

  private void Nextish() {
    if(GameController.gameFinished){
      GameController.EndGame();
    }else{
      if(Unit.current.DoneWithTurn()){
        if(Random.value <= 0.25f && GameController.treasureCount < 3){
          StartCoroutine(SpawnTreasure());
        }else{
          ContinueNextTurn();
        }
      }
    }
  }

  private IEnumerator SpawnTreasure(){
    print("spawning treasure");
    GameController.treasureCount++;
    int x = Random.Range(0, 19);
    int z = Random.Range(0, 19);

    while(!Helpers.GetTile(x, z).CanHaveTreasure()){
      x = Random.Range(0, 19);
      z = Random.Range(0, 19);
    }

    int y = VoxelController.GetElevation(x, z);

    GameObject treasure = Instantiate(Resources.Load("Treasure"), Vector3.zero, Quaternion.identity) as GameObject;
    Cursor cursor = Helpers.GetTile(x, z);
    cursor.standingTreasure = treasure.GetComponent<Treasure>();
    print(treasure.GetComponent<Treasure>());
    treasure.GetComponent<Treasure>().cursor = cursor;
    
    Helpers.SetTransformPosition(treasure.transform, x, y, z);
    MainCamera.Lock();
    MainCamera.CenterOnWorldPoint(treasure.transform.position);
    yield return new WaitForSeconds(3f);
    MainCamera.Unlock();
    instance.ContinueNextTurn();
  }

  private void ContinueNextTurn()
  {
    Unit.current.ReadyNextTurn();
    CursorController.moveEnabled = true;
    instance.AdvanceTpToNext();
  }
}