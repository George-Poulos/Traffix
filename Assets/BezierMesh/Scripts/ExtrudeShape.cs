public struct ExtrudeShape {
    public Vertex[] vert2Ds;
    public int[] lines;

    public ExtrudeShape( Vertex[] vert2Ds, int[] lines ) {
        this.vert2Ds = vert2Ds;
        this.lines = lines;
    }
}
