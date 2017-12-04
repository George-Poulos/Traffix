using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public string xmlFileName;
    public GameObject prefab;

    private GameObject path;
    private GameObject navMapObj;
    private List<NavPath> paths;
    // Use this for initialization
    void Start () {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, xmlFileName);

        navMapObj = new GameObject("NavMap");
        navMapObj.transform.position = new Vector3(0, 0, 0);
        navMapObj.AddComponent<NavMap>();
        var navMap = navMapObj.GetComponent<NavMap>();
        navMap.spawnPrefab = prefab;
        navMap.setMap(filePath);
    }

    // Update is called once per frame
    void Update () {

    }
}
