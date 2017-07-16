using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    transform.Rotate(Vector3.up * Time.deltaTime * 100);
    transform.Rotate(Vector3.right * Time.deltaTime * 100);
	}
}
