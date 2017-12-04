using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public GameObject mapObj { get; private set; }
    public GameObject[] spawnPrefabs;
    public float scale = 1000;
    public List<Node> intersections { get; private set; }
    public List<Node> spawnPoints { get; private set; }
    public Dictionary<long, int> pathMapping { get; private set; }

    private float minLat, minLon, maxLat, maxLon;
    private Map map;
    private List<string> roadTypes = new List<string> { "motorway", "trunk",
                                                        "primary", "secondary", "tertiary", "unclassified", "residential",
                                                        "service", "motorway_link", "trunk_link", "primary_link",
                                                        "secondary_link", "tertiary_link", "road" };
    private Dictionary<long, List<NavPath>> paths = new Dictionary<long, List<NavPath>>();

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

        if(mapObj != null) Destroy(mapObj);
        mapObj = new GameObject("Map");
        mapObj.transform.position = new Vector3(0, 0, 0);
        mapObj.transform.parent = transform;
        mapObj.AddComponent<Map>();
        map = mapObj.GetComponent<Map>();
        map.navMap = this;

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
                    map.addNode(id, lat, lon, tags);
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
                    if(shouldAdd) map.addEdge(id, tags, refs);
                    break;

                default:
                    break;
            }
        }
        map.Generate();
        pathMapping = new Dictionary<long, int>();
        intersections = new List<Node>();
        foreach(var edge in map.Ways.Values) {
            intersections.Add(map.Nodes[edge.StartId]);
            pathMapping[edge.StartId] = intersections.Count-1;
            intersections.Add(map.Nodes[edge.EndId]);
            pathMapping[edge.EndId] = intersections.Count-1;
        }
        spawnPoints = intersections.FindAll(delegate(Node n) { return n.Edges.Count == 1; });
        GenSpawnPoints();
    }

    private void GenSpawnPoints() {
        foreach(var node in spawnPoints) {
            paths[node.id] = Dijkstra(node);
            Spawn s = node.gameObject.AddComponent<Spawn>();
            s.cars = spawnPrefabs;
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

    public List<NavPath> getPaths(long id) {
        if(paths.ContainsKey(id)) return paths[id];
        else return null;
    }

    private List<NavPath> Dijkstra(Node s) {
        Dictionary<long, QueueNode> touched = new Dictionary<long, QueueNode>();
        HashSet<long> done = new HashSet<long>();
        FastPriorityQueue<QueueNode> pq = new FastPriorityQueue<QueueNode>(map.Nodes.Count);
        List<NavPath> ret = Enumerable.Repeat(NavPath.NoPath, intersections.Count).ToList();

        QueueNode start = new QueueNode(s.id);
        start.path = new NavPath();
        touched[s.id] = start;
        pq.Enqueue(start, 0);

        while(pq.Count != 0) {
            QueueNode qn = pq.Dequeue();
            float distance = qn.Priority;
            NavPath p = qn.path;
            Node n = map.Nodes[qn.id];
            done.Add(n.id);
            ret[pathMapping[n.id]] = p;
            foreach(var edgeId in n.Edges) {
                Way edge = map.Ways[edgeId];
                if(edge.GetNext(n.id) != -1) {
                    Node next = map.Nodes[edge.GetNext(n.id)];
                    float newWeight = distance + edge.weight;
                    if(!done.Contains(next.id)) {
                        List<Vector3> points = new List<Vector3>(edge.waypoints);
                        if(edge.isBack(n.id)) points.Reverse();
                        if(touched.ContainsKey(next.id)) {
                            qn = touched[next.id];
                            if(qn.Priority > newWeight) {
                                pq.UpdatePriority(qn, newWeight);
                                qn.path = new NavPath(p.getPoints());
                                qn.path.Add(points);
                            }
                        } else {
                            qn = new QueueNode(next.id);
                            qn.path = new NavPath(p.getPoints());
                            qn.path.Add(points);
                            touched[next.id] = qn;
                            pq.Enqueue(qn, newWeight);
                        }
                    }
                }
            }
        }
       return ret;
    }
}
