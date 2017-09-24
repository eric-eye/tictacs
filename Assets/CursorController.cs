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
    if (GameController.state == GameController.State.PickAction && selected){
      ResetPath();
    }
    if (GameController.state == GameController.State.PickTarget){
      GameController.CancelAttack();
    }
	}

  public static void ShowMoveCells(){
    List <int[]> path = GetAllPaths(Unit.current.xPos, Unit.current.zPos, Unit.current.MoveLength(), false);
    HighlightMovableTiles(path);
  }

  public void Confirm(){
    //if((GameController.state == GameController.State.PickTarget) &&
        //Cursor.hovered && Cursor.hovered.attack){
      //GameController.DoAction(Cursor.hovered);
    //}
  }

  public void ShowPath(){
    selected = Cursor.hovered;
    _path = DeriveShortestPath(selected.xPos, selected.zPos, Unit.current.xPos, Unit.current.zPos);
    HighlightTiles(_path);
  }

  [Command]
  public void CmdMoveAlong(int x, int z){
    List<int[]> path = DeriveShortestPath(x, z, Unit.current.xPos, Unit.current.zPos);
    moveEnabled = false;
    Coordinate[] coordinates = new Coordinate[path.Count];
    int c = 0;
    foreach(int[] array in path){
      Coordinate coordinate = new Coordinate();
      coordinate.x = array[0];
      coordinate.z = array[1];
      coordinate.counter = array[2];
      coordinate.elevation = array[3];
      coordinates[c] = coordinate;
      c++;
    }
    Unit.current.CmdSetPath(coordinates);
  }

  public static void ResetPath(){
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
        Cursor tile = GetTile(x, z);
        tile.UnsetAttack();
      }
    }
  }

  public static void ShowActionCursors(GameObject actionObject){
    IAction action = actionObject.GetComponent<IAction>();

    int xPos = Unit.current.xPos;
    int zPos = Unit.current.zPos;

    foreach(int[] coordinates in GetAllPaths(xPos, zPos, action.MaxDistance(), true)){
      if(action.CanTargetSelf() || coordinates[0] != xPos || coordinates[1] != zPos) {
        Cursor tile = GetTile(coordinates[0], coordinates[1]);
        if(tile) {
          if(action.NeedsLineOfSight()){
            RaycastHit hit;
            GameObject target = tile.gameObject;

            if(tile.standingUnit) target = tile.standingUnit.transform.Find("Hittable").gameObject;
            if (Physics.Linecast(Unit.current.transform.Find("Hittable").transform.position, target.transform.position, out hit, 1 << 8)){
              if(hit.collider.gameObject == target.gameObject){
                tile.SetAttack();
              }
            }
          }else{
            tile.SetAttack();
          }
        }
      }
    }
  }

  private static List<Cursor> Neighbors(int xPos, int zPos){
    List<Cursor> list = new List<Cursor>();

    list.Add(GetTile(xPos + 1, zPos));
    list.Add(GetTile(xPos - 1, zPos));
    list.Add(GetTile(xPos, zPos + 1));
    list.Add(GetTile(xPos, zPos - 1));
    list.RemoveAll(r => (r == null));

    return list;
  }

  private static Cursor GetTile(int x, int z){
    if(x >= 0 && z>= 0 && x < cursorMatrix.Count && z < cursorMatrix[x].Count){
      return cursorMatrix[x][z];
    } else {
      return null;
    }
  }

  private static void HighlightTiles(List<int[]> tileCoordinates) {
    foreach(List<Cursor> list in cursorMatrix){
      foreach(Cursor tile in list){
        tile.UnsetPath();
      }
    }
    for(int i = 0; i < tileCoordinates.Count; i++){
      Cursor tile = GetTile(tileCoordinates[i][0], tileCoordinates[i][1]);
      if(tile) tile.SetPath();
    }
  }

  private static void HighlightMovableTiles(List<int[]> tileCoordinates) {
    foreach(List<Cursor> list in cursorMatrix){
      foreach(Cursor tile in list){
        tile.UnsetMovement();
      }
    }
    for(int i = 0; i < tileCoordinates.Count; i++){
      Cursor tile = GetTile(tileCoordinates[i][0], tileCoordinates[i][1]);
      if(tile) tile.SetMovement();
    }
  }

  public static void UnsetMovement() {
    foreach(List<Cursor> list in cursorMatrix){
      foreach(Cursor tile in list){
        tile.UnsetMovement();
      }
    }
  }

  private static List<int[]> DeriveShortestPath(int xPos, int zPos, int originX, int originZ) {
    List<int[]> queue = new List<int[]>();
    List<int[]> shortestPath = new List<int[]>();
    queue.Add(new int[] { xPos, zPos, 0, VoxelController.GetElevation(xPos, zPos) });
    for(int i = 0; i < queue.Count; i++){
      int[] entry = queue[i];
      int counter = entry[2] + 1;

      List<Cursor> neighbors = Neighbors(entry[0], entry[1]);

      List<int[]> newCells = new List<int[]>();

      foreach(Cursor cursor in neighbors){
        if(!cursor.standingUnit || cursor.standingUnit == Unit.current){
          int elevation = VoxelController.GetElevation(cursor.xPos, cursor.zPos);
          if(Mathf.Abs(elevation - VoxelController.GetElevation(entry[0], entry[1])) < 2){
              newCells.Add(new int[] { cursor.xPos, cursor.zPos, counter, elevation });
          }
        }
      }

      bool reachedDestination = false;

      for(int a = 0; a < newCells.Count; a++){
        if(newCells[a][0] == originX && newCells[a][1] == originZ){
          reachedDestination = true;
          break;
        }
      }

      for(int a = newCells.Count - 1; a >= 0; a--){
        for(int g = 0; g < queue.Count; g++) {
          if(newCells[a][0] == queue[g][0] &&
              newCells[a][1] == queue[g][1] &&
              newCells[a][2] >= queue[g][2]) {
            newCells.RemoveAt(a);
            break;
          }
        }
      }

      for(int a = 0; a < newCells.Count; a++){
        queue.Add(newCells[a]);
      }

      if(reachedDestination) {
        queue.Reverse();
        int firstIndex = queue.FindIndex(r => (r[0] == originX && r[1] == originZ));
        shortestPath.Add(queue[firstIndex]);

        int[] previousElement = queue[firstIndex];


        for(int b = firstIndex; b < queue.Count; b++){
          int[] currentElement = queue[b];

          if(
              (
               (
                currentElement[0] == previousElement[0] - 1 &&
                currentElement[1] == previousElement[1]
               ) ||
               (
                currentElement[0] == previousElement[0] + 1 &&
                currentElement[1] == previousElement[1]
               ) ||
               (
                currentElement[0] == previousElement[0] &&
                currentElement[1] == previousElement[1] + 1
               ) ||
               (
                currentElement[0] == previousElement[0] &&
                currentElement[1] == previousElement[1] - 1
               )
              ) && currentElement[2] == previousElement[2] - 1
            ){
            shortestPath.Add(currentElement);
            previousElement = currentElement;
          }
        }

        break;
      }
    }

    return(shortestPath);
  }

  private static List<int[]> GetAllPaths(int originX, int originZ, int maxHops, bool allowOthers) {
    List<int[]> queue = new List<int[]>();
    queue.Add(new int[] { originX, originZ, 0 });
    for(int i = 0; i < queue.Count; i++){
      int[] entry = queue[i];
      int counter = entry[2] + 1;
      if(counter > maxHops) continue;

      List<Cursor> neighbors = Neighbors(entry[0], entry[1]);

      List<int[]> newCells = new List<int[]>();

      foreach(Cursor cursor in neighbors){
        if(allowOthers || !cursor.standingUnit || cursor.standingUnit == Unit.current){
          int elevation = VoxelController.GetElevation(cursor.xPos, cursor.zPos);
          if(Mathf.Abs(elevation - VoxelController.GetElevation(entry[0], entry[1])) < 2){
            newCells.Add(new int[] { cursor.xPos, cursor.zPos, counter, elevation });
          }
        }
      }

      for(int a = newCells.Count - 1; a >= 0; a--){
        for(int g = 0; g < queue.Count; g++) {
          if(newCells[a][0] == queue[g][0] &&
              newCells[a][1] == queue[g][1] &&
              newCells[a][2] <= queue[g][2]) {
            newCells.RemoveAt(a);
            break;
          }
        }
      }

      for(int a = 0; a < newCells.Count; a++){
        queue.Add(newCells[a]);
      }
    }

    return(queue);
  }
}
