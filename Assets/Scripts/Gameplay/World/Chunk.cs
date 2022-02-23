using Gameplay.Data;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Gameplay.Terrain
{
    public struct Chunk
    {
        public Array2D<int> Data { get; }
        public int _xCoordinate;
        public int _yCoordinate;

        public Chunk(int xCoordinate, int yCoordinate, int chunkSize, int worldSeed, float frequency, float terrainDensity, float[] biomeData, Dictionary<int, TerrainType> biomeBorders, Dictionary<int, List<TerrainType>> biomeOrders)
        {
            string[] terrainTypeNames = Enum.GetNames(typeof(TerrainType));
            int terrainTypeCount = terrainTypeNames.Length;
            FastNoiseLite fs = new FastNoiseLite();
            fs.SetFrequency(frequency);
            fs.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
            TerrainType[] blockingTerrainTypes = new BlockingTerrainType().Types;
            // Instatiate Data and coordiantes

            FastNoiseLite biomeMap = new FastNoiseLite();
            biomeMap.SetSeed(worldSeed);
            biomeMap.SetFrequency(0.005f);
            biomeMap.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
            biomeMap.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.Manhattan);
            biomeMap.SetCellularJitter(1.4f);
            biomeMap.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);

            FastNoiseLite borderMap = new FastNoiseLite();
            borderMap.SetSeed(worldSeed);
            borderMap.SetFrequency(0.005f);
            borderMap.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
            borderMap.SetCellularJitter(1.4f);
            borderMap.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);
            borderMap.SetCellularReturnType(FastNoiseLite.CellularReturnType.Distance2Div);
            borderMap.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);

            FastNoiseLite biomeWarp = new FastNoiseLite();
            biomeWarp.SetDomainWarpType(FastNoiseLite.DomainWarpType.OpenSimplex2);
            biomeWarp.SetDomainWarpAmp(250);
            biomeWarp.SetSeed(worldSeed);
            biomeWarp.SetFrequency(0.005f);
            biomeWarp.SetFractalType(FastNoiseLite.FractalType.DomainWarpProgressive);
            biomeWarp.SetFractalOctaves(5);
            biomeWarp.SetFractalLacunarity(2);
            biomeWarp.SetFractalGain(.5f);
            
            
            _xCoordinate = xCoordinate;
            _yCoordinate = yCoordinate;
            Data = new Array2D<int>(chunkSize);

            // Build Data
            int xOffset = xCoordinate * chunkSize;
            int yOffset = yCoordinate * chunkSize;

            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    float bx = (x + xOffset) * terrainDensity;
                    float by = (y + yOffset) * terrainDensity;
                    biomeWarp.DomainWarp(ref bx, ref by);

                    float tx = (x + xOffset) * terrainDensity;
                    float ty = (y + yOffset) * terrainDensity;
                    biomeWarp.DomainWarp(ref tx, ref ty);

                    float bv = (biomeMap.GetNoise(bx, by) + 1) / 2;
                    int biomeKey = Mathf.FloorToInt(bv * 20);
                    
                    if (biomeData[biomeKey] == 0) {
                        biomeData[biomeKey] = bv;
                    }
                    float biomeValue = biomeData[biomeKey];
                    if (!biomeOrders.ContainsKey(biomeKey))
                    {
                        List<TerrainType> terrainTypes = new List<TerrainType>();
                        List<int> indices = new List<int>();
                        for (int i = 0; i < terrainTypeCount; i++)
                        {
                            indices.Add(i);
                        }
                        int mult = 2;
                        while (indices.Count > 0)
                        {
                            int index = Mathf.FloorToInt(((biomeMap.GetNoise(bx * mult, by * mult) + 1) / 2) * indices.Count);
                            terrainTypes.Add((TerrainType)index);
                            indices.RemoveAt(index);
                            mult++;
                        }
                        biomeOrders[biomeKey] = terrainTypes;
                    }

                    fs.SetSeed(Mathf.FloorToInt(biomeValue * worldSeed));
                    int dataVal = getCellValue(fs, tx, ty, biomeValue, terrainTypeCount, biomeOrders[biomeKey]);
                    
                    float isBorder = (borderMap.GetNoise(bx, by) + 1) / 2;
                    if (isBorder > .4f)
                    {
                        float percent = ((float)biomeKey / (float)biomeData.Length);
                        int borderType = Mathf.FloorToInt(percent * blockingTerrainTypes.Length);
                        Data.Set(x, y, (int)blockingTerrainTypes[borderType]);
                        
                    }
                    else
                    {
                        Data.Set(x, y, dataVal);
                    }
                }
            }
        }

        public int getCellValue(FastNoiseLite fs, float x, float y, float biomeValue, int terrainTypeCount, List<TerrainType> biomeOrder) 
        {
            float terrainValue = (fs.GetNoise(x, y) + 1) / 2;
            int dataVal = (int)biomeOrder[Mathf.FloorToInt(terrainValue * terrainTypeCount)];
            return dataVal;
        }

        public void Set(int x, int y, int value)
        {
            Data.Set(x, y, value);
        }

        public int Get(int x, int y)
        {
            return Data.Get(x, y);
        }
    }
}