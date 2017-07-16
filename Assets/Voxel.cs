using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridFramework.Grids;
using GridFramework.Extensions.Align;

public class Voxel : MonoBehaviour {

  public int xPos;
  public int zPos;
  public int yPos;
  public bool respawnMarker;
  public Material grassMaterial;
  public Material dirtMaterial;

  private RectGrid _grid;

  public enum TerrainType {
    Dirt,
    Grass
  }

  public TerrainType terrainType;

  Dictionary<TerrainType, Material> terrainMaterials = new Dictionary<TerrainType, Material>();

  public void SetTerrainType(TerrainType newTerrainType){
    terrainType = newTerrainType;
    GetComponent<Renderer>().materials = new Material[]{ terrainMaterials[terrainType] };
  }

  void Awake () {
    terrainMaterials.Add(TerrainType.Dirt, dirtMaterial);
    terrainMaterials.Add(TerrainType.Grass, grassMaterial);
  }

	// Use this for initialization
	void Start () {
    if(GameObject.Find("Grid")){
      _grid = GameObject.Find("Grid").GetComponent<RectGrid>();

      Vector3 position = transform.position;

      position.x = xPos + 0.5f;
      position.z = zPos + 0.5f;
      position.y = yPos + 0.5f;

      if(respawnMarker){
        Helpers.GetTile(xPos, zPos).respawnMarker = true;
        transform.GetComponent<Renderer>().material.color = Color.blue;
      }

      transform.position = position;

      _grid.AlignTransform(transform);
    }

    // Get the mesh
    Mesh theMesh = this.transform.GetComponent<MeshFilter>().mesh as Mesh;

    // Now store a local reference for the UVs
    Vector2[] theUVs = new Vector2[theMesh.uv.Length];
    theUVs = theMesh.uv;

    // set UV co-ordinates

    //TOP
    theUVs[4] = new Vector2(.25f, .75f);
    theUVs[5] = new Vector2(.5f, .75f);
    theUVs[8] = new Vector2(.25f, 1f);
    theUVs[9] = new Vector2(.25f, 1f);

    //FRONT
    theUVs[2] = new Vector2(0f, .75f);
    theUVs[3] = new Vector2(.25f, .75f);
    theUVs[0] = new Vector2(0f, .5f);
    theUVs[1] = new Vector2(.25f, .5f);

    //BACK
    theUVs[10] = new Vector2(0f, .75f);
    theUVs[11] = new Vector2(.25f, .75f);
    theUVs[6] = new Vector2(0f, .5f);
    theUVs[7] = new Vector2(.25f, .5f);

    //LEFT
    theUVs[17] = new Vector2(0f, .75f);
    theUVs[18] = new Vector2(.25f, .75f);
    theUVs[16] = new Vector2(0f, .5f);
    theUVs[19] = new Vector2(.25f, .5f);

    //RIGHT
    theUVs[22] = new Vector2(0f, .75f);
    theUVs[21] = new Vector2(.25f, .75f);
    theUVs[23] = new Vector2(0f, .5f);
    theUVs[20] = new Vector2(.25f, .5f);

    //BOTTOM
    theUVs[15] = new Vector2(.25f, .5f);
    theUVs[12] = new Vector2(.5f, .5f);
    theUVs[13] = new Vector2(.25f, .25f);
    theUVs[14] = new Vector2(.25f, .5f);

    // Assign the mesh its new UVs
    theMesh.uv = theUVs;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
