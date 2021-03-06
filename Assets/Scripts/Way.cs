﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way : MonoBehaviour {
    public long id;
    public float weight;
    public long StartId { get { return refs[0]; } }
    public long EndId { get { return refs[refs.Count-1]; } }
    public List<Vector3> waypoints = new List<Vector3>();
    public Dictionary<string, string> tags = new Dictionary<string, string>();
    public List<long> refs = new List<long>();
    public string type;

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
            if(key == "highway") type = val;
            this.tags.Add(key, val);
        }
    }

    public void addTags(Dictionary<string, string> tags) {
        this.tags = new Dictionary<string, string>(tags);
    }

    public long GetNext(long id) {
        if(StartId == id)
            return EndId;
        else if(!isOneWay() && EndId == id)
            return StartId;
        else
            return -1;
    }

    public bool isOneWay() {
        return tags.ContainsKey("oneway") && tags["oneway"] == "yes";
    }

    public bool isBisect(long id) {
        return (StartId != id && EndId != id);
    }

    public bool isBack(long id) {
        return EndId == id;
    }

    public List<long> split(long id) {
        List<long> ret;
        for(int i = 0; i < refs.Count; i++) {
            if(refs[i] == id) {
                ret = refs.GetRange(i, refs.Count-i);
                refs = refs.GetRange(0, i+1);
                return ret;
            }
        }
        return new List<long>();
    }
}
