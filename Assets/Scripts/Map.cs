﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Map : MonoBehaviour {
    public NavMap navMap;
    public Dictionary<long, Node> Nodes { get { return mapNodes; } }
    public Dictionary<long, Way> Ways { get { return mapWays; } }
    private Dictionary<long, Node> mapNodes = new Dictionary<long, Node>();
    private Dictionary<long, Way> mapWays = new Dictionary<long, Way>();
    private Dictionary<long, Building> mapBuildings = new Dictionary<long, Building>();
    private Dictionary<long, Light> Lights = new Dictionary<long, Light>();
    private List<Node> intersections;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void addNode(long id, float lat, float lon, ArrayList tags) {
        GameObject n = new GameObject();
        n.transform.parent = transform;
        Vector3 nodePosition = new Vector3(navMap.lonToX(lon), 0, navMap.latToY(lat));
        n.transform.position = nodePosition;
        n.name = id.ToString();
        n.AddComponent<Node>();
        Node node = n.GetComponent<Node>();
        node.addTags(tags);
        node.id = id;
        mapNodes.Add(id, node);
        if (node.Tags.ContainsKey ("highway") && node.Tags ["highway"] == "traffic_signals") {
            Light light = n.AddComponent<Light> ();
            Lights[id] = light;
        }
    }

    public void addEdge(long id, ArrayList tags, List<long> refs) {
        GameObject w = new GameObject();
        w.name = id.ToString();
        w.transform.parent = transform;
        Way way = w.AddComponent<Way>();
        way.addTags(tags);
        way.id = id;
        foreach(long nodeId in refs) {
            way.addRef(nodeId);
            Node n = mapNodes[nodeId];
            n.addEdge(id);
        }
        mapWays.Add(id, way);
    }

    public void addBuilding(long id, ArrayList tags, List<long> refs) {
        GameObject building = new GameObject();
        Building b = building.AddComponent<Building>();
        b.transform.parent = transform;
        b.addTags(tags);
        b.id = id;
        building.name = (b.tags.ContainsKey("name")) ? b.tags["name"] : id.ToString();
        foreach(long nodeId in refs) {
            b.addRef(nodeId);
        }
        mapBuildings.Add(id, b);
    }

    public void splitEdge(long id, Way edge, long bisectId) {
        GameObject w = new GameObject();
        w.name = id.ToString();
        w.transform.parent = transform;
        Way way = w.AddComponent<Way>();
        way.addTags(edge.tags);
        way.id = id;
        List<long> splitRefs = edge.split(bisectId);
        foreach(long nodeId in splitRefs) {
            way.addRef(nodeId);
            Node n = mapNodes[nodeId];
            n.replaceEdge(edge.id, id);
        }
        mapWays.Add(id, way);
    }

    public void Generate() {
        intersections = mapNodes.Values.ToList().FindAll(delegate(Node n) {
                return n.Edges.Count > 1;
            });
        long max = mapWays.Values.Max(delegate(Way w) {
                return w.id;
            });
        long nextEdgeId = max + 1;
        foreach(Node intersection in intersections) {
            foreach(var edgeId in intersection.Edges) {
                Way edge = mapWays[edgeId];
                if(edge.isBisect(intersection.id)) {
                    splitEdge(nextEdgeId, edge, intersection.id);
                    nextEdgeId++;
                }
            }
        }

        foreach(Way way in mapWays.Values) {
            GameObject w = way.gameObject;
            var renderer = w.AddComponent<RoadRenderer>();
            renderer.map = this;
            Vector3[] positions = new Vector3[way.refs.Count];
            for(int i = 0; i < way.refs.Count; i++) {
                positions[i] = mapNodes[way.refs[i]].transform.position;
            }
            renderer.AddPositions(positions);
            renderer.GenSegments();
        }
        foreach(Building b in mapBuildings.Values) {
            var renderer = b.gameObject.AddComponent<BuildingRenderer>();
            renderer.map = this;
            Vector3[] positions = new Vector3[b.refs.Count];
            for(int i = 0; i < b.refs.Count; i++) {
                positions[i] = mapNodes[b.refs[i]].transform.position;
            }
            renderer.AddPositions(positions);
        }
    }
}
