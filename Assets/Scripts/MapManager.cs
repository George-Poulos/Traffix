using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public string xmlFileName;

    // Use this for initialization
    void Start () {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, xmlFileName);

        GameObject navMapObj = new GameObject("NavMap");
        navMapObj.transform.position = new Vector3(0, 0, 0);
        navMapObj.AddComponent<NavMap>();
        var navMap = navMapObj.GetComponent<NavMap>();
        navMap.setMap(filePath);
    }

    // Update is called once per frame
    void Update () {

    }
}
