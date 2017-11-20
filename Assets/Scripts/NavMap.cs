using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMap : MonoBehaviour {
    private float minLat, minLon, maxLat, maxLon;
    private GameObject map;
    public float scale = 100;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
    }

    public void setMap(string mapFilePath) {
        if(map != null) Destroy(map);
        map = new GameObject("Map");
        map.transform.position = new Vector3(0, 0, 0);
        map.transform.parent = transform;
        map.AddComponent<Map>();
        var baseMap = map.GetComponent<Map>();
        baseMap.navMap = this;

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreWhitespace = true;

        using (XmlReader reader = XmlReader.Create(mapFilePath, settings)) {
            reader.MoveToContent();
            while(reader.Read()) {
                if (reader.NodeType == XmlNodeType.Element) {
                    switch(reader.Name) {
                        case "bounds":
                            //attributes: minlat minlon maxlat maxlon
                            minLat = float.Parse(reader["minlat"]);
                            minLon = float.Parse(reader["minlon"]);
                            maxLat = float.Parse(reader["maxlat"]);
                            maxLon = float.Parse(reader["maxlon"]);
                            break;

                        case "node":
                            //attributes: id lat lon
                            //child: tag attributes: k v
                            long id = long.Parse(reader["id"]);
                            float lat = float.Parse(reader["lat"]);
                            float lon = float.Parse(reader["lon"]);
                            baseMap.addNode(id, lat, lon);
                            break;

                        case "way":
                            //attributes: id
                            //child: tag attributes: k v
                            //child: nd attributes: ref -> node id
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }

    public float lonToX(float lon) {
        return (lon - minLon) * scale;
    }

    public float latToY(float lat) {
        return (lat - minLat) * scale;
    }

    public float xToLon(float x) {
        return (x/scale) + minLon;
    }

    public float yToLat(float y) {
        return (y/scale) + minLat;
    }
}
