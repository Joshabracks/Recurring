using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Terrain
{
    public class WorldController : MonoBehaviour
    {
        private World _world;
        private Dictionary<Vector2, GameObject> chunkRenders;
        void Start()
        {
            chunkRenders = new Dictionary<Vector2, GameObject>();
            _world = new World(1337, 64, .01f, .5f);
            _world.AddChunk(new Vector2(0, 0));
        }

        private void initChunk(Vector2 key) {
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
            for (int i = 0; i < keys.Length; i++) {
                if (!_world.IsChunkRendered(keys[i])) {

                }
            }
        }
    }
}
