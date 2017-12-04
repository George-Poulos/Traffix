using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour {
    public long id;
    public Vector3 scale;
    public enum LightState { RED_GREEN, GREEN_RED }
    private LightState lightState;
    private const float SCALE_FACTOR = 0.05f;
    private const float OFFSET = 0.1f;

// Use this for initialization
    void Start () {
		draw ();
    }

    // Update is called once per frame
    void Update () {

    }

    public void draw() {
        GameObject Nlight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject Slight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject Elight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject Wlight = GameObject.CreatePrimitive(PrimitiveType.Sphere);

		Nlight.transform.parent = transform;
		Slight.transform.parent = transform;
		Elight.transform.parent = transform;
		Wlight.transform.parent = transform;

		Nlight.transform.position = transform.position;
		Slight.transform.position = transform.position;
		Elight.transform.position = transform.position;
		Wlight.transform.position = transform.position;

        Nlight.transform.localScale = new Vector3(SCALE_FACTOR + OFFSET, SCALE_FACTOR, SCALE_FACTOR);
        Slight.transform.localScale = new Vector3(SCALE_FACTOR - OFFSET, SCALE_FACTOR, SCALE_FACTOR);
        Elight.transform.localScale = new Vector3(SCALE_FACTOR, SCALE_FACTOR, SCALE_FACTOR + OFFSET);
        Wlight.transform.localScale = new Vector3(SCALE_FACTOR, SCALE_FACTOR, SCALE_FACTOR - OFFSET);

        switch (lightState) {
            case LightState.RED_GREEN:
                Nlight.GetComponent<Renderer>().material.color = Color.red;
                Slight.GetComponent<Renderer>().material.color = Color.red;
                Elight.GetComponent<Renderer>().material.color = Color.green;
                Wlight.GetComponent<Renderer>().material.color = Color.green;
                break;
            case LightState.GREEN_RED:
                Nlight.GetComponent<Renderer>().material.color = Color.green;
                Slight.GetComponent<Renderer>().material.color = Color.green;
                Elight.GetComponent<Renderer>().material.color = Color.red;
                Wlight.GetComponent<Renderer>().material.color = Color.red;
                break;
            default:
                break;
        }
    }
}
