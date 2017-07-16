using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hits : MonoBehaviour {

  private Text text;
  public int damage;
  private float lifetime = 0;


	// Use this for initialization
	void Start () {
    text = GetComponent<Text>();
    text.text = damage.ToString();
    transform.parent = GameObject.Find("Popups").transform;
    transform.position = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(transform.position);
	}
	
	// Update is called once per frame
	void Update () {
    lifetime = lifetime + Time.deltaTime;
    transform.Translate(Vector3.up * Time.deltaTime * 50);
    if(lifetime > 1) Destroy(gameObject);
	}
}
