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
        private Dictionary<Vector2, Chunk> _chunks;
        private Dictionary<Vector2, bool> _chunksRendered;
        public World(int seed, int chunkSize, float frequency, float terrainDensity) {
            _chunks = new Dictionary<Vector2, Chunk>();
            _chunksRendered = new Dictionary<Vector2, bool>();
            _seed = seed;
            _chunkSize = chunkSize;
            _frequency = frequency;
            _terrainDensity = terrainDensity;
        }
        public void AddChunk(Vector2 key)
        {
            _chunks[key] = new Chunk((int)key.x, (int)key.y, _chunkSize, _seed, _frequency, _terrainDensity);
            _chunksRendered[key] = false;
        }

        public void ChunkIsRendered(Vector2 key) {
            _chunksRendered[key] = true;
        }
        
        // public Chunk GetChunk(int x, int y) {
        //     int[] values = indexValues(x, y);
        //     Vector2 key = new Vector2(values[0], values[1]);
        //     if (_chunks.ContainsKey(key)) {
        //         return _chunks[new Vector2(values[0], values[1])];
        //     }
            
        //     AddChunk(new Vector2(values[0], values[1]));
        //     return _chunks[key];
        // }

        public Chunk GetChunk(Vector2 key) {
            return _chunks[key];
            // return GetChunk((int)key.x, (int)key.y);
        }

        // public int GetCell(int x, int y) {
        //     int[] values = indexValues(x, y);
        //     Vector2 key = new Vector2(values[0], values[1]);
        //     if (!_chunks.ContainsKey(key)) {
        //         AddChunk(key);
        //     }
        //     Debug.Log($"Set Cell ({x},{y}) --> [{values[0]},{values[1]}][{values[2]},{values[3]}]");

        //     return _chunks[key].Get(values[2], values[3]);
        // }

        public bool HasChunk(Vector2 key) {
            return _chunks.ContainsKey(key);
        }
        public int GetCell(Vector2 chunkCoord, Vector2 cellCoord) {
            if (!_chunks.ContainsKey(chunkCoord))
            {
                AddChunk(chunkCoord);
            }
            return _chunks[chunkCoord].Get((int)cellCoord.x, (int)cellCoord.y);
            // int x = (int)cellCoord.x + (int)(chunkCoord.x * _chunkSize);
            // int y = (int)cellCoord.y + (int)(chunkCoord.y * _chunkSize);
            // return GetCell(x, y);
        }

        public bool IsChunkRendered(Vector2 key) {
            return _chunksRendered[key];
        }

        // public void SetCell(int x, int y, int value) {
        //     int[] values = indexValues(x, y);
        //     _chunks[new Vector2(values[0], values[1])].Set(values[2], values[3], value);
        // }

        // private int[] indexValues(int x, int y) {
        //     // var xDir = x < 0 ? -1 : 1;
        //     // var yDir = y < 0 ? -1 : 1;
        //     int _x = (Mathf.Abs(x) - (Mathf.Abs(x) % _chunkSize));
        //     int _y = (Mathf.Abs(y) - (Mathf.Abs(y) % _chunkSize));
        //     int a = _x / _chunkSize * ((x > 0) ? 1 : -1);
        //     int b = _y / _chunkSize * ((y > 0) ? 1 : -1);
        //     int c = x >= 0 ? _x : _chunkSize - _x;
        //     int d = y >= 0 ? _y : _chunkSize - _y;
        //     if (c == _chunkSize) {
        //         c = 0;
        //     }
        //     if (d == _chunkSize) {
        //         d = 0;
        //     }
        //     // int c = (x - a) / _chunkSize;
        //     // if (c < 0) 
        //     // {
        //     //     c += _chunkSize;
        //     // }
            
        //     // int d = (y - b) / _chunkSize;
        //     // if (d < 0) {
        //     //     d += _chunkSize;
        //     // }
        //     return new int[]{a, b, c, d};
        // }

        private string chunkKey(int x, int y)
        {
            return $"{x.ToString()}|{y.ToString()}";
        }

        private string chunkKey(Vector2 key) {
            return chunkKey((int)key.x, (int)key.y);
        }

        // public void Save()
        // {
        //     FileSystem fs = new FileSystem();
        //     foreach (KeyValuePair<Vector2, Chunk> entry in _chunks) {
        //         fs.Save(entry.Value.Data.Raw(), "World", chunkKey(entry.Key), "wda");
        //     }
            
        //     fs.SaveJSON<WorldValues>(new WorldValues{
        //         Seed = _seed,
        //         ChunkSize = _chunkSize,
        //         Frequency = _frequency,
        //         TerrainDensity = _terrainDensity
        //     });
        // }

        // public void Load()
        // {
        //     FileSystem fs = new FileSystem();
        //     string worldValuesPath = $"{Application.dataPath}/Data/WorldValues/-1.json";
        //     if (fs.Exists(worldValuesPath)) {
        //         WorldValues worldValues = fs.LoadFromJSON<WorldValues>(worldValuesPath);
        //         _seed = worldValues.Seed;
        //         _frequency = worldValues.Frequency;
        //         _terrainDensity = worldValues.TerrainDensity;
        //         _chunkSize = worldValues.ChunkSize;
        //     }
        //     Dictionary<string, int[]> chunkData = fs.LoadAllBytes("World", "wda");
        //     _chunks = new Dictionary<Vector2, Chunk>();
        //     foreach(KeyValuePair<string, int[]> entry in chunkData) 
        //     {
        //         string[] coords = entry.Key.Split('|');
        //         int x = int.Parse(coords[0]);
        //         int y = int.Parse(coords[1]);
        //         _chunks[new Vector2(x, y)] = new Chunk(x, y, _chunkSize, entry.Value);
        //     }
        // }
    }
}
