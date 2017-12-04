using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class NavMap : MonoBehaviour {

    public class QueueNode : FastPriorityQueueNode
    {
        public long id { get; private set; }
        public NavPath path { get; set; }
        public QueueNode(long id)
        {
            this.id = id;
        }
    }

    private float minLat, minLon, maxLat, maxLon;
    public GameObject map { get; private set; }
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
        int countLights = 0;

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
                    bool isLight = false;
                    children = node.ChildNodes;
                    foreach(XmlNode child in children) {
                        string key = child.Attributes["k"].Value;
                        string val = child.Attributes["v"].Value;
                        if(key.Equals("highway") && val.Equals("traffic_signals")) {
                            countLights++;
                            isLight = true;
                        }
                        var tag = new ArrayList();
                        tag.Add(key);
                        tag.Add(val);
                        tags.Add(tag);
                    }
                    baseMap.addNode(id, lat, lon, tags, isLight);
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
        
        // Debug.Log("Count: " + countLights);
        baseMap.render();
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

    public NavPath findPath(long start, long end) {
        Map m = map.GetComponent<Map>();
        Node s = m.Nodes[start];
        Node t = m.Nodes[end];
        return Dijkstra(s, t, m);
    }

    private NavPath Dijkstra(Node s, Node t, Map m) {
        Dictionary<long, QueueNode> touched = new Dictionary<long, QueueNode>();
        HashSet<long> done = new HashSet<long>();
        FastPriorityQueue<QueueNode> pq = new FastPriorityQueue<QueueNode>(m.Nodes.Count);
        QueueNode start = new QueueNode(s.id);
        start.path = new NavPath();
        touched[s.id] = start;
        pq.Enqueue(start, 0);

        while(pq.Count != 0) {
            QueueNode qn = pq.Dequeue();
            float distance = qn.Priority;
            NavPath p = qn.path;
            Node n = m.Nodes[qn.id];
            if(n.id == t.id) {
                Debug.Log(p.getEdges().Count);
                return p;
            }
            done.Add(n.id);
            foreach(var edgeId in n.Edges) {
                Way edge = m.Ways[edgeId];
                Node next = m.Nodes[edge.GetNext(n.id)];
                float newWeight = distance + edge.weight;
                if(!done.Contains(next.id)) {
                    if(touched.ContainsKey(next.id)) {
                        qn = touched[next.id];
                        if(qn.Priority > newWeight) {
                            pq.UpdatePriority(qn, newWeight);
                            qn.path = new NavPath(p.getEdges());
                            qn.path.Add(edge);
                        }
                    } else {
                        qn = new QueueNode(next.id);
                        qn.path = new NavPath(p.getEdges());
                        qn.path.Add(edge);
                        touched[next.id] = qn;
                        pq.Enqueue(qn, newWeight);
                    }
                }
            }
        }
        return new NavPath();
    }
}
