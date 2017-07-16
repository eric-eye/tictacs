using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBehavior : MonoBehaviour {
  private bool dragging = false;
  private float minDragTimer = 0.25f;
  private float dragTimer = 0;
  private bool selected = false;
  private Unit unit;
	private LineRenderer line;
	private Transform characterModel;

  public GameObject model;
  public Cursor modelCursor;
  public Cursor lastGoodModelCursor;
  public int stanceIndex = 0;

	// Use this for initialization
	void Start () {
	}

  public void Load(){
		characterModel = transform.Find("Body");
    unit = GetComponent<Unit>();
		line = GetComponent<LineRenderer>();

		model = GameObject.Instantiate(characterModel.gameObject, transform.position, Quaternion.identity);
    model.transform.parent = transform.Find("Model");
    model.transform.localScale = characterModel.localScale;

    SetModelCursor(Helpers.GetTile(unit.xPos, unit.zPos));
    HideModel();
  }
	
	// Update is called once per frame
	void Update () {
    if(!ModelController.inModelMode){
      dragTimer = 0;
      dragging = false;
      return;
    }

    bool clicked = Input.GetMouseButton(0);

    if(!dragging && clicked && Cursor.hovered && (Cursor.hovered.standingUnit == unit || Cursor.hovered.standingModel == model)){
      if(ModelController.unit != unit){
        ModelController.unit = unit;
        CursorController.ShowMoveCells();
        Menu.Refresh();
      }
    }

    if(clicked){
      dragTimer += Time.deltaTime;
      dragging = dragging || dragTimer >= minDragTimer && Cursor.hovered && Cursor.hovered.standingModel == model;
      if(dragging && !selected) DisplayModel(model);
    }else{
      selected = false;
      dragTimer = 0;
      dragging = false;

			MoveModelTowardCursor(modelCursor);
    }

    if(dragging){
      CursorController.Cancel();

			Cursor hoveredCursor = Cursor.hovered; 

      if(hoveredCursor){
				MoveModelTowardCursor(hoveredCursor);

        if(Cursor.hovered.standingModel == null){
					SetModelCursor(Cursor.hovered);
        }else if(Cursor.hovered.standingModel != model){
					SetModelCursor(lastGoodModelCursor);
        }
      }
    }
	}

  public Stance Stance(){
    return(unit.Stances()[stanceIndex].GetComponent<Stance>());
  }

	private void MoveModelTowardCursor(Cursor cursor){
			Vector3 target = Helpers.WorldPointFromCursor(cursor);
      print(target);
			Vector3 position = Vector3.Lerp(model.transform.parent.position, target, 0.75f);
      model.transform.parent.position = position;
			line.SetPosition(0, transform.position);
			line.SetPosition(1, position);
	}

	private void SetModelCursor(Cursor cursor){
    if(modelCursor) modelCursor.standingModel = null;
		modelCursor = cursor;
		lastGoodModelCursor = cursor;
		cursor.standingModel = model;
    if(modelCursor == Helpers.GetTile(unit.xPos, unit.zPos)){
      model.gameObject.active = false;
    }else{
      model.gameObject.active = true;
    }
	}

  public void SetStance(int newStanceIndex){
    stanceIndex = newStanceIndex;
  }

  public void HideModel(){
    model.transform.parent.gameObject.SetActive(false);
    line.enabled = false;
  }

  public void ShowModel(){
    stanceIndex = 0;
    Transform parent = model.transform.parent;
    Cursor tile = Helpers.GetTile(unit.xPos, unit.zPos);

    if(tile){
      SetModelCursor(tile);
      parent.gameObject.SetActive(true);
      parent.position = Helpers.WorldPointFromCursor(tile);
      model.transform.rotation = characterModel.transform.rotation;

      line.enabled = false;
    }
  }

  private void DisplayModel(GameObject model){
    model.transform.parent.gameObject.SetActive(true);

		SetColor("ArmLeft1", unit._color);
		SetColor("ArmRight1", unit._color);
		SetColor("Body1", unit._color);
		SetColor("LegLeft1", Color.white);
		SetColor("LegRight1", Color.white);
		SetColor("Head1", Color.white);

    GetComponent<LineRenderer>().enabled = true;
  }

	private void SetColor(string part, Color color){
		Renderer renderer = model.transform.Find("CharacterModel").Find(part).GetComponent<Renderer>();
    renderer.material = unit.transparentMaterial;
    renderer.material.color = new Color(color.r, color.g, color.b, 0.1f);

	}
}
