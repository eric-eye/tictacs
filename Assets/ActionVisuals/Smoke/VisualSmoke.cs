using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSmoke : Visual {
	void Start () {
    transform.LookAt(Camera.main.transform.position);	
	}
}
