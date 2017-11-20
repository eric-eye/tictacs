using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{

  public Unit unit;
  public string points;
  
  private Text text;
  private Camera mainCamera;
  private Transform anchor;

  // Use this for initialization
  void Start()
  {
    text = GetComponent<Text>();
    anchor = unit.transform.Find("UIAnchors").Find("Points");
    mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    transform.parent = GameObject.Find("Popups").transform;
  }

  // Update is called once per frame
  void Update()
  {
    text.text = unit.points.ToString();
    transform.position = mainCamera.WorldToScreenPoint(anchor.position);
  }
}
