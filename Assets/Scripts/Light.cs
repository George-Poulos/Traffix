using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour {
    public long id;
    public Vector3 scale;
    public enum LightState { RED_GREEN, GREEN_RED }
    public enum LightColor { RED, GREEN }
    private LightState lightState;
    private const float SCALE_FACTOR = 0.02f;
    private const float OFFSET = 0.1f;
    private const float TRANSPARANCY = 0.4f;
    private GameObject Nlight, Slight, Elight, Wlight;

    // Use this for initialization
    void Start () {
        // Initially set the lights to a random state.
        lightState = (LightState)Math.Round(UnityEngine.Random.value);
        draw ();
        InvokeRepeating("NextState", 
            0.2f + UnityEngine.Random.value, 
            2.0f + UnityEngine.Random.value);
    }

    // Update is called once per frame
    void Update () {
    }

    void NextState()
    {
        switch (lightState) {
            case LightState.GREEN_RED:
                setState(LightState.RED_GREEN);
                break;
            case LightState.RED_GREEN:                
                setState(LightState.GREEN_RED);
                break;
            default:
                break;
        }
    }

    private void setState(LightState ls)
    {
        switch (ls) {
            case LightState.RED_GREEN:
                setLightColor(Nlight, LightColor.RED);
                setLightColor(Slight, LightColor.RED);
                setLightColor(Elight, LightColor.GREEN);
                setLightColor(Wlight, LightColor.GREEN);
                break;
            case LightState.GREEN_RED:
                setLightColor(Nlight, LightColor.GREEN);
                setLightColor(Slight, LightColor.GREEN);
                setLightColor(Elight, LightColor.RED);
                setLightColor(Wlight, LightColor.RED);
                break;
            default:
                break;
        }
        
        lightState = ls;
    }

    public void draw() {
        Nlight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Slight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Elight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Wlight = GameObject.CreatePrimitive(PrimitiveType.Sphere);

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

        setState(lightState);
    }

    private void setLightColor(GameObject go, LightColor lc)
    {
        Color myRed = new Color(Color.red.r, Color.red.g, Color.red.b, TRANSPARANCY);
        Color myGreen = new Color(Color.green.r, Color.green.g, Color.green.b, TRANSPARANCY);

        go.GetComponent<Renderer>().material.SetFloat("_Mode", 2);
        go.GetComponent<Renderer>().material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        go.GetComponent<Renderer>().material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        go.GetComponent<Renderer>().material.SetInt("_ZWrite", 0);
        go.GetComponent<Renderer>().material.DisableKeyword("_ALPHATEST_ON");
        go.GetComponent<Renderer>().material.EnableKeyword("_ALPHABLEND_ON");
        go.GetComponent<Renderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        go.GetComponent<Renderer>().material.renderQueue = 3000;

        switch(lc) {
            case LightColor.RED:
                go.GetComponent<Renderer>().material.color = myRed;
                break;
            case LightColor.GREEN:
                go.GetComponent<Renderer>().material.color = myGreen;
                break;
            default:
                break;
        }
    }
}
