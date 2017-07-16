using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = MainCamera.instanceTransform.position;
		transform.rotation = MainCamera.instanceTransform.rotation;
	}
}
