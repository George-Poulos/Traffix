using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour {
    public long id;
    public Vector3 position;
    public Vector3 scale;
    private Transform lightTransform;


    public Light(Vector3 pos, Transform tf) {
        position = pos;
        lightTransform = tf;
    }
    public void draw() {
        GameObject light = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        light.transform.parent = lightTransform;
        light.transform.position = position;
        light.transform.localScale = new Vector3(0.2F, 0.2f, 0.2f);
    }
}