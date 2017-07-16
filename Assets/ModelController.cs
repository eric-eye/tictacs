using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour {
  public static bool inModelMode = false;
  public static Unit unit;

  public void Toggle(){
    inModelMode = !inModelMode;
    unit = Unit.current;

    ModelModeButton.ToggleText(inModelMode);

    if(inModelMode){
      Camera.main.GetComponent<MainCamera>().ModelMode();
      ShowModels();
    }else{
      Camera.main.GetComponent<MainCamera>().Reset();
      HideModels();
    }

    GameController.RefreshPlayerView();
  }

  private void HideModels(){
    foreach(Unit unit in Unit.All()){
      ModelBehavior modelBehavior = unit.GetComponent<ModelBehavior>();
      modelBehavior.HideModel();
    }
  }

  private void ShowModels(){
    foreach(Unit unit in Unit.All()){
      ModelBehavior modelBehavior = unit.GetComponent<ModelBehavior>();
      modelBehavior.ShowModel();
    }
  }
}
