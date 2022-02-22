using UnityEngine;
using Gameplay.Data;
using System.Collections.Generic;
using System;

namespace Gameplay.Terrain
{
    public class ChunkBuilder
    {
        private int[][] triangleLookup = new int[][]{
        new int[]{},
        new int[]{7, 1, 0},
        new int[]{3, 2, 1},
        new int[]{7, 3, 0, 3, 2, 0},
        new int[]{5, 4, 3},
        new int[]{7, 1, 0, 5, 1, 7, 5, 3, 1, 5, 4, 3},
        new int[]{5, 4, 1, 4, 2, 1, },
        new int[]{7, 5, 0, 5, 4, 0, 4, 2, 0},
        new int[]{6, 5, 7},
        new int[]{6, 5, 1, 6, 1, 0},
        new int[]{6, 5, 7, 5, 1, 7, 5, 3, 1, 3, 2, 1},
        new int[]{5, 3, 2, 5, 2, 6, 6, 2, 0},
        new int[]{6, 3, 7, 6, 4, 3},
        new int[]{6, 4, 0, 4, 1, 0, 4, 3, 1},
        new int[]{6, 1, 7, 6, 2, 1, 6, 4, 2},
        new int[]{6, 4, 2, 6, 2, 0},
    };

    public int [][] altTriangleLookup3 = new int[][]{
        null,
        null,
        null,
        new int[]{7, 3, 0, 3, 2, 0, 5, 3, 7},
        null,
        null,
        new int[]{5, 4, 1, 4, 2, 1, 5, 1, 7},
        null,
        null,
        new int[]{6, 5, 1, 6, 1, 0, 5, 3, 1},
        null,
        null,
        new int[]{6, 3, 7, 6, 4, 3, 7, 3, 1},
        null,
        null,
        null,
    };

    public int [][] altTriangleLookup4 = new int[][]{
        null,
        new int[]{7, 1, 0, 7, 8, 1},
        new int[]{3, 2, 1, 8, 3, 1},
        null,
        new int[]{5, 4, 3, 5, 3, 8},
        null,
        null,
        null,
        new int[]{6, 5, 7, 5, 8, 7},
        null,
        null,
        null,
        null,
        null,
        null,
        null,
    };

    private bool[][] edgeLookup = new bool[][]{
        new bool[]{},
        new bool[]{true, true, false},
        new bool[]{true, false, true},
        new bool[]{true, true, false, true, false, false},
        new bool[]{true, false, true},
        new bool[]{true, true, false, true, true, true, true, true, true, true, false, true},
        new bool[]{true, false, true, false, false, true, },
        new bool[]{true, true, false, true, false, false, false, false, false},
        new bool[]{false, true, true},
        new bool[]{false, true, true, false, true, false},
        new bool[]{false, true, true, true, true, true, true, true, true, true, false, true},
        new bool[]{true, true, false, true, false, false, false, false, false},
        new bool[]{false, true, true, false, false, true},
        new bool[]{false, false, false, false, true, false, false, true, true},
        new bool[]{false, true, true, false, false, true, false, false, false},
        new bool[]{false, false, false, false, false, false},
    };

    public bool [][] altEdgeCases = new bool[][]{
        null,
        new bool[]{true, true, false, true, true, true},
        new bool[]{true, false, true, true, true, true},
        new bool[]{true, true, false, true, false, false, true, true, true},
        new bool[]{true, false, true, true, true, true},
        null,
        new bool[]{true, false, true, false, false, true, true, true, true},
        null,
        new bool[]{false, true, true, true, true, true},
        new bool[]{false, true, true, false, true, false, true, true, true},
        null,
        null,
        new bool[]{false, true, true, false, false, true, true, true, true},
        null,
        null,
        null,
    };


        private Vector3[] squareVertices = new Vector3[]{
            new Vector3(-1, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 1),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, -1),
            new Vector3(0, 0, -1),
            new Vector3(-1, 0, -1),
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, 0)
        };

        public Mesh BuildChunkMesh(World world, Vector2 chunkCoord)
        {
            Chunk chunk = world.GetChunk(chunkCoord);
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            List<Vector2> uv2 = new List<Vector2>();
            List<int> triangles = new List<int>();
            for (int x = 0; x < chunk.Data.Width; x++)
            {
                for (int y = 0; y < chunk.Data.Height; y++)
                {
                    Vector2 cellCoord = new Vector2(x, y);
                    int cellValue = chunk.Data.Get(x, y);
                    List<Vector2> cases = getCases(world, chunk, chunkCoord, cellCoord);
                    for (int c = 0; c < cases.Count; c++)
                    {
                        Vector2 _case = cases[c];
                        int[] tris = triangleLookup[(int)_case.x];
                        bool[] edges = edgeLookup[(int)_case.x];
                        if (cases.Count == 3 && altTriangleLookup3[(int)_case.x] != null) {

                            tris = altTriangleLookup3[(int)_case.x];
                            edges = altEdgeCases[(int)_case.x];
                        } else if (cases.Count == 4) {
                            tris = altTriangleLookup4[(int)_case.x];
                            edges = altEdgeCases[(int)_case.x];
                        }
                        
                        for (int i = 0; i < squareVertices.Length; i++)
                        {
                            Vector3 vertex = new Vector3(squareVertices[i].x + (x * 2), 0, squareVertices[i].z + (y * 2));
                            vertices.Add(vertex);
                            uv.Add(new Vector2(vertex.x, vertex.z));
                            uv2.Add(new Vector2(_case.y, 1));
                            
                        }
                        int triIndexStart = vertices.Count;
                        for (int i = tris.Length - 1; i >= 0; i--)
                        {
                            int triIndex = triIndexStart + tris[i] - squareVertices.Length;
                            triangles.Add(triIndex);
                            if (!edges[i]) {
                                uv2[triIndex] = new Vector2(uv2[triIndex].x, -1);
                            }
                        }
                        
                    }
                    // if (cases.Count == 4) 
                    // {
                    //     Debug.Log("Adding big case")
                    //     int[] tris = triangleLookup[16];
                    //     int triIndexStart = vertices.Count;
                    //     for (int i = 0; i < tris.Length; i++)
                    //     {
                    //         int triIndex = triIndexStart + tris[i] - squareVertices.Length;
                    //         triangles.Add(triIndex);
                    //     }
                    // }
                }
            }
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uv.ToArray();
            mesh.uv2 = uv2.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private List<Vector2> getCases(World world, Chunk chunk, Vector2 chunkCoord, Vector2 cellCoord)
        {
            List<Vector2> cases = new List<Vector2>();
            int terrainTypeCount = Enum.GetNames(typeof(TerrainType)).Length;
            for (int i = 0; i < terrainTypeCount; i++)
            {
                int value = (int)i;
                int[] caseValues = new int[]{
                    value == world.GetCell(chunkCoord, new Vector2(cellCoord.x, cellCoord.y)) ? 1 : 0,
                    value == world.GetCell(
                            cellCoord.x < chunk.Data.Width - 1 ? chunkCoord : new Vector2(chunkCoord.x + 1, chunkCoord.y),
                            new Vector2((cellCoord.x < chunk.Data.Width - 1) ? cellCoord.x + 1 : 0, cellCoord.y)
                        ) ? 1 : 0,
                    value == world.GetCell(
                            new Vector2(
                                cellCoord.x < chunk.Data.Width - 1 ? chunkCoord.x : chunkCoord.x + 1,
                                cellCoord.y < chunk.Data.Height - 1 ? chunkCoord.y : chunkCoord.y + 1),
                            new Vector2(
                                cellCoord.x < chunk.Data.Width - 1 ? cellCoord.x + 1: 0,
                                cellCoord.y < chunk.Data.Height - 1 ? cellCoord.y + 1 : 0)
                        ) ? 1 : 0,
                    value == world.GetCell(
                            cellCoord.y < chunk.Data.Height - 1 ? chunkCoord : new Vector2(chunkCoord.x, chunkCoord.y + 1),
                            new Vector2(cellCoord.x, cellCoord.y < chunk.Data.Height - 1 ? cellCoord.y + 1 : 0)
                        ) ? 1 : 0,
                };
                int _case = new Converter().BinaryToInt(caseValues);
                if (_case > 0)
                {
                    cases.Add(new Vector2(_case, i));
                }
            }
            return cases;
        }
    }
}
