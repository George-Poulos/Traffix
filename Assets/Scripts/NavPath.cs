using System.Collections.Generic;
using UnityEngine;

public class NavPath {
    public static NavPath NoPath = new NavPath(false);
    public static NavPath EmptyPath = new NavPath();
    List<Vector3> points;
    public bool isPath { get; private set; }

    public NavPath() {
        this.points = new List<Vector3>();
        isPath = true;
    }

    public NavPath(bool isPath) {
        this.points = new List<Vector3>();
        this.isPath = isPath;
    }

    public NavPath(List<Vector3> points) {
        this.points = new List<Vector3>(points);
        isPath = true;
    }

    public void Add(List<Vector3> points) {
        this.points.AddRange(points);
    }

    public List<Vector3> getPoints() {
        return points;
    }
}
