using Gameplay.Data;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Gameplay.Terrain
{
    public struct Chunk
    {
        public Array2D<int> Data {get;}
        private Array2D<float> biomeData;
        private Dictionary<float, List<TerrainType>> biomeOrders;
        private Dictionary<float, TerrainType> biomeBorders;
        public int _xCoordinate;
        public int _yCoordinate;

        public Chunk(int xCoordinate, int yCoordinate, int chunkSize, int worldSeed, float frequency, float terrainDensity)
        {   
            string[] terrainTypeNames = Enum.GetNames(typeof(TerrainType));
            int terrainTypeCount = terrainTypeNames.Length;
            FastNoiseLite fs = new FastNoiseLite();
            fs.SetFrequency(frequency);
            fs.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
            // Instatiate Data and coordiantes

            FastNoiseLite biomeMap = new FastNoiseLite();
            biomeMap.SetSeed(worldSeed);
            biomeMap.SetFrequency(frequency);
            biomeMap.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
            biomeMap.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.Manhattan);
            biomeMap.SetCellularJitter(1.4f); 
            biomeMap.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);
            biomeMap.SetDomainWarpType(FastNoiseLite.DomainWarpType.OpenSimplex2);

            biomeMap.SetDomainWarpAmp(250);
            _xCoordinate = xCoordinate;
            _yCoordinate = yCoordinate;
            Data = new Array2D<int>(chunkSize);
            biomeData = new Array2D<float>(chunkSize);
            biomeOrders = new Dictionary<float, List<TerrainType>>();
            biomeBorders = new Dictionary<float, TerrainType>();
            
            // Build Data
            int xOffset = xCoordinate * chunkSize;
            int yOffset = yCoordinate * chunkSize;

            for (int x = 0; x < chunkSize; x++) {
                for (int y = 0; y < chunkSize; y++) {
                    float biomeValue = (biomeMap.GetNoise((x + xOffset) * terrainDensity, (y + yOffset) * terrainDensity));
                    biomeData.Set(x, y, biomeValue);
                    if (!biomeOrders.ContainsKey(biomeValue)) {
                        List<TerrainType> terrainTypes = new List<TerrainType>();
                        List<int> indices = new List<int>();
                        for (int i = 0; i < terrainTypeCount; i++) {
                            indices.Add(i);
                        }
                        int mult = 2;
                        while (indices.Count > 0) {
                            int index = Mathf.FloorToInt(((biomeMap.GetNoise(x * mult, y * mult) + 1) / 2) * indices.Count);
                            terrainTypes.Add((TerrainType)index);
                            indices.RemoveAt(index);
                            mult++;
                        }
                        biomeOrders[biomeValue] = terrainTypes;
                    }

                    float up = (biomeMap.GetNoise((x + xOffset) * terrainDensity, (y + yOffset + 1) * terrainDensity));
                    float down = (biomeMap.GetNoise((x + xOffset) * terrainDensity, (y + yOffset - 1) * terrainDensity));
                    float left = (biomeMap.GetNoise((x + xOffset - 1) * terrainDensity, (y + yOffset) * terrainDensity));
                    float right = (biomeMap.GetNoise((x + xOffset + 1) * terrainDensity, (y + yOffset) * terrainDensity));
                    if (biomeValue != up || biomeValue != down || biomeValue != left || biomeValue != right) {
                        if (!biomeBorders.ContainsKey(biomeValue)) {
                            string[] bttNames = Enum.GetNames(typeof(BlockingTerrainType));
                            string btt = bttNames[(Mathf.FloorToInt(((biomeMap.GetNoise(x, y) + 1) / 2) * bttNames.Length))];
                            int ttIndex = 0;
                            for (int i = 0; i < terrainTypeNames.Length; i++) {
                                if (btt == terrainTypeNames[i]) {
                                    ttIndex = i;
                                    break;
                                }
                            }
                            biomeBorders[biomeValue] = (TerrainType)ttIndex;
                        }
                        Data.Set(x,y,(int)biomeBorders[biomeValue]);
                    }   
                    else 
                    {


                        fs.SetSeed(Mathf.FloorToInt(biomeValue * worldSeed));
                        float terrainValue = (fs.GetNoise((x + xOffset) * terrainDensity, (y + yOffset) * terrainDensity) + 1) / 2;
                        int dataVal = (int)biomeOrders[biomeValue][Mathf.FloorToInt(terrainValue * terrainTypeCount)];
                        Data.Set(x, y, dataVal);
                    }
                }
            }
        }

        public void Set(int x, int y, int value) {
            Data.Set(x, y, value);
        }

        public int Get(int x, int y) {
            return Data.Get(x, y);
        }
    }
}