using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSlash : Visual {
	void Start () {
    transform.LookAt(Camera.main.transform.position);	
	}
}
