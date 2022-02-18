using Gameplay.Data;
using UnityEngine;

namespace Gameplay.Terrain
{
    public struct Chunk
    {
        public Array2D<byte> Data {get;}
        private int _xCoordinate;
        private int _yCoordinate;

        public Chunk(int xCoordinate, int yCoordinate, int chunkSize, int worldSeed, float frequency, float terrainDensity)
        {
            // Instatiate Data and coordiantes
            _xCoordinate = xCoordinate;
            _yCoordinate = yCoordinate;
            Data = new Array2D<byte>(chunkSize);
            
            // Build Data
            int xStart = xCoordinate * chunkSize;
            int yStart = yCoordinate * chunkSize;

            for (int x = 0; x < chunkSize; x++) {
                for (int y = 0; y < chunkSize; y++) {
                    float terrainValue = Mathf.PerlinNoise(xStart + x + worldSeed * frequency, yStart + y - worldSeed * frequency);
                    Data.Set(x, y, terrainValue < terrainDensity ? (byte)0 : (byte)1);
                }
            }
        }

        public Chunk(int xCoordinate, int yCoordinate, int chunkSize, byte[] data)
        {
            _xCoordinate = xCoordinate;
            _yCoordinate = yCoordinate;
            Data = new Array2D<byte>(chunkSize, data);
        }

        public void Set(int x, int y, byte value) {
            Data.Set(x, y, value);
        }

        public byte Get(int x, int y) {
            return Data.Get(x, y);
        }
    }
}