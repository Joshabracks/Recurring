using UnityEngine;
using Gameplay.Data;
using System.Collections.Generic;
using Gameplay.Terrain;
public class MarchingSquares
{
    private Vector2[] vertices = new Vector2[]{
        new Vector2(-1, 1),
        new Vector2(0, 1),
        new Vector2(1, 1),
        new Vector2(-1, 0),
        // new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(-1, -1),
        new Vector2(0, -1),
        new Vector2(1, -1),
    };

    public struct ChunkBuilder
    {
        private Array2D<int> lookup;
        public void Init()
        {
            lookup = new Array2D<int>(
                16, 
                12, 
                new int[]{
                    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                    3, 6, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                    4, 7, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                    3, 4, 5, 4, 7, 5, -1, -1, -1,-1, -1, -1,
                    1, 2, 4,-1, -1, -1,-1, -1, -1,-1, -1, -1,
                    1, 2, 4, 1, 4, 6, 1, 6, 3, 3, 6, 5,
                    1, 2, 7, 1, 7, 6,-1, -1, -1,-1, -1, -1,
                    1, 2, 7, 1, 7, 3, 3, 7, 5,-1, -1, -1,
                    0, 1, 3,-1, -1, -1,-1, -1, -1,-1, -1, -1,
                    0, 1, 6, 0, 6, 5,-1, -1, -1,-1, -1, -1,
                    0, 1, 3, 1, 6, 3, 1, 4, 6, 4, 7, 6,
                    0, 1, 5, 1, 4, 5, 4, 7, 5,-1, -1, -1,
                    0, 2, 3, 2, 4, 3,-1, -1, -1,-1, -1, -1,
                    0, 6, 5, 0, 4, 6, 0, 2, 4,-1, -1, -1,
                    0, 2, 4, 2, 6, 4, 2, 7, 6,-1, -1, -1,
                    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                }
            );
        }

        public Mesh BuildChunkMesh(Dictionary<string, Chunk> chunks, string chunkKey) {
            Chunk chunk = chunks[chunkKey];
            Array2D<byte> data = chunk.Data;
            string[] keySplit = chunkKey.Split('|');
            int[] chunkCoord = new int[]{int.Parse(keySplit[0]), int.Parse(keySplit[1])};
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            List<int> triangles = new List<int>();
            for (int x = 0; x < data.Width; x++) {
                for (int y = 0; y < data.Height; y++) {
                    if ((int)data.Get(x, y) == 0) 
                    {
                        // List<Vector2> verts = getVerts(x, y);
                    }
                }
            }
            return mesh;
        }
    }

    // private int[][] lookup = new int[][]{
    //     new int[]{},
    //     new int[]{3, 7, 6},
    //     new int[]{5, 8, 7},
    //     new int[]{3, 5, 6, 5, 8, 6},
    //     new int[]{1, 2, 5},
    //     new int[]{1, 2, 5, 1, 5, 7, 1, 7, 3, 3, 7, 6},
    //     new int[]{1, 2, 8, 1, 8, 7},
    //     new int[]{1, 2, 8, 1, 8, 3, 3, 8, 6},
    //     new int[]{0, 1, 3},
    //     new int[]{0, 1, 7, 0, 7, 6},
    //     new int[]{0, 1, 3, 1, 7, 3, 1, 5, 7, 5, 8, 7},
    //     new int[]{0, 1, 6, 1, 5, 6, 5, 8, 6},
    //     new int[]{0, 2, 3, 2, 5, 3},
    //     new int[]{0, 7, 6, 0, 5, 7, 0, 2, 5},
    //     new int[]{0, 2, 5, 2, 7, 5, 2, 8, 7},
    //     new int[]{}
    // };
}
