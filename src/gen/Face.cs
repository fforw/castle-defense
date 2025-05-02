using System;

namespace CastleDefense.gen;

public class Face
{
    public HalfEdge HalfEdge { get; set; }

    public int VertexCount
    {
        get
        {

            int count = 0;
            HalfEdge current = HalfEdge;
            do
            {
                count++;
                current = current.Next;
            
            } while (current != HalfEdge);
            return count;
        }
    }
    
    
    public override string ToString()
    {
        string s = base.ToString() + ": ";

        HalfEdge current = HalfEdge;
        do
        {
            s += current.Vertex.Position.ToString() + ", ";
            current = current.Next;
            
        } while (current != HalfEdge);
        
        return s;
    }
}