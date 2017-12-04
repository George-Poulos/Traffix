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
    private const float SCALE_FACTOR = 0.1f;


    public Light(Vector3 pos, Transform tf) {
        position = pos;
        lightTransform = tf;
        // Initially set the lights to a random state.
        lightState = (LightState)Math.Round(UnityEngine.Random.value);
    }
    public void draw() {
        GameObject light = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        light.transform.parent = lightTransform;
        light.transform.position = position;
        light.transform.localScale = new Vector3(SCALE_FACTOR, SCALE_FACTOR, SCALE_FACTOR);
        switch (lightState) {
            case LightState.RED_GREEN:
                light.GetComponent<Renderer>().material.color = Color.red;
                break;
            case LightState.GREEN_RED:
                light.GetComponent<Renderer>().material.color = Color.green;
                break;
            default:
                break;
        }
    }
}
