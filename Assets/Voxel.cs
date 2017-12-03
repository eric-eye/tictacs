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

  private RectGrid _grid;

	// Use this for initialization
	void Start () {
    _grid = GameObject.Find("Grid").GetComponent<RectGrid>();

    Vector3 position = transform.position;

    position.x = xPos + 0.5f;
    position.z = zPos + 0.5f;
    position.y = yPos + 0.5f;

    if(respawnMarker){
      transform.GetComponent<Renderer>().material.color = Color.blue;
    }

    transform.position = position;

    _grid.AlignTransform(transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
