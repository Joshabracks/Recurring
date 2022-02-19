using UnityEngine;
using Gameplay.Data;
using System.Collections.Generic;
using Gameplay.Terrain;

namespace Gameplay.Terrain
{

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
        new int[]{6, 4, 2, 6, 2, 0}
    };

    private Vector3[] squareVertices = new Vector3[]{
      new Vector3(-1, 0, -1), 
      new Vector3(0, 0, -1), 
      new Vector3(1, 0, -1),
      new Vector3(1, 0, 0),                       
      new Vector3(1, 0, 1),
      new Vector3(0, 0, 1), 
      new Vector3(-1, 0, 1), 
      new Vector3(-1, 0, 0)
    };

      public Mesh BuildChunkMesh(World world, Vector2 chunkCoord)
      {
        Chunk chunk = world.GetChunk(chunkCoord);
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        List<int> triangles = new List<int>();
        for (int x = 0; x < chunk.Data.Width; x++)
        {
          for (int y = 0; y < chunk.Data.Height; y++)
          {
            Vector2 cellCoord = new Vector2(x, y);
            byte cellValue = world.GetCell(chunkCoord, cellCoord);
            int _case = getCase(world, chunk, chunkCoord, cellCoord);
            int[] tris = triangleLookup[_case];
            if (tris.Length > 0) {
              for (int i = 0; i < squareVertices.Length; i++) {
                vertices.Add(new Vector3(squareVertices[i].x + (x * 2), 0, squareVertices[i].y + (y * 2)));
                // --- do stuff
              }
            }
          }
        }
        return mesh;
      }

      private int getCase(World world, Chunk chunk, Vector2 chunkCoord, Vector2 cellCoord)
      {
        byte value = chunk.Get((int)cellCoord.x, (int)cellCoord.y);
        int[] caseValues = new int[]{
          value == world.GetCell(chunkCoord, new Vector2(cellCoord.x, cellCoord.y)) ? 1 : 0,
          value == world.GetCell(chunkCoord, new Vector2(cellCoord.x + 1, cellCoord.y)) ? 1 : 0,
          value == world.GetCell(chunkCoord, new Vector2(cellCoord.x + 1, cellCoord.y + 1)) ? 1 : 0,
          value == world.GetCell(chunkCoord, new Vector2(cellCoord.x, cellCoord.y + 1)) ? 1 : 0,
        };
        int _case = new Converter().BinaryToInt(caseValues);
        return _case;
      }
    }
  }
}