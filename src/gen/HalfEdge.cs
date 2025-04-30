using Godot;
namespace CastleDefense.gen;

public class HalfEdge
{
    public HalfEdge Next { get; set; }  
    public HalfEdge Twin { get; set; }
    public Vector3 Vertex { get; set; }
    public Edge Edge { get; set; }
    public Face Face { get; set; }
    
}