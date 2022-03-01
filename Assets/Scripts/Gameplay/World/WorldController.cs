using System.Collections.Generic;
using UnityEngine;
using Gameplay.Player;
using Gameplay.Data;

namespace Gameplay.Terrain
{
    public class WorldController : MonoBehaviour
    {
        // templates

        private World _world;
        private Dictionary<Vector2, GameObject> chunkRenders;
        public Material material;
        // [Range(1, 10000)]
        // public int seed = GameSettings.seed;
        [Range(0, 5)]
        public float density = 0.5f;
        [Range(0, .1f)]
        public float frequency = 0.005f;

        public int chunkSize = 32;
        // private int _biomes;

        public int biomeCount() {
            return _world.biomeCount();
        }
        public void Initialize()
        {
            // seed = GameSettings.seed;
            chunkRenders = new Dictionary<Vector2, GameObject>();
            _world = new World(GameSettings.seed, chunkSize, frequency, density);
            initChunk(new Vector2(0, 0));
        }

        public void initChunk(Vector2 key) {
            Vector2[] keysToCheck = new Vector2[] {
                new Vector2(key.x -1, key.y + 1),   new Vector2(key.x, key.y + 1),    new Vector2(key.x + 1, key.y + 1),
                new Vector2(key.x -1, key.y),       key,                              new Vector2(key.x + 1, key.y),
                new Vector2(key.x -1, key.y - 1),   new Vector2(key.x, key.y - 1),    new Vector2(key.x + 1, key.y - 1),
                
            };
            verifyChunks(keysToCheck);
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
                    Vector3 pos = new Vector3(keys[i].x * 2 * chunkSize, 0, keys[i].y * 2 * chunkSize);
                    Mesh mesh = chunkBuilder.BuildChunkMesh(_world, keys[i], new Vector2(pos.x, pos.z));
                    GameObject go = new GameObject();
                    go.transform.parent = gameObject.transform;
                    go.transform.position = pos;
                    
                    MeshRenderer renderer = go.AddComponent<MeshRenderer>();
                    renderer.material = material;
                    MeshFilter filter = go.AddComponent<MeshFilter>();
                    filter.mesh = mesh;
                    go.AddComponent<MeshCollider>();
                    go.name = keys[i].x + "," + keys[i].y;
                    go.tag = "Chunk";
                    chunkRenders[keys[i]] = go;
                    _world.ChunkIsRendered(keys[i]);
                }   
            }
        }
        public void EnableDisableChunksByDistance(GameObject currentChunk, int drawDistance) {
            foreach (KeyValuePair<Vector2, GameObject> pair in chunkRenders) {
                GameObject go = pair.Value;
                if (Vector3.Distance(go.transform.position, currentChunk.transform.position) > chunkSize * drawDistance) {
                    go.SetActive(false);
                } else {
                    go.SetActive(true);
                }
            }
        }
    }

}
