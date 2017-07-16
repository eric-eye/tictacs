using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;

public class VoxelController : MonoBehaviour {

  public GameObject voxelPrefab;

  public static List<List<int>> elevationMatrix = new List<List<int>>();
  //public static List<List<List<Voxel>>> voxelMatrix = new List<List<List<Voxel>>>();

  private static RectGrid _grid;
  private static Parallelepiped _renderer;
  private static int xMin;
  private static int xMax;
  private static int zMin;
  private static int zMax;

	// Use this for initialization
	void Start () {
    _grid = GameObject.Find("Grid").GetComponent<RectGrid>();
    _renderer = _grid.gameObject.GetComponent<Parallelepiped>();
    xMin = (int)_renderer.From[0];
    xMax = (int)_renderer.To[0];
    zMin = (int)_renderer.From[2];
    zMax = (int)_renderer.To[2];

    for(int x = xMin; x < xMax; x++){
      elevationMatrix.Add(new List<int>());
      //voxelMatrix.Add(new List<List<Voxel>>());
      for(int z = zMin; z < zMax; z++){
        int elevationMax = 1;
        if(Random.value < .2f){
          elevationMax = 2;
        }
        elevationMatrix[x].Add(elevationMax - 1);
        //voxelMatrix[x].Add(new List<Voxel>());
        for(int elevation = 0; elevation < elevationMax; elevation++){
          GameObject voxelObject = Instantiate(voxelPrefab, Vector3.zero, Quaternion.identity);
          voxelObject.transform.parent = GameObject.Find("Voxels").transform;
          Voxel voxel = voxelObject.GetComponent<Voxel>();
          voxel.xPos = x;
          voxel.zPos = z;
          voxel.yPos = elevation;
          //voxelMatrix[x][z].Add(voxel);
        }
      }
    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
