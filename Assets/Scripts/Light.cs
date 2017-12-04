using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour {
    public long id;
    public Vector3 position;
    public Vector3 scale;
    public enum LightState { RED_GREEN, GREEN_RED }
    private LightState lightState;
    private Transform lightTransform;
    private const float SCALE_FACTOR = 0.05f;
    private const float OFFSET = 0.1f;

// Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {

    }
    public Light(Vector3 pos, Transform tf) {
        position = pos;
        lightTransform = tf;
        // Initially set the lights to a random state.
        lightState = (LightState)Math.Round(UnityEngine.Random.value);
    }
    public void draw() {
        GameObject Nlight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject Slight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject Elight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject Wlight = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        Nlight.transform.parent = lightTransform;
        Slight.transform.parent = lightTransform;
        Elight.transform.parent = lightTransform;
        Wlight.transform.parent = lightTransform;

        Nlight.transform.position = position;
        Slight.transform.position = position;
        Elight.transform.position = position;
        Wlight.transform.position = position;

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
