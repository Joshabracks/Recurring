using Gameplay.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Terrain
{
    public struct WorldValues {
        public int Seed;
        public int ChunkSize;
        public float Frequency;
        public float TerrainDensity; 
    }
    public struct World
    {
        private int _seed;
        private int _chunkSize;
        private float _frequency;
        private float _terrainDensity;
        private Dictionary<string, Chunk> _chunks;
        private Dictionary<string, bool> _chunksRendered;
        public World(int seed, int chunkSize, float frequency, float terrainDensity) {
            _chunks = new Dictionary<string, Chunk>();
            _chunksRendered = new Dictionary<string, bool>();
            _seed = seed;
            _chunkSize = chunkSize;
            _frequency = frequency;
            _terrainDensity = terrainDensity;
        }
        public void AddChunk(int x, int y)
        {
            string key = chunkKey(x, y);
            _chunks[key] = new Chunk(x, y, _chunkSize, _seed, _frequency, _terrainDensity);
            _chunksRendered[key] = false;
        }
        
        public Chunk GetChunk(int x, int y) {
            int[] values = indexValues(x, y);
            string key = chunkKey(values[0], values[1]);
            if (_chunks.ContainsKey(key)) {
                return _chunks[chunkKey(values[0], values[1])];
            }
            
            AddChunk(values[0], values[1]);
            return _chunks[key];
        }

        public Chunk GetChunk(Vector2 key) {
            return GetChunk((int)key.x, (int)key.y);
        }

        public byte GetCell(int x, int y) {
            int[] values = indexValues(x, y);
            string key = chunkKey(values[0], values[1]);
            if (!_chunks.ContainsKey(key)) {
                AddChunk(values[0], values[1]);
            }
            return _chunks[key].Get(values[2], values[3]);
        }

        public byte GetCell(Vector2 chunkCoord, Vector2 cellCoord) {
            int x = (int)cellCoord.x + (int)(chunkCoord.x * _chunkSize);
            int y = (int)cellCoord.y + (int)(chunkCoord.y * _chunkSize);
            return GetCell(x, y);
        }

        public void SetCell(int x, int y, byte value) {
            int[] values = indexValues(x, y);
            _chunks[chunkKey(values[0], values[1])].Set(values[2], values[3], value);
        }

        private int[] indexValues(int x, int y) {
            int a = (x % _chunkSize);
            int b = (y % _chunkSize);
            int c = (x - a) / _chunkSize;
            int d = (y - b) / _chunkSize;
            return new int[]{a, b, c, d};
        }

        private string chunkKey(int x, int y)
        {
            return $"{x.ToString()}|{y.ToString()}";
        }

        public void Save()
        {
            FileSystem fs = new FileSystem();
            foreach (KeyValuePair<string, Chunk> entry in _chunks) {
                fs.Save(entry.Value.Data.Raw(), "World", entry.Key, "wda");
            }
            
            fs.SaveJSON<WorldValues>(new WorldValues{
                Seed = _seed,
                ChunkSize = _chunkSize,
                Frequency = _frequency,
                TerrainDensity = _terrainDensity
            });
        }

        public void Load()
        {
            FileSystem fs = new FileSystem();
            string worldValuesPath = $"{Application.dataPath}/Data/WorldValues/-1.json";
            if (fs.Exists(worldValuesPath)) {
                WorldValues worldValues = fs.LoadFromJSON<WorldValues>(worldValuesPath);
                _seed = worldValues.Seed;
                _frequency = worldValues.Frequency;
                _terrainDensity = worldValues.TerrainDensity;
                _chunkSize = worldValues.ChunkSize;
            }
            Dictionary<string, byte[]> chunkData = fs.LoadAllBytes("World", "wda");
            _chunks = new Dictionary<string, Chunk>();
            foreach(KeyValuePair<string, byte[]> entry in chunkData) 
            {
                string[] coords = entry.Key.Split('|');
                int x = int.Parse(coords[0]);
                int y = int.Parse(coords[1]);
                _chunks[entry.Key] = new Chunk(x, y, _chunkSize, entry.Value);
            }
        }
    }
}
