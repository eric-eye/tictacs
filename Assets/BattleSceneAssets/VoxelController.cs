using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;
using UnityEngine.Networking;

public class VoxelController : NetworkBehaviour {
  public struct Coordinate
    {
      public int x;
      public int z;
      public int y;
      public bool respawnMarker;
    };

  public GameObject voxelPrefab;

  public class CoordinateList : SyncListStruct<Coordinate> {};

  [SyncVar]
  private CoordinateList syncCoordinates = new CoordinateList();

  private List<List<int>> elevationMatrix = new List<List<int>>();
  private List<List<bool>> respawnMatrix = new List<List<bool>>();

  private static RectGrid _grid;
  private static Parallelepiped _renderer;
  private static int xMin;
  private static int xMax;
  private static int zMin;
  private static int zMax;

  public static List<int[]> respawnMarkerList = new List<int[]>{
    new int[] {0, 0},
    new int[] {5, 0},
    new int[] {10, 0},
    new int[] {15, 0},
    new int[] {4, 19},
    new int[] {9, 19},
    new int[] {14, 19},
    new int[] {19, 19},
  };

  private static VoxelController instance;

  public static int GetElevation(int x, int z){
    return(instance.elevationMatrix[x][z]);
  }

  void CacheMatrix(){
    foreach(Coordinate c in syncCoordinates){
      if(elevationMatrix.Count <= c.x){
        elevationMatrix.Add(new List<int>());
        respawnMatrix.Add(new List<bool>());
      }
      if(elevationMatrix[c.x].Count <= c.z){
        elevationMatrix[c.x].Add(c.y);
        respawnMatrix[c.x].Add(c.respawnMarker);
      }
      elevationMatrix[c.x][c.z] = c.y;
    }
  }

	// Use this for initialization
	void Start () {
    instance = this;

    _grid = GameObject.Find("Grid").GetComponent<RectGrid>();
    _renderer = _grid.gameObject.GetComponent<Parallelepiped>();
    xMin = (int)_renderer.From[0];
    xMax = (int)_renderer.To[0];
    zMin = (int)_renderer.From[2];
    zMax = (int)_renderer.To[2];

    if(NetworkServer.active){
      for(int x = xMin; x < xMax; x++){
        for(int z = zMin; z < zMax; z++){
          int elevationMax = 1;
          if(Random.value < .2f){
            elevationMax = 2;
          }
          Coordinate coordinate = new Coordinate();
          coordinate.x = x;
          coordinate.z = z;
          coordinate.y = elevationMax - 1;
          coordinate.respawnMarker = respawnMarkerList.Exists(r => r[0] == x && r[1] == z);
          syncCoordinates.Add(coordinate);
        }
      }

      NetworkServer.Spawn(gameObject);
    }

    CacheMatrix();

    RenderVoxels();

    CursorController.instance.Load();
	}

  void RenderVoxels(){
    for(int x = xMin; x < xMax; x++){
      for(int z = zMin; z < zMax; z++){
        for(int elevation = 0; elevation < GetElevation(x, z) + 1; elevation++){
          GameObject voxelObject = Instantiate(voxelPrefab, Vector3.zero, Quaternion.identity);
          voxelObject.transform.parent = GameObject.Find("Voxels").transform;
          Voxel voxel = voxelObject.GetComponent<Voxel>();
          voxel.xPos = x;
          voxel.zPos = z;
          voxel.yPos = elevation;
          voxel.respawnMarker = instance.respawnMatrix[x][z];
        }
      }
    }
  }
	
	// Update is called once per frame
	void Update () {
		
	}


}
