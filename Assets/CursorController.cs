using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridFramework.Renderers.Rectangular;
using GridFramework.Grids;
using UnityEngine.Networking;

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

  private static List<int[]> _path;
  private static RectGrid _grid;
  private static Parallelepiped _renderer;
  private static int xMin;
  private static int xMax;
  private static int zMin;
  private static int zMax;

	// Use this for initialization
	void Start () {
    instance = this;
    _grid = GameObject.Find("Grid").GetComponent<RectGrid>();
    _renderer = _grid.gameObject.GetComponent<Parallelepiped>();
    xMin = (int)_renderer.From[0];
    xMax = (int)_renderer.To[0];
    zMin = (int)_renderer.From[2];
    zMax = (int)_renderer.To[2];

    for(int x = xMin; x < xMax; x++){
      cursorMatrix.Add(new List<Cursor>());
      for(int z = zMin; z < zMax; z++){
        GameObject cursorObject = Instantiate(cursorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        cursorObject.transform.parent = GameObject.Find("Cursors").transform;
        Cursor cursor = cursorObject.GetComponent<Cursor>();
        cursor.originalColor = Color.gray;
        cursor.xPos = x;
        cursor.zPos = z;
        cursor.yPos = VoxelController.GetElevation(x, z);
        cursorMatrix[x].Add(cursor);
      }
    }
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
    if(GameController.IsCurrentPlayer() && !Unit.current.hasMoved){
      List<Cursor> path = Helpers.GetRadialTiles(Unit.current.xPos, Unit.current.zPos, Unit.current.MoveLength(), false);
      HighlightMovableTiles(path);
    }else{
      UnsetMovement();
    }
  }

  public void ShowPath(){
    ActionInformation.Show("Movement", "0", "0", "You know, lets you move");
    Menu.Hide();
    selected = Cursor.hovered;
    _path = Helpers.DeriveShortestPath(selected.xPos, selected.zPos, Unit.current.xPos, Unit.current.zPos);
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

  public static void ShowActionCursors(int actionIndex){
    Action action = Unit.current.Actions()[actionIndex].GetComponent<Action>();

    ActionInformation.Show(action.Name(), action.TpCost().ToString(), action.MpCost().ToString(), action.Description());

    int xPos = Unit.current.xPos;
    int zPos = Unit.current.zPos;

    List<Cursor> tiles = Helpers.GetRadialTiles(xPos, zPos, action.MaxDistance(), true);

    foreach(Cursor tile in tiles){
      if (IsValidTarget(action, tile, xPos, zPos)) tile.SetAttack();
    }
  }

    public static void ShowActionRangeCursors(Cursor cursor, int actionIndex)
    {
        Action action = Unit.current.Actions()[actionIndex].GetComponent<Action>();
        List<Cursor> tiles = new List<Cursor>();

        int xPos = cursor.xPos;
        int zPos = cursor.zPos;

        {
            if (action.CursorMode() == Action.CursorModes.Radial)
            {
                if (action.RadialDistance() > 0)
                {
                    tiles = Helpers.GetRadialTiles(xPos, zPos, action.RadialDistance(), true);
                }
            }
            else
            {
                tiles = Helpers.GetLineTiles(Unit.current.xPos, Unit.current.zPos, xPos, zPos, action.LineDistance());
            }
        }

        foreach (Cursor tile in tiles)
        {
            if (IsValidTarget(action, tile, xPos, zPos)) tile.SetAttackInRange();
        }
    }

  private static bool IsValidTarget(Action action, Cursor tile, int xPos, int zPos){
    if(action.CanTargetSelf() || tile.xPos != xPos || tile.zPos != zPos) {
      if(action.NeedsLineOfSight()){
        return(Helpers.CanHitTarget(tile));
      }else{
        return(true);
      }
    }
    return(false);
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

  private static void HighlightMovableTiles(List<Cursor> tiles) {
    foreach(List<Cursor> list in cursorMatrix){
      foreach(Cursor tile in list){
        tile.UnsetMovement();
      }
    }
    foreach(Cursor tile in tiles){
      tile.SetMovement();
    }
  }

  private static void UnsetMovement() {
    foreach(List<Cursor> list in cursorMatrix){
      foreach(Cursor tile in list){
        tile.UnsetMovement();
      }
    }
  }
}
