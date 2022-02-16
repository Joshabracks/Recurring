using Data;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay.Terrain
{
    public struct Chunk
    {
        private Array2D<byte> _data;

        public Chunk(int xCoordinate, int yCoordinate)
        {
            _data = new Array2D<byte>(WorldData.ChunkSize);
            int xStart = xCoordinate * WorldData.ChunkSize;
            int yStart = yCoordinate * WorldData.ChunkSize;
            for (int x = xStart; x < xStart + WorldData.ChunkSize; x++) {
                for (int y = yStart; y < yStart + WorldData.ChunkSize; y++) {
                    float terrainValue = Mathf.PerlinNoise(x + WorldData.WorldSeed * WorldData.Frequency, y - WorldData.WorldSeed * WorldData.Frequency);
                    // Setting terrain to open (Value 0) or blocked (Value 1)
                    _data.Set(x, y, terrainValue < WorldData.TerrainDensity ? (byte)0 : (byte)1);
                }
            }
        }
    }
}