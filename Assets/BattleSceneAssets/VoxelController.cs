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

  public class CoordinateList : List<Coordinate> {};

  private CoordinateList syncCoordinates = new CoordinateList();

  private List<List<int>> elevationMatrix = new List<List<int>>();
  private List<List<bool>> respawnMatrix = new List<List<bool>>();

  private static RectGrid _grid;
  private static Parallelepiped _renderer;
  private static int xMin;
  public static int xMax;
  private static int zMin;
  public static int zMax;

  public static RectGrid Grid()
  {
    return(GameObject.Find("Grid").GetComponent<RectGrid>());
  }

  public static List<int[]> respawnMarkerList = new List<int[]>{
    new int[] {1, 12},
    new int[] {8, 17},
    new int[] {12, 19},
    new int[] {14, 19},
    new int[] {7, 0},
    new int[] {13, 2},
    new int[] {19, 6},
    new int[] {19, 9},
  };

  private List<int[]> mapTopography = new List<int[]>
  {
    //         A  B  C  D  E  F  G  H  I  J  K  L  M  N  O  P  Q  R  S  T
    new int[] {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0}, // 1
    new int[] {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 0}, // 2
    new int[] {1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1}, // 3
    new int[] {1, 3, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1}, // 4
    new int[] {1, 3, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1}, // 5
    new int[] {1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 0, 1, 1, 1}, // 6
    new int[] {1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0}, // 7
    new int[] {1, 1, 2, 1, 1, 1, 1, 3, 3, 3, 1, 1, 1, 1, 2, 1, 0, 1, 1, 1}, // 8
    new int[] {0, 1, 2, 1, 1, 1, 1, 3, 3, 3, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1}, // 9
    new int[] {0, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1}, // 10
    new int[] {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0}, // 11
    new int[] {1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, // 12
    new int[] {1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 1, 1}, // 13
    new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1}, // 14
    new int[] {1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 3, 1, 1, 1, 0, 0, 1, 1, 1, 1}, // 15
    new int[] {1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1}, // 16
    new int[] {1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1}, // 17
    new int[] {1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1}, // 18
    new int[] {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0}, // 19
    new int[] {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0}, // 20
  };

  public static VoxelController instance;

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
	  xMin = 0;//(int)_renderer.From[0];
	  xMax = 20;//(int)_renderer.To[0];
	  zMin = 0;//(int)_renderer.From[2];
	  zMax = 20;//(int)_renderer.To[2];

    for(int x = xMin; x < xMax; x++){
      for(int z = zMin; z < zMax; z++){
        int elevationMax = mapTopography[x][z];
        Coordinate coordinate = new Coordinate();
        coordinate.x = x;
        coordinate.z = z;
        coordinate.y = elevationMax - 1;
        coordinate.respawnMarker = respawnMarkerList.Exists(r => r[0] == x && r[1] == z);
        syncCoordinates.Add(coordinate);
      }
    }

    CacheMatrix();

    RenderVoxels();

    CursorController.instance.Load();
	}

  void RenderVoxels(){
    for(int x = xMin; x < xMax; x++){
      for(int z = zMin; z < zMax; z++){
        int maxElevation = GetElevation(x, z);
        for(int elevation = 0; elevation < maxElevation + 1; elevation++){
          GameObject voxelObject = Instantiate(voxelPrefab, Vector3.zero, Quaternion.identity);
          voxelObject.transform.parent = GameObject.Find("Voxels").transform;
          Voxel voxel = voxelObject.GetComponent<Voxel>();
          voxel.xPos = x;
          voxel.zPos = z;
          voxel.yPos = elevation;
          voxel.respawnMarker = instance.respawnMatrix[x][z];
          if(elevation == maxElevation) {
            voxel.SetTerrainType(Voxel.TerrainType.Grass);
          }else{
            voxel.SetTerrainType(Voxel.TerrainType.Dirt);
          }
        }
      }
    }
  }
	
	// Update is called once per frame
	void Update () {
		
	}


}
