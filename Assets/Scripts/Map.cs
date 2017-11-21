using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public NavMap navMap;
    private Dictionary<long, Node> mapNodes = new Dictionary<long, Node>();
    private Dictionary<long, Way> mapWays = new Dictionary<long, Way>();

    // Use this for initialization
    void Start () {
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
        w.transform.parent = transform;
        w.name = id.ToString();
        Way way = w.AddComponent<Way>();
        way.addTags(tags);
        way.id = id;
        foreach(long nodeId in refs) {
            way.addRef(nodeId);
            Node n = mapNodes[nodeId];
            n.addEdge(id);
        }
        var line = w.AddComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Sprites/Default"));
        // Set the width of the Line Renderer
        line.SetWidth(0.01F, 0.01F);
        line.startColor = Color.white;
        line.endColor = Color.white;
        // Set the number of vertex fo the Line Renderer
        line.positionCount = refs.Count;
        for(int i = 0; i < refs.Count; i++) {
            line.SetPosition(i, mapNodes[refs[i]].transform.position);
        }
        mapWays.Add(id, way);
    }
}
