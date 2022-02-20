using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Terrain
{
    public class WorldController : MonoBehaviour
    {
        private World _world;
        private Dictionary<Vector2, GameObject> chunkRenders;
        public Material material;
        [Range(1, 10000)]
        public int seed = 1337;
        [Range(0, 5)]
        public float density = 0.5f;
        [Range(0, .1f)]
        public float frequency = 0.005f;

        public int chunkSize = 32;
        void Start()
        {
            chunkRenders = new Dictionary<Vector2, GameObject>();
            _world = new World(seed, chunkSize, frequency, density);
            initChunk(new Vector2(0, 0));
        }

        private void initChunk(Vector2 key) {
            Debug.Log("INIT CHUNKS");
            Vector2[] keysToCheck = new Vector2[] {
                new Vector2(key.x -1, key.y + 1),   new Vector2(key.x, key.y + 1),    new Vector2(key.x + 1, key.y + 1),
                new Vector2(key.x -1, key.y),       key,                              new Vector2(key.x + 1, key.y),
                new Vector2(key.x -1, key.y - 1),   new Vector2(key.x, key.y - 1),    new Vector2(key.x + 1, key.y - 1),
                
            };
            Debug.Log("VERIFY CHUNKS");
            verifyChunks(keysToCheck);
            Debug.Log("RENDER CHUNKS");
            renderChunks(keysToCheck);
        }

        private void verifyChunks(Vector2[] keysToCheck) {
            
            for (int i = 0; i < keysToCheck.Length; i++) {
                if (!_world.HasChunk(keysToCheck[i])) {
                    _world.AddChunk(keysToCheck[i]);
                }
            }
        }

        private void renderChunks(Vector2[] keys) {
            ChunkBuilder chunkBuilder = new ChunkBuilder();
            for (int i = 0; i < keys.Length; i++) {
                if (!_world.IsChunkRendered(keys[i])) {
                    Mesh mesh = chunkBuilder.BuildChunkMesh(_world, keys[i]);
                    GameObject go = new GameObject();
                    go.transform.parent = gameObject.transform;
                    go.transform.position = new Vector3(keys[i].x * 2 * chunkSize, 0, keys[i].y * 2 * chunkSize);
                    
                    MeshRenderer renderer = go.AddComponent<MeshRenderer>();
                    renderer.material = material;
                    MeshFilter filter = go.AddComponent<MeshFilter>();
                    filter.mesh = mesh;
                    chunkRenders[keys[i]] = go;
                }   
            }
        }
    }
}
