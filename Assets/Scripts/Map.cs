using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public NavMap navMap;
    private Dictionary<long, GameObject> mapObjs;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void addNode(long id, float lat, float lon) {
        GameObject n = GameObject.CreatePrimitive(PrimitiveType.Plane);
        n.transform.localScale = new Vector3(0.01F, 0.01F, 0.01F);
        n.transform.parent = transform;
        n.transform.position = new Vector3(navMap.lonToX(lon), 0, navMap.latToY(lat));
        n.name = id.ToString();
    }
}
