using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridFramework.Renderers.Rectangular;
using GridFramework.Grids;
using UnityEngine.Networking;
using System.Linq;

public class CursorController : NetworkBehaviour {
  public struct Coordinate
    {
      public int x;
      public int z;
      public int counter;
      public int elevation;
    };

  public GameObject cursorPrefab;

  public static Cursor selected;
  public static CursorController instance;
  public static List<List<Cursor>> cursorMatrix = new List<List<Cursor>>();
  public static bool moveEnabled = true;
  public static bool showingActionCursors = false;

  private static List<int[]> _path;
  private static RectGrid _grid;
  private static Parallelepiped _renderer;
  private static int xMin;
  private static int xMax;
  private static int zMin;
  private static int zMax;
  public static bool loaded = false;

	// Use this for initialization
	void Awake () {
    instance = this;
    _grid = GameObject.Find("Grid").GetComponent<RectGrid>();
    _renderer = _grid.gameObject.GetComponent<Parallelepiped>();
    xMin = (int)_renderer.From[0];
    xMax = (int)_renderer.To[0];
    zMin = (int)_renderer.From[2];
    zMax = (int)_renderer.To[2];
	}

  public void Load(){
    for(int x = xMin; x < xMax; x++){
      cursorMatrix.Add(new List<Cursor>());
      for(int z = zMin; z < zMax; z++){
        int elevation = VoxelController.GetElevation(x, z); 
        GameObject cursorObject = Instantiate(cursorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        cursorObject.transform.parent = GameObject.Find("Cursors").transform;
        Cursor cursor = cursorObject.GetComponent<Cursor>();
        cursor.originalColor = Color.gray;
        cursor.xPos = x;
        cursor.zPos = z;
        cursor.yPos = elevation;
        if (elevation < 0)
        {
          cursor.useable = false;
        }
        cursorMatrix[x].Add(cursor);
      }
    }

    StartCoroutine(SetLoaded());
  }

  private static IEnumerator SetLoaded(){
    yield return new WaitForSeconds(2f);
    loaded = true;
  }

	// Update is called once per frame
	public static void Cancel () {
    ActionInformation.Hide();

    if (GameController.state == GameController.State.PickAction && selected){
      ResetPath();
    }
    if (GameController.state == GameController.State.PickTarget || GameController.state == GameController.State.ConfirmTarget){
      GameController.CancelAttack();
    }
	}

  public static void ShowMoveCells(){
    UnsetMovement();
    Unit unit = Unit.Subject();

    if(unit && (ModelController.inModelMode || GameController.IsCurrentPlayer() && !unit.hasMoved && !unit.dead)){
      List<Cursor> path = Helpers.GetRadialTiles(unit.xPos, unit.zPos, unit.MoveLength(), unit.JumpHeight(), false, 0, false, true);
      HighlightMovableTiles(path);
    }
  }

  public static void RefreshAlarmCursors(){
    if(Unit.current){
      for(int x = xMin; x < xMax; x++){
        for(int z = zMin; z < zMax; z++){
          Cursor tile = Helpers.GetTile(x, z);
          tile.UnsetAlarm();
        }
      }

      List<Cursor> fullPath = new List<Cursor>();

      foreach(Unit unit in Unit.All().Where((u) => u.playerIndex != Unit.current.playerIndex)){
        List<Cursor> path = Helpers.GetAlarmTiles(unit.xPos, unit.zPos, unit.lookDirection);
        fullPath = fullPath.Concat(path).ToList();
      }
      HighlightAlarmTiles(fullPath);
    }
  }

  public void ShowPath(){
    ActionInformation.Show("Movement", "0", "You know, lets you move");
    Menu.Hide();
    selected = Cursor.hovered;
    _path = Helpers.DeriveShortestPath(selected.xPos, selected.zPos, Unit.current.xPos, Unit.current.zPos, Unit.current);
    HighlightTiles(_path);
  }

  public static void ResetPath(){
    Menu.Refresh();
    selected = null;
    foreach(List<Cursor> list in cursorMatrix){
      foreach(Cursor tile in list){
        tile.UnsetPath();
      }
    }
  }

  public static void HideAttackCursors(){
    showingActionCursors = false;
    for(int x = xMin; x < xMax; x++){
      for(int z = zMin; z < zMax; z++){
        Cursor tile = Helpers.GetTile(x, z);
        tile.UnsetAttack();
      }
    }
  }

  public static void HideConfirmAttackCursors(){
    for(int x = xMin; x < xMax; x++){
      for(int z = zMin; z < zMax; z++){
        Cursor tile = Helpers.GetTile(x, z);
        tile.UnsetAttackConfirm();
        tile.UnsetAttackInRange();
      }
    }
  }

  public static void ShowActionCursors(Unit unit, int actionIndex){
    showingActionCursors = true;
    UnitAction action = unit.Actions()[actionIndex].GetComponent<UnitAction>();

    ActionInformation.Show(action.Name(), action.MpCost().ToString(), action.actionType().ToString() + " -- " + action.Description());

    Unit.Coordinate projectedCoordinate = unit.ProjectedCoordinate();

    int xPos = projectedCoordinate.xPos;
    int zPos = projectedCoordinate.zPos;

    List<Cursor> tiles = Helpers.GetRadialTiles(xPos, zPos, action.MaxDistance(), action.MaxHeightDifference(), action.CanTargetOthers(), action.MinDistance(), action.HeightAssisted());

    foreach(Cursor tile in tiles){
      if (IsValidTarget(Unit.Subject(), action, tile, xPos, zPos)) tile.SetAttack();
    }
  }

  public static void ShowActionRangeCursors(Cursor cursor, int actionIndex){
      Unit unit = Unit.Subject();

      Unit.Coordinate projectedCoordinate = unit.ProjectedCoordinate();

      int unitxPos = projectedCoordinate.xPos;
      int unitzPos = projectedCoordinate.zPos;

      UnitAction action = unit.Actions()[actionIndex].GetComponent<UnitAction>();
      List<Cursor> tiles = new List<Cursor>();

      int xPos = cursor.xPos;
      int zPos = cursor.zPos;

      {
          if (action.CursorMode() == UnitAction.CursorModes.Radial)
          {
              if (action.RadialDistance() > 0)
              {
                  tiles = Helpers.GetRadialTiles(xPos, zPos, action.RadialDistance(), 1, true);
              }
          }
          else
          {
              tiles = Helpers.GetLineTiles(unitxPos, unitzPos, xPos, zPos, action.LineDistance());
          }
      }

      foreach (Cursor tile in tiles)
      {
          if (IsValidTarget(Unit.Subject(), action, tile, xPos, zPos)) tile.SetAttackInRange();
      }
  }

  private static bool IsValidTarget(Unit requestor, UnitAction action, Cursor tile, int xPos, int zPos){
    bool valid = true;

    if(tile.standingUnit && !action.CanTargetOwnTeam()) {
      valid = tile.standingUnit.playerIndex != action.Unit().playerIndex;
    }

    if(valid) {
      if(action.CanTargetSelf() || tile.xPos != xPos || tile.zPos != zPos) {
        if(action.NeedsLineOfSight()){
          valid = Helpers.CanHitTarget(requestor.ProjectedHittable().position, tile);
        }
      }else{
        valid = false;
      }
    }

    Unit unit = Unit.Subject();

    if(valid && unit != null) {
      foreach(Buff buff in unit.Buffs()) {
        valid = buff.CanTarget(tile);
        if(!valid){
          break;
        }
      };
    }

    return(valid);
  }

  public static void ShowConfirmActionCursors(Cursor tile){
    tile.SetAttackConfirm();
  }

  private static void HighlightTiles(List<int[]> tileCoordinates) {
    foreach(List<Cursor> list in cursorMatrix){
      foreach(Cursor tile in list){
        tile.UnsetPath();
      }
    }
    for(int i = 0; i < tileCoordinates.Count; i++){
      Cursor tile = Helpers.GetTile(tileCoordinates[i][0], tileCoordinates[i][1]);
      if(tile) tile.SetPath();
    }
  }

  public static void HighlightAlarmTiles(List<Cursor> tiles) {
    foreach(List<Cursor> list in cursorMatrix){
      foreach(Cursor tile in list){
        tile.UnsetAlarm();
      }
    }
    foreach(Cursor tile in tiles){
      tile.SetAlarm();
    }
  }

  public static void HighlightMovableTiles(List<Cursor> tiles) {
    foreach(List<Cursor> list in cursorMatrix){
      foreach(Cursor tile in list){
        tile.UnsetMovement();
      }
    }
    foreach(Cursor tile in tiles){
      if(CanMoveThere(tile)){
        tile.SetMovement();
      }
    }
  }

  private static bool CanMoveThere(Cursor tile) {
    bool canMoveThere = true;

    Unit unit = Unit.Subject();

    foreach(Buff buff in unit.Buffs()) {
      canMoveThere = buff.CanMoveTo(tile);
      if(!canMoveThere) {
        break;
      }
    };

    return canMoveThere;
  }

  public static void UnsetMovement() {
    foreach(List<Cursor> list in cursorMatrix){
      foreach(Cursor tile in list){
        tile.UnsetMovement();
      }
    }
  }
}
