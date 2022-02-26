using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Terrain;
using Gameplay.Player;

namespace Gameplay.State
{

    public class AI
    {
        public float aggression;
        public float selfPreservation;
        public float mood;

        public float AttackRadius(float nightmareIntensity)
        {
            return aggression * nightmareIntensity;
        }

        public void move(Character character)
        {

            Vector2 direction = new Vector2(
                character.movement.x,
                character.movement.y
            ).normalized;

            Vector3 blockingPointHover = new Vector3(
                character.transform.position.x + direction.x * 2f,
                character.transform.position.y >= .5f ? character.transform.position.y : .5f,
                character.transform.position.z + direction.y * 2f
            );

            

            Ray ray = new Ray(blockingPointHover, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                int index = hit.triangleIndex * 3;
                MeshCollider mc = hit.collider as MeshCollider;
                if (mc == null)
                {
                    return;
                }
                Mesh mesh = mc.sharedMesh;
                Vector2 uv2 = mesh.uv2[mesh.triangles[index]];
                TerrainType terrainType = (TerrainType)(uv2.x);
                // if (!character.AllowedTerrain.Contains(terrainType)) {
                //     return;
                // }
                character.terrainType = terrainType;
            }
            else
            {
                return;
            }

            if (willMoveHere(character.terrainType, character))
            {
                
                Vector3 _direction = (blockingPointHover - character.transform.position).normalized;
                if (_direction != Vector3.zero) {

                    //create the rotation we need to be in to look at the target
                    Quaternion _lookRotation = Quaternion.LookRotation(_direction);

                    //rotate us over time according to speed until we are in the required rotation
                    character.transform.rotation = _lookRotation;
                }
                character.MakeEquip();
                character.CheckGearModifiers();
                character.CheckTerrainModifiers();
                // floatPlayer();
                character.transform.position = new Vector3(
                    character.transform.position.x + direction.x * character.ModifiedSpeed,
                    character.transform.position.y,
                    character.transform.position.z + direction.y * character.ModifiedSpeed
                );
            } else {
                character.movement.x = Random.Range(-1f, 1f);
                character.movement.y = Random.Range(-1f, 1f);
            }
        }

        public bool willMoveHere(TerrainType terrain, Character character)
        {
            switch (terrain)
            {
                case TerrainType.Sand:
                    return true;
                case TerrainType.Dirt:
                    return true;
                case TerrainType.Grass:
                    return true;
                case TerrainType.Water:
                    if (character.gear.innertube != null || selfPreservation < aggression * mood)
                    {
                        return true;
                    }
                    return false;
                case TerrainType.Hole:
                    if (character.gear.balloon != null || selfPreservation < aggression * mood * 2 + (character.gear.umbrella != null ? 1 : 0))
                    {
                        return true;
                    }
                    return false;
                case TerrainType.Rock:
                    return true;
                case TerrainType.QuickSand:
                    if (selfPreservation < aggression * mood)
                    {
                        return true;
                    }
                    return false;
                case TerrainType.Lava:
                    if (character.gear.umbrella != null || selfPreservation < aggression * mood * 2 + (character.gear.balloon != null ? 1 : 0))
                    {
                        return true;
                    }
                    return false;
            }
            return true;
        }
    }

}