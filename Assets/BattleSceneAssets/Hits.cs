using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hits : MonoBehaviour
{

  public string damage;
  private float lifetime = 0;
  public Color color = Color.white;
  private Vector3 originalPosition;

  // Use this for initialization
  void Start()
  {
    originalPosition = transform.position;
    Text text = transform.Find("Canvas").Find("Panel").Find("Text").GetComponent<Text>();
    Text shadow = transform.Find("Canvas").Find("Panel").Find("Shadow").GetComponent<Text>();
    text.text = damage;
    shadow.text = damage;
    text.color = color;
    transform.parent = GameObject.Find("Popups").transform;
    FixPosition();
  }

  // Update is called once per frame
  void Update()
  {
    FixPosition();
    lifetime = lifetime + Time.deltaTime;
    transform.Translate(Vector3.up * lifetime * 50);
    if (lifetime > 1) Destroy(gameObject);
  }

  private void FixPosition(){
    transform.position = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(originalPosition);
  }
}
