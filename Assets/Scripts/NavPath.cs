using System.Collections.Generic;
using UnityEngine;

public class NavPath {
    List<Vector3> points;

    public NavPath(List<Vector3> points) {
        this.points = new List<Vector3>(points);
    }

    public NavPath() {
        this.points = new List<Vector3>();
    }

    public void Add(List<Vector3> points) {
        this.points.AddRange(points);
    }

    public List<Vector3> getPoints() {
        return points;
    }
}
