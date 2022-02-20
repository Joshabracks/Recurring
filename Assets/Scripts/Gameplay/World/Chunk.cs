using Gameplay.Data;
using UnityEngine;

namespace Gameplay.Terrain
{
    public struct Chunk
    {
        public Array2D<int> Data {get;}
        public int _xCoordinate;
        public int _yCoordinate;

        public Chunk(int xCoordinate, int yCoordinate, int chunkSize, int worldSeed, float frequency, float terrainDensity)
        {
            FastNoiseLite fs = new FastNoiseLite();
            fs.SetSeed(worldSeed);
            fs.SetFrequency(frequency);
            fs.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
            // Instatiate Data and coordiantes
            _xCoordinate = xCoordinate;
            _yCoordinate = yCoordinate;
            Data = new Array2D<int>(chunkSize);
            
            // Build Data
            int xOffset = xCoordinate * chunkSize;
            int yOffset = yCoordinate * chunkSize;

            for (int x = 0; x < chunkSize; x++) {
                for (int y = 0; y < chunkSize; y++) {
                    float terrainValue = (fs.GetNoise((x + xOffset) * terrainDensity, (y + yOffset) * terrainDensity) + 1) / 2;
                    
                    int dataVal = Mathf.FloorToInt(terrainValue * 4);
                    
                    Data.Set(x, y, dataVal);
                }
            }
        }

        public Chunk(int xCoordinate, int yCoordinate, int chunkSize, int[] data)
        {
            _xCoordinate = xCoordinate;
            _yCoordinate = yCoordinate;
            Data = new Array2D<int>(chunkSize, data);
        }

        public void Set(int x, int y, int value) {
            Data.Set(x, y, value);
        }

        public int Get(int x, int y) {
            return Data.Get(x, y);
        }
    }
}