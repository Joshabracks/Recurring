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

        public Chunk GetChunk(Vector2 key) {
            return _chunks[key];
        }

        public bool HasChunk(Vector2 key) {
            return _chunks.ContainsKey(key);
        }
        public int GetCell(Vector2 chunkCoord, Vector2 cellCoord) {
            if (!_chunks.ContainsKey(chunkCoord))
            {
                AddChunk(chunkCoord);
            }
            return _chunks[chunkCoord].Get((int)cellCoord.x, (int)cellCoord.y);
        }

        public bool IsChunkRendered(Vector2 key) {
            return _chunksRendered[key];
        }

        private string chunkKey(int x, int y)
        {
            return $"{x.ToString()}|{y.ToString()}";
        }

        private string chunkKey(Vector2 key) {
            return chunkKey((int)key.x, (int)key.y);
        }
    }
}
