using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMap : MonoBehaviour {
    private float minLat, minLon, maxLat, maxLon;
    private GameObject map;
    public float scale = 1000;
    List<string> roadTypes = new List<string> { "motorway", "trunk",
    "primary", "secondary", "tertiary", "unclassified", "residential",
    "service", "motorway_link", "trunk_link", "primary_link",
    "secondary_link", "tertiary_link", "road" };

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
    }

    public void setMap(string mapFilePath) {
        bool shouldAdd;
        ArrayList tags;
        long id;
        XmlNodeList children;

        if(map != null) Destroy(map);
        map = new GameObject("Map");
        map.transform.position = new Vector3(0, 0, 0);
        map.transform.parent = transform;
        map.AddComponent<Map>();
        var baseMap = map.GetComponent<Map>();
        baseMap.navMap = this;

        // XmlReaderSettings settings = new XmlReaderSettings();
        // settings.IgnoreWhitespace = true;

        XmlDocument doc = new XmlDocument();
        doc.Load(mapFilePath);
        XmlNodeList nodes = doc["osm"].ChildNodes;
        foreach(XmlNode node in nodes ) {
            var attrs = node.Attributes;
            switch(node.Name) {
                case "bounds":
                    minLat = float.Parse(attrs["minlat"].Value);
                    minLon = float.Parse(attrs["minlon"].Value);
                    maxLat = float.Parse(attrs["maxlat"].Value);
                    maxLon = float.Parse(attrs["maxlon"].Value);
                    break;

                case "node":
                    id = long.Parse(attrs["id"].Value);
                    float lat = float.Parse(attrs["lat"].Value);
                    float lon = float.Parse(attrs["lon"].Value);
                    tags = new ArrayList();
                    children = node.ChildNodes;
                    foreach(XmlNode child in children) {
                        string key = child.Attributes["k"].Value;
                        string val = child.Attributes["v"].Value;
                        var tag = new ArrayList();
                        tag.Add(key);
                        tag.Add(val);
                        tags.Add(tag);
                    }
                    baseMap.addNode(id, lat, lon, tags);
                    break;

                case "way":
                    id = long.Parse(attrs["id"].Value);
                    shouldAdd = false;
                    tags = new ArrayList();
                    var refs = new List<long>();
                    children = node.ChildNodes;
                    foreach(XmlNode child in children) {
                        switch(child.Name) {
                            case "tag":
                                string key = child.Attributes["k"].Value;
                                string val = child.Attributes["v"].Value;
                                if(key == "highway" && roadTypes.Contains(val)) shouldAdd = true;
                                if(key == "layer" && val != "0") shouldAdd = false;
                                var tag = new ArrayList();
                                tag.Add(key);
                                tag.Add(val);
                                tags.Add(tag);
                                break;

                            case "nd":
                                refs.Add(long.Parse(child.Attributes["ref"].Value));
                                break;

                            default:
                                break;
                        }
                    }
                    if(shouldAdd) baseMap.addEdge(id, tags, refs);
                    break;

                default:
                    break;
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
