using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRenderer : MonoBehaviour {
    public Map map;
    private Building b;
    private List<Vector3> verticesPos = new List<Vector3>();

    // Use this for initialization
    void Start () {
        b = gameObject.GetComponent<Building>();
        float height = map.navMap.metersToUnit(130f);
        try {
            if(b.tags.ContainsKey("height"))
                height = map.navMap.metersToUnit(float.Parse(b.tags["height"]));
            if(b.tags.ContainsKey("building:levels"))
                height = map.navMap.metersToUnit(int.Parse(b.tags["building:levels"]) * 3.5f);
        } catch {
            height = map.navMap.metersToUnit(float.Parse(b.tags["height"].Split(' ')[0]));
        }

        for(int i = 1; i < verticesPos.Count; i++) {
            GameObject w = Instantiate(map.navMap.buildingPrefab, transform.position, Quaternion.identity) as GameObject;
            Vector3 start = verticesPos[i-1];
            Vector3 end = verticesPos[i];

            Vector3 between = end - start;
            float distance = between.magnitude;
            Vector3 scale = w.transform.localScale;
            scale.x = 0.01F;
            scale.y = height;
            scale.z = distance;
            w.transform.localScale = scale;
            w.transform.position = start + (between / 2.0F);
            w.transform.LookAt(end);
            w.transform.parent = transform;
            w.name = gameObject.name + "-" + i.ToString();
        }
        Vector3 position = transform.position;
        position += new Vector3(0,height/2.0F,0);
        transform.position = position;
    }

    // Update is called once per frame
    void Update () {

    }

    public void AddPositions(Vector3[] positions) {
        if(positions.Length < 2) throw new Exception("not enough vertices for a building.");
        foreach(var position in positions) {
            verticesPos.Add(position);
        }
    }
}
