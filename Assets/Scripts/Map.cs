using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public NavMap navMap;
    private Dictionary<long, Node> mapNodes = new Dictionary<long, Node>();
    private Dictionary<long, Way> mapWays = new Dictionary<long, Way>();

    // Use this for initialization
    void Start () {
        int i = 0, j = 0, k = 0;
        foreach(Way w in mapWays.Values) {
            if(w.refs.Count == 2) {
                i++;
            } else if(w.refs.Count == 4) {
                j++;
            } else {
                k++;
            }
        }
        Debug.Log(i);
        Debug.Log(j);
        Debug.Log(k);
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
        GameObject w;
        if(refs.Count == 2) {
            w = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 start = mapNodes[refs[0]].transform.position;
            Vector3 end = mapNodes[refs[1]].transform.position;

            Vector3 between = end - start;
            float distance = between.magnitude;
            Vector3 scale = w.transform.localScale;
            scale.x = 0.01F;
            scale.y = 0.001F;
            scale.z = distance;
            w.transform.localScale = scale;
            w.transform.position = start + (between / 2.0F);
            w.transform.LookAt(end);

            Material roadMaterial = new Material(Shader.Find("Standard"));
            w.GetComponent<MeshRenderer>().material = roadMaterial;
            roadMaterial.mainTexture = Resources.Load("road") as Texture;
            roadMaterial.mainTextureScale = new Vector2(1,35*distance);
        } else if(refs.Count == 4) {
            w = new GameObject();
            var line = w.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = Color.yellow;
            line.endColor = Color.yellow;
            // Set the width of the Line Renderer
            line.SetWidth(0.01F, 0.01F);
            // Set the number of vertex fo the Line Renderer
            line.positionCount = refs.Count;
            for(int i = 0; i < refs.Count; i++) {
                line.SetPosition(i, mapNodes[refs[i]].transform.position);
            }
            //            w = GameObject.CreatePrimitive(PrimitiveType.Cube);
        } else {
            w = new GameObject();
            var line = w.AddComponent<LineRenderer>();
            // Set the width of the Line Renderer
            line.SetWidth(0.01F, 0.01F);
            // Set the number of vertex fo the Line Renderer
            line.positionCount = refs.Count;
            for(int i = 0; i < refs.Count; i++) {
                line.SetPosition(i, mapNodes[refs[i]].transform.position);
            }
        }
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
        mapWays.Add(id, way);
    }
}
