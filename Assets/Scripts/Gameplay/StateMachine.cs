using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Player;
using Gameplay.Terrain;

namespace Gameplay.State
{
    

    public class StateMachine : MonoBehaviour
    {
        public WorldController worldController;
        public PlayerController playerController;

        void Update()
        {
            checkChunks();
        }

        private void checkChunks() {
            GameObject currentChunkObject = playerController.GetCurrentChunk();
            string[] coordsString = currentChunkObject.name.Split(',');
            Vector2 coords = new Vector2(int.Parse(coordsString[0]), int.Parse(coordsString[1]));
            worldController.initChunk(coords);
        }
    }

}