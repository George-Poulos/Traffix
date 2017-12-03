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
        NavPath p = navMap.findPath(929584674, 26098106);
        GameObject path = new GameObject("Path");
        var line = path.AddComponent<LineRenderer>();
        List<Vector3> points = p.getPoints();
        // Set the width of the Line Renderer
        line.SetWidth(0.01F, 0.01F);
        // Set the number of vertex fo the Line Renderer
        line.positionCount = points.Count;
        for(int i = 0; i < points.Count; i++) {
            line.SetPosition(i, points[i]+(Vector3.up * .01f));
        }
        GameObject car = GameObject.CreatePrimitive(PrimitiveType.Cube);
        car.name = "Car";
        car.transform.localScale = new Vector3(.1f,.1f,.1f);
        var move = car.AddComponent<MoveCrap>();
        car.transform.position = navMap.map.GetComponent<Map>().Nodes[929584674].gameObject.transform.position;
        move.positions = points.ToArray();
    }

    // Update is called once per frame
    void Update () {

    }
}
