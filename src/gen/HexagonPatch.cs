using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Godot;
using Godot.NativeInterop;

namespace CastleDefense.gen;

public class HexagonPatch
{
    public readonly HexagonPatchOptions Opts;

    public readonly int Q;
    public readonly int R; 

    public readonly List<Face> Faces;

    public HexagonPatch(int q, int r, HexagonPatchOptions opts = null)
    {
        Q = q;
        R = r;
        
        Opts = opts ?? new HexagonPatchOptions();
    }

    public List<Face> Build()
    {
        return CreateFaces();
    }

    public List<Face> CreateFaces()
    {
        List<Face> faces = [];
        List<Vector3> cell = Opts.Cell;

        int last = Opts.PatchSize - 1;
        
        for (int r = 0; r < Opts.PatchSize; r++)
        {
            for (int q = 0; q < Opts.PatchSize; q++)
            {
                var offset = Opts.CalculateOffset(Q + q, R + r);

                Face f0 = createTri(cell[0], cell[1], cell[2], offset);
                Face f1 = createTri(cell[0], cell[2], cell[3], offset);
                Face f2 = createTri(cell[0], cell[3], cell[4], offset);
                Face f3 = createTri(cell[0], cell[4], cell[5], offset);
                Face f4 = createTri(cell[0], cell[5], cell[6], offset);
                Face f5 = createTri(cell[0], cell[6], cell[1], offset);
                
                f0.HalfEdge.TwinWith(f5.HalfEdge.Next.Next);
                f1.HalfEdge.TwinWith(f0.HalfEdge.Next.Next);
                f2.HalfEdge.TwinWith(f1.HalfEdge.Next.Next);
                f3.HalfEdge.TwinWith(f2.HalfEdge.Next.Next);
                f4.HalfEdge.TwinWith(f3.HalfEdge.Next.Next);
                f5.HalfEdge.TwinWith(f4.HalfEdge.Next.Next);

                if (q > 0)
                {
                    // twin with the second face of the previous group
                    faces[^5].HalfEdge.Next.TwinWith(f4.HalfEdge.Next);                    
                }

                if (r > 0)
                {
                    bool isEvenRow = (r & 1) == 0;

                    if (isEvenRow)
                    {
                        if (q > 0)
                        {
                            faces[^((Opts.PatchSize + 1) * Constants.NumFaces - 2)].HalfEdge.Next.TwinWith(f5.HalfEdge.Next);
                        }
                        faces[^(Opts.PatchSize * Constants.NumFaces - 3)].HalfEdge.Next.TwinWith(f0.HalfEdge.Next);
                        
                    }
                    else
                    {
                        faces[^(Opts.PatchSize * Constants.NumFaces - 2)].HalfEdge.Next.TwinWith(f5.HalfEdge.Next);
                        if (q < last)
                        {
                            faces[^((Opts.PatchSize - 1) * Constants.NumFaces - 3)].HalfEdge.Next.TwinWith(f0.HalfEdge.Next);
                        }
                    }
                }

                faces.Add(f0);
                faces.Add(f1);
                faces.Add(f2);
                faces.Add(f3);
                faces.Add(f4);
                faces.Add(f5);
                
            }
        }

        return faces;
    }

    private Face createTri(Vector3 p0, Vector3 p1, Vector3 p2, Vector2I offset)
    {
        Face face = new Face();
        HalfEdge h0 = new HalfEdge(new Edge(), face, new Vertex(new Vector3(p0.X + offset.X, p0.Y + offset.Y, p0.Z)));
        h0.Edge.HalfEdge = h0;
        h0.Vertex.HalfEdge = h0;
        HalfEdge h1 = new HalfEdge(new Edge(), face, new Vertex(new Vector3(p1.X + offset.X, p1.Y + offset.Y, p1.Z)));
        h1.Edge.HalfEdge = h1;
        h1.Vertex.HalfEdge = h1;
        HalfEdge h2 = new HalfEdge(new Edge(), face, new Vertex(new Vector3(p2.X + offset.X, p2.Y + offset.Y, p2.Z)));
        h2.Edge.HalfEdge = h2;
        h2.Vertex.HalfEdge = h2;

        h0.Next = h1;
        h1.Next = h2;
        h2.Next = h0;
        
        face.HalfEdge = h0;
        
        return face;
    }
}