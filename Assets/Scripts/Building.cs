using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    public long id;
    public Dictionary<string, string> tags = new Dictionary<string, string>();
    public List<long> refs = new List<long>();

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void addRef(long id) {
        refs.Add(id);
    }

    public void addTags(ArrayList tags) {
        foreach(ArrayList tag in tags) {
            string key = (string)tag[0];
            string val = (string)tag[1];
            this.tags.Add(key, val);
        }
    }
}
