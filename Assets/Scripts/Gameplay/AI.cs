using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Terrain;
using Gameplay.Player;

namespace Gameplay.State
{

    public class AI
    {
        public Character mainCharacter;
        public GameObject _characterContainer;
        public float aggression;
        public float selfPreservation;
        public float mood;
        public float nightmareIntensity;
        // public float attackRadius;
        public float AttackRadius(float aggression)
        {
            // return 10;
            return mood * (aggression + nightmareIntensity) * 100;
        }

        public Weapon ShouldAttack(Character character) {
            if (character.gear.hammer != null)
            {
                if (Vector3.Distance(mainCharacter.transform.position, character.transform.position) < 3)
                {
                    return character.gear.hammer;
                }
            }
            if (character.gear.gun != null)
            {
                if (Vector3.Distance(mainCharacter.transform.position, character.transform.position) < character.gear.gun.range)
                {
                    return character.gear.gun;
                }
            }
            return null;
        }

        public void move(Character character)
        {

            Vector2 direction;
            if (Vector3.Distance(character.transform.position, mainCharacter.transform.position) < AttackRadius(aggression))
            {
                Vector2 difference = new Vector2(mainCharacter.transform.position.x, mainCharacter.transform.position.z) - new Vector2(character.transform.position.x, character.transform.position.z);
                direction = difference;
            }
            else
            {

                direction = new Vector2(
                    character.movement.x,
                    character.movement.y
                ).normalized;
            }

            Vector3 blockingPointHover = new Vector3(
                character.transform.position.x + direction.x * 2f,
                .5f,
                character.transform.position.z + direction.y * 2f
            );

            TerrainType terrainType;

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
                terrainType = (TerrainType)(uv2.x);
                // if (!character.AllowedTerrain.Contains(terrainType)) {
                //     return;
                // }
                // character.terrainType = terrainType;
            }
            else
            {
                return;
            }
            character.SetTerrainType();
            // bool frontBlock = false;
            // ray = new Ray(character.transform.position, character.transform.forward);
            // if (Physics.Raycast(ray, out hit, 1)) {
            //     Character c = hit.collider.gameObject.GetComponent<Character>();
            //     if (c != null) {
            //         frontBlock = true;
            //     }
            // }

            if (willMoveHere(terrainType, character))
            {
                Vector3 _direction = (blockingPointHover - character.transform.position).normalized;
                if (_direction != Vector3.zero)
                {

                    //create the rotation we need to be in to look at the target
                    Quaternion _lookRotation = Quaternion.LookRotation(new Vector3(_direction.x, 0, _direction.z));

                    //rotate us over time according to speed until we are in the required rotation
                    character.transform.rotation = _lookRotation;
                }
                // character.transform.position = new Vector3(
                //     character.transform.position.x + direction.x * character.ModifiedSpeed,
                //     character.transform.position.y,
                //     character.transform.position.z + direction.y * character.ModifiedSpeed
                // );
                // ray = new Ray(character.transform.position, character.transform.right);
                // if (Physics.Raycast(ray, out hit, 3))
                // {
                //     Character c = hit.collider.gameObject.GetComponent<Character>();
                //     if (c != null)
                //     {
                //         rightBlock = .1f;
                //     }
                // }
                // ray = new Ray(character.transform.position, -character.transform.right);
                // if (Physics.Raycast(ray, out hit, 3))
                // {
                //     Character c = hit.collider.gameObject.GetComponent<Character>();
                //     if (c != null)
                //     {
                //         leftBlock = .1f;
                //     }
                // }
                Vector2 blockingDirctions = new Vector2(0, 0);
                Character[] characters = _characterContainer.transform.GetComponentsInChildren<Character>();
                foreach (Character c in characters)
                {
                    blockingDirctions = applyDistance(character, c, blockingDirctions);
                }
                // blockingDirctions = applyDistance(character, mainCharacter, blockingDirctions);
                float forwarMultiplier = applyDistance(character, mainCharacter, blockingDirctions) != Vector2.zero ? 0 : .75f;
                if (blockingDirctions != Vector2.zero)
                {
                    blockingDirctions = blockingDirctions.normalized;
                    character.transform.position = new Vector3(
                        character.transform.position.x + (blockingDirctions.x * character.ModifiedSpeed * .25f),
                        character.transform.position.y,
                        character.transform.position.z + (blockingDirctions.y * character.ModifiedSpeed * .25f)
                    );
                }

                character.transform.Translate(0, 0.0f, character.ModifiedSpeed * forwarMultiplier);

            }
            else
            {
                character.movement.x = Random.Range(-1f, 1f);
                character.movement.y = Random.Range(-1f, 1f);
            }
        }

        private Vector2 applyDistance(Character character, Character c, Vector2 blockingDirections) {
            var dist = Vector3.Distance(c.transform.position, character.transform.position);
            if (dist < 2)
            {
                // Vector2 dir = (new Vector2(c.transform.position.x, c.transform.position.z) - new Vector2(character.transform.position.x, character.transform.position.z)).normalized;

                // blockingDirctions = (blockingDirctions + dir).normalized;
                if (c.transform.position.x < character.transform.position.x)
                {
                    blockingDirections.x = Mathf.Clamp(blockingDirections.x + 1, 0, 1);
                }
                else if (c.transform.position.x > character.transform.position.x)
                {
                    blockingDirections.x = Mathf.Clamp(blockingDirections.x - 1, -1, 0);
                }

                else if (c.transform.position.y < character.transform.position.y)
                {
                    blockingDirections.y = Mathf.Clamp(blockingDirections.y + 1, 0, 1);
                }
                else if (c.transform.position.y > character.transform.position.y)
                {
                    blockingDirections.y = Mathf.Clamp(blockingDirections.y - 1, -1, 0);
                }
            }
            return blockingDirections;
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