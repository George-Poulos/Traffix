using System.Collections.Generic;
using UnityEngine;

public class NavPath {
    List<Way> edges;

    public NavPath(List<Way> edges) {
        this.edges = new List<Way>(edges);
    }

    public NavPath() {
        this.edges = new List<Way>();
    }

    public void Add(Way edge) {
        this.edges.Add(edge);
    }

    public List<Vector3> getPoints() {
        List<Vector3> points = new List<Vector3>();
        foreach(Way edge in edges) {
            points.AddRange(edge.waypoints);
        }
        return points;
    }

    public List<Way> getEdges() {
        return edges;
    }
}
