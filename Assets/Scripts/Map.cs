using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Map : MonoBehaviour {
    public NavMap navMap;
    private Dictionary<long, Node> mapNodes = new Dictionary<long, Node>();
    private Dictionary<long, Way> mapWays = new Dictionary<long, Way>();
    public List<NavMeshSurface> navSurfaces = new List<NavMeshSurface>();

    // Use this for initialization
    void Start () {
        foreach(var surface in navSurfaces) {
            surface.BuildNavMesh();
        }

        int i = 0, j = 0, k = 0, l = 0;
        foreach(Way w in mapWays.Values) {
            if(w.refs.Count == 2) {
                i++;
            } else if(w.refs.Count == 3) {
                j++;
            } else if(w.refs.Count == 4) {
                k++;
            } else {
                l++;
            }
        }
        Debug.Log(i);
        Debug.Log(j);
        Debug.Log(k);
        Debug.Log(l);
    }

    // Update is called once per frame
    void Update () {

    }

    public void addNode(long id, float lat, float lon, ArrayList tags) {
        GameObject n = new GameObject();
        n.transform.parent = transform;
        n.transform.position = new Vector3(navMap.lonToX(lon), 0, navMap.latToY(lat));
        n.name = id.ToString();
        n.AddComponent<Node>();
        Node node = n.GetComponent<Node>();
        node.addTags(tags);
        node.id = id;
        mapNodes.Add(id, node);
    }

    public void addEdge(long id, ArrayList tags, List<long> refs) {
        GameObject w = new GameObject();
        w.name = id.ToString();
        var renderer = w.AddComponent<RoadRenderer>();
        renderer.map = this;
        Vector3[] positions = new Vector3[refs.Count];
        for(int i = 0; i < refs.Count; i++) {
            positions[i] = mapNodes[refs[i]].transform.position;
        }
        renderer.AddPositions(positions);
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
}
