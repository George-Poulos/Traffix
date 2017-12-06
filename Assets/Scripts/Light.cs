using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour {
    public long id;
    public Vector3 scale;
    public enum LightState { RED_GREEN, RED_YELLOW, RED_RED_1, GREEN_RED, YELLOW_RED, RED_RED_2 }
    public enum LightColor { RED, GREEN, YELLOW }
    private LightState lightState;
    private const float SCALE_FACTOR = 0.02f;
    private const float OFFSET = 0.1f;
    private const float TRANSPARANCY = 0.1f;
    public const float CYCLE_SPEED = 0.5f; // Base cycle speed for all the lights.  Lower is faster.
    private const int GREEN_INTERVAL = 10;
    private const int YELLOW_INTERVAL = 1;
    private const int RED_INTERVAL = 1; // Interval for red-red lights
    private int currentInterval = 0;
    private GameObject Nlight, Slight, Elight, Wlight;

    // Use this for initialization
    void Start () {
        // Initially set the lights to a random LightState.
        lightState = (LightState)Math.Floor(UnityEngine.Random.value * Enum.GetNames(typeof(LightState)).Length);
        draw ();

        // public void InvokeRepeating(string methodName, float time, float repeatRate);
        // Invokes the method methodName in time seconds, then repeatedly every repeatRate seconds.
        InvokeRepeating("NextState", CYCLE_SPEED * UnityEngine.Random.value, CYCLE_SPEED);
    }

    // Update is called once per frame
    void Update () {
    }

    void NextState()
    {
        switch (lightState) {
            case LightState.RED_GREEN:
                if (currentInterval >= GREEN_INTERVAL) {
                    setState(LightState.RED_YELLOW);
                } else {
                    currentInterval++;
                }
                break;
            case LightState.RED_YELLOW:
                if (currentInterval >= YELLOW_INTERVAL) {
                    setState(LightState.RED_RED_1);
                } else {
                    currentInterval++;
                }
                break;
            case LightState.RED_RED_1:
                if (currentInterval >= RED_INTERVAL) {
                    setState(LightState.GREEN_RED);
                } else {
                    currentInterval++;
                }
                break;
            case LightState.GREEN_RED:
                if (currentInterval >= GREEN_INTERVAL) {
                    setState(LightState.YELLOW_RED);
                } else {
                    currentInterval++;
                }
                break;
            case LightState.YELLOW_RED:
                if (currentInterval >= YELLOW_INTERVAL) {
                    setState(LightState.RED_RED_2);
                } else {
                    currentInterval++;
                }
                break;
            case LightState.RED_RED_2:
                if (currentInterval >= RED_INTERVAL) {
                    setState(LightState.RED_GREEN);
                } else {
                    currentInterval++;
                }
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
            case LightState.RED_YELLOW:
                setLightColor(Nlight, LightColor.RED);
                setLightColor(Slight, LightColor.RED);
                setLightColor(Elight, LightColor.YELLOW);
                setLightColor(Wlight, LightColor.YELLOW);
                break;
            case LightState.RED_RED_1:
            case LightState.RED_RED_2:
                setLightColor(Nlight, LightColor.RED);
                setLightColor(Slight, LightColor.RED);
                setLightColor(Elight, LightColor.RED);
                setLightColor(Wlight, LightColor.RED);
                break;
            case LightState.GREEN_RED:
                setLightColor(Nlight, LightColor.GREEN);
                setLightColor(Slight, LightColor.GREEN);
                setLightColor(Elight, LightColor.RED);
                setLightColor(Wlight, LightColor.RED);
                break;
            case LightState.YELLOW_RED:
                setLightColor(Nlight, LightColor.YELLOW);
                setLightColor(Slight, LightColor.YELLOW);
                setLightColor(Elight, LightColor.RED);
                setLightColor(Wlight, LightColor.RED);
                break;
            default:
                break;
        }

        lightState = ls;
        currentInterval = 0;
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
        Color myYellow = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, TRANSPARANCY);

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
            case LightColor.YELLOW:
                go.GetComponent<Renderer>().material.color = myYellow;
                break;
            default:
                break;
        }
    }
}
