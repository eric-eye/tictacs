using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridFramework.Grids;
using GridFramework.Extensions.Align;

public class Cursor : MonoBehaviour {

  public static Cursor hovered;

  public int xPos;
  public int zPos;
  public int yPos;
  public Unit standingUnit;
  public bool attack = false;
  public Color originalColor;
  public bool movable = false;

  private RectGrid _grid;
  private Color imprintColor;
  private bool path = false;

	// Use this for initialization
	void Start () {
    _grid = GameObject.Find("Grid").GetComponent<RectGrid>();

    Vector3 position = transform.position;

    position.x = xPos + 0.5f;
    position.z = zPos + 0.5f;
    position.y = yPos + 1.1f;

    transform.position = position;

    NegotiateColor();

    //_grid.AlignTransform(transform);
	}
	
  void OnMouseEnter() {
    hovered = this;
    NegotiateColor();
  }

  void OnMouseExit() {
    hovered = null;
    NegotiateColor();
  }

  public void NegotiateColor(){
    if(hovered == this){
      SetColor(new Color(1, 0, 0, 0.75f));
    }else if(attack){
      SetColor(new Color(0, 1, 0, 0.75f));
    }else if(path){
      SetColor(new Color(0, 0, 1, 0.75f));
    }else if(movable){
      SetColor(new Color(1, 1, 0, 0.5f));
    }else {
      SetColor(new Color(0, 0, 0, 0));
    }
  }

  public void SetMovement(){
    movable = true;
    NegotiateColor();
  }

  public void UnsetMovement(){
    movable = false;
    NegotiateColor();
  }

  public void SetPath(){
    path = true;
    NegotiateColor();
  }

  public void UnsetPath(){
    path = false;
    NegotiateColor();
  }

  public void SetAttack(){
    attack = true;
    NegotiateColor();
  }

  public void UnsetAttack(){
    attack = false;
    NegotiateColor();
  }

  void SetColor(Color color) {
    gameObject.GetComponent<Renderer>().material.color = color;
  }
}
