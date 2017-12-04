using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public long id;
    public Dictionary<string, string> Tags = new Dictionary<string, string>();
    public List<long> Edges = new List<long>();

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void addEdge(long id) {
        Edges.Add(id);
    }

    public void addTags(ArrayList tags) {
        foreach(ArrayList tag in tags) {
            this.Tags.Add((string)tag[0], (string)tag[1]);
        }
    }

    public void replaceEdge(long id, long replacementId) {
        for(int i = 0; i < Edges.Count; i++) {
            if(Edges[i] == id) Edges[i] = replacementId;
        }
    }
}
