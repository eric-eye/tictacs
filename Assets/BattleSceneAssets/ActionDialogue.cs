using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionDialogue : MonoBehaviour
{
    private Text text;
    public string message;
    private float lifetime = 0;
    public System.Action whenDone;
    public Unit unit;
    public Color color = Color.yellow;
    private Camera mainCamera;
    private Transform anchor;
    private Vector3 originalPosition;

    // Use this for initialization
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        text = transform.Find("Canvas").Find("Panel").Find("Text").GetComponent<Text>();
        anchor = unit.transform.Find("UIAnchors").Find("ActionDialogue");
        originalPosition = anchor.transform.position;
        text.text = message;
        text.color = color;
        transform.parent = GameObject.Find("Popups").transform;
        FixPosition();
    }

    // Update is called once per frame
    void Update()
    {
        FixPosition();
        transform.position = mainCamera.WorldToScreenPoint(anchor.position);
        lifetime = lifetime + Time.deltaTime;
        if (lifetime > 2)
        {
            whenDone();
            Destroy(gameObject);
        };
    }

    private void FixPosition(){
        transform.position = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(originalPosition);
    }
}