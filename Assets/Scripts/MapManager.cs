using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public string xmlFileName;

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
        navMap.setMap(filePath);
        paths = navMap.getPaths(929515174).FindAll(delegate(NavPath p) { return p.isPath; });
        path = new GameObject("Path");
        path.AddComponent<LineRenderer>();
        InvokeRepeating("SpawnThing", 0f, 5f);
    }

    // Update is called once per frame
    void Update () {

    }

    void SpawnThing() {
        var p = RandomPath().First();
        var line = path.GetComponent<LineRenderer>();
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
        car.transform.position = navMapObj
            .GetComponent<NavMap>()
            .mapObj.GetComponent<Map>()
            .Nodes[929515174]
            .gameObject.transform.position;
        move.positions = points.ToArray();
    }

    public IEnumerable<NavPath> RandomPath() {
        System.Random rand = new System.Random();
        while(true)
        {
            yield return paths[rand.Next(paths.Count)];
        }
    }
}
