using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Terrain
{
    public class WorldController : MonoBehaviour
    {
        private World _world;
        void Start()
        {
            _world = new World();
        }
    }
}
