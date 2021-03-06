﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public string xmlFileName;
    public GameObject[] spawnPrefabs;
    public GameObject roadPrefab;
    public GameObject buildingPrefab;

    private GameObject path;
    private GameObject navMapObj;
    private List<NavPath> paths;
    // Use this for initialization
    void Awake () {
    }

    // Update is called once per frame
    void Update () {

    }

    public void init (){
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, xmlFileName);

        navMapObj = new GameObject("NavMap");
        navMapObj.transform.position = new Vector3(0, 0, 0);
        var navMap = navMapObj.AddComponent<NavMap>();
        navMap.spawnPrefabs = spawnPrefabs;
        navMap.roadPrefab = roadPrefab;
        navMap.buildingPrefab = buildingPrefab;
        navMap.setMap(filePath);
    }
}
