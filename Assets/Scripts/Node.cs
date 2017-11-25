using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public long id;
    private Dictionary<string, string> tags = new Dictionary<string, string>();
    private List<long> edges = new List<long>();

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void addEdge(long id) {
        edges.Add(id);
    }

    public void addTags(ArrayList tags) {
        foreach(ArrayList tag in tags) {
            this.tags.Add((string)tag[0], (string)tag[1]);
        }
    }
}
