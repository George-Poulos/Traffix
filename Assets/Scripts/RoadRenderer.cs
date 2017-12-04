using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoadRenderer : MonoBehaviour {
    public Map map;
    private int vertices = 2;
    private List<GameObject> segments = new List<GameObject>(1);
    private List<OrientedPoint[]> paths = new List<OrientedPoint[]>(1);
    private List<Vector3> waypoints = new List<Vector3>();
    private float cost = 0;
    private List<Vector3> verticesPos = new List<Vector3>(2);
    private float width = 1F;
    private float height = 1F;
    public int positionCount {

        get { return vertices; }

        set {
            vertices = Math.Max(2, value);
            foreach(GameObject g in segments) {
                Destroy(g);
            }
            segments.Clear();
            segments.Capacity = vertices - 1;
            verticesPos.Clear();
            verticesPos.Capacity = vertices;
        }
    }


    // Use this for initialization
    void Start () {
        RenderSegments();
    }

    private void RenderSegments() {
        Vertex[] vertices = {
            new Vertex(new Vector3(-.0125f,-.0025f,0), Vector3.down, 0),
            new Vertex(new Vector3(.0125f,-.0025f,0), Vector3.down, 1),
            new Vertex(new Vector3(.0125f,-.0025f,0), Vector3.right, 1),
            new Vertex(new Vector3(.0125f,.0025f,0), Vector3.right, 1),
            new Vertex(new Vector3(.0125f,.0025f,0), Vector3.up, 1),
            new Vertex(new Vector3(-.0125f,.0025f,0), Vector3.up, 0),
            new Vertex(new Vector3(-.0125f,.0025f,0), Vector3.left, 0),
            new Vertex(new Vector3(-.0125f,-.0025f,0), Vector3.left, 0)
        };
        int[] lines = new int[]{
            0, 1,
            2, 3,
            4, 5,
            6, 7
        };
        ExtrudeShape shape = new ExtrudeShape(vertices, lines);

        for(int i = 0; i < segments.Count; i++) {
            RenderSegment(segments[i], shape, paths[i]);
        }
    }

    private void RenderSegment(GameObject segment, ExtrudeShape shape, OrientedPoint[] path) {
        // TODO: use the shape from the previous segment to make segments continuous
        // TODO: use sampling to fix texture stretching
        MeshFilter mf = segment.AddComponent<MeshFilter>();
        Mesh mesh = mf.mesh;
        Extrude(mesh, shape, path);
    }

    // Update is called once per frame
    void Update () {

    }

    public void SetWidth(float width, float height) {
        this.width = width;
        this.height = height;
    }

    public void AddPositions(Vector3[] positions) {
        foreach(var position in positions) {
            verticesPos.Add(position);
        }
    }

    public void AddPosition(Vector3 position) {
        verticesPos.Add(position);
        if(verticesPos.Count > 1) {
            Vector3 start = verticesPos[verticesPos.Count-2];
            Vector3 end = position;
            GameObject road = GameObject.CreatePrimitive(PrimitiveType.Cube);

            Vector3 between = end - start;
            float distance = between.magnitude;
            Vector3 scale = road.transform.localScale;
            scale.x = width;
            scale.y = height;
            scale.z = distance;
            road.transform.localScale = scale;
            road.transform.position = start + (between / 2.0F);
            road.transform.LookAt(end);

            Material roadMaterial = new Material(Shader.Find("Standard"));
            road.GetComponent<MeshRenderer>().material = roadMaterial;
            roadMaterial.mainTexture = Resources.Load("road") as Texture;
            roadMaterial.mainTextureScale = new Vector2(1,35*distance);
            road.transform.parent = transform;
            road.name = String.Format("{0}-{1}",
                                      gameObject.name,
                                      segments.Count);
            segments.Add(road);
        }
    }

    private void Extrude( Mesh mesh, ExtrudeShape shape, OrientedPoint[] path ) {
        int vertsInShape = shape.vert2Ds.Length;
        int segments = path.Length - 1;
        int edgeLoops = path.Length;
        int vertCount = vertsInShape * edgeLoops;
        int triCount = shape.lines.Length * segments;
        int triIndexCount = triCount * 3;

        int[] triangleIndices     = new int[ triIndexCount ];
        Vector3[] vertices     = new Vector3[ vertCount ];
        Vector3[] normals         = new Vector3[ vertCount ];
        Vector2[] uvs             = new Vector2[ vertCount ];

        for( int i = 0; i < path.Length; i++ ) {
            int offset = i * vertsInShape;
            for( int j = 0; j < vertsInShape; j++ ) {
                int id = offset + j;
                vertices[id] = path[i].LocalToWorld( shape.vert2Ds[j].point );
                normals[id] = path[i].LocalToWorldDirection( shape.vert2Ds[j].normal );
                uvs[id] = new Vector2( shape.vert2Ds[j].uCoord, i / ((float)edgeLoops) );
            }
        }
        int ti = 0;
        int[] lines = shape.lines;
        for( int i = 0; i < segments; i++ ) {
            int offset = i * vertsInShape;
            for ( int l = 0; l < lines.Length; l += 2 ) {
                int a = offset + lines[l] + vertsInShape;
                int b = offset + lines[l];
                int c = offset + lines[l+1];
                int d = offset + lines[l+1] + vertsInShape;
                triangleIndices[ti] = a;     ti++;
                triangleIndices[ti] = b;     ti++;
                triangleIndices[ti] = c;     ti++;
                triangleIndices[ti] = c;     ti++;
                triangleIndices[ti] = d;     ti++;
                triangleIndices[ti] = a;     ti++;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangleIndices;
        mesh.normals = normals;
        mesh.uv = uvs;
    }

    public void GenSegments() {
        int edgeLoops = 20;
        for(int i = 0; i < verticesPos.Count - 4; i += 3) {
            Vector3[] points = {
                verticesPos[i],
                verticesPos[i+1],
                verticesPos[i+2],
                verticesPos[i+3]
            };
            AddSegment(points, edgeLoops);
        }
        int remainder = (verticesPos.Count - 1) % 3;
        int n = verticesPos.Count;
        Vector3[] remPoints = new Vector3[4];
        if(remainder == 1) {
            remPoints[0] = verticesPos[n-2];
            remPoints[1] = verticesPos[n-1];
            remPoints[2] = verticesPos[n-1];
            remPoints[3] = verticesPos[n-1];
        } else if(remainder == 2) {
            remPoints[0] = verticesPos[n-3];
            remPoints[1] = verticesPos[n-2];
            remPoints[2] = verticesPos[n-2];
            remPoints[3] = verticesPos[n-1];
        } else if(n != 0) {
                remPoints[0] = verticesPos[n-4];
                remPoints[1] = verticesPos[n-3];
                remPoints[2] = verticesPos[n-2];
                remPoints[3] = verticesPos[n-1];
        }
        AddSegment(remPoints, edgeLoops);

        Way w = gameObject.GetComponent<Way>();
        w.weight = cost;
        w.waypoints = new List<Vector3>(waypoints);
    }

    private void AddSegment(Vector3[] points, int edgeLoops) {
        OrientedPoint[] path = new OrientedPoint[edgeLoops+1];
        CubicBezier3D bezier = new CubicBezier3D(points);
        GameObject segment = new GameObject();

        var renderer = segment.AddComponent<MeshRenderer>();

        Vector3? prev = null, curr = null;
        for(int i = 0; i <= edgeLoops; i++) {
            float t = (1f/edgeLoops) * i;
            prev = curr;
            curr = bezier.GetPoint(t);
            waypoints.Add((Vector3)curr);
            if(prev != null) {
                cost += ((Vector3)(curr - prev)).magnitude;
            }
            path[i] = new OrientedPoint((Vector3)curr, bezier.GetOrientation3D(t, Vector3.up));
        }

        Material roadMaterial = new Material(Shader.Find("Standard"));
        renderer.material = roadMaterial;
        roadMaterial.mainTexture = Resources.Load("road") as Texture;

        segment.transform.parent = transform;
        segment.name = gameObject.name + String.Format("-{0}",segments.Count);
        segments.Add(segment);
        paths.Add(path);
    }
}
