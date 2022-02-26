using UnityEngine;
using Gameplay.Terrain;

namespace  Gameplay.Player 
{
    
    public class PlayerController : MonoBehaviour
    {
        public Character MainCharacter;

        private float blockingPointHoverHeight = 0.5f;

        public void Initialize() {
            MainCharacter.Randomize();
            MainCharacter.SetCustomizationValues();
        }

        void Update() {
            setMove();
            pickupStuff();
        }

        void FixedUpdate() {
            MainCharacter.MakeEquip();
            MainCharacter.CheckGearModifiers();
            MainCharacter.CheckTerrainModifiers();
            MainCharacter.Float();
            if (MainCharacter.Health <= 0) {
                return; 
            }
            movePlayer();
            turnPlayer();
        }

        void pickupStuff() {
            GameObject[] allGear = GameObject.FindGameObjectsWithTag("Gear");
            foreach (GameObject go in allGear) {
                if (Vector2.Distance(new Vector2(go.transform.position.x, go.transform.position.z), new Vector2(MainCharacter.transform.position.x, MainCharacter.transform.position.z)) > 2) {
                    continue;
                }
                Gear g = go.GetComponent<Gear>();
                if (g != null && g.equippedCharacter != MainCharacter) {
                    g.equippedCharacter = MainCharacter;
                    g.PickUp(MainCharacter);
                }
            }
        }
        
        

        

        public void turnPlayer() {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit)) 
            {
                Vector3 Target = hit.point;
                Target.y = MainCharacter.transform.position.y;
                //find the vector pointing from our position to the target
                Vector3 _direction = (Target - MainCharacter.transform.position).normalized;

                //create the rotation we need to be in to look at the target
                Quaternion _lookRotation = Quaternion.LookRotation(_direction);

                //rotate us over time according to speed until we are in the required rotation
                MainCharacter.transform.rotation = _lookRotation;
            }    
        }

        public GameObject GetCurrentChunk() {
            Vector2 direction = new Vector2(
                MainCharacter.movement.x,
                MainCharacter.movement.y
            ).normalized;
            
            Vector3 blockingPointHover = new Vector3(
                MainCharacter.transform.position.x + direction.x * 1f,
                blockingPointHoverHeight,
                MainCharacter.transform.position.z + direction.y * 1f
            );
            Ray ray = new Ray(blockingPointHover, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) 
            {
                GameObject go = hit.collider.gameObject;
                if (go.tag == "Chunk") {
                    return go;
                }
            }
            return null;
        }

        

        public void movePlayer() {

            Vector2 direction = new Vector2(
                MainCharacter.movement.x,
                MainCharacter.movement.y
            ).normalized;
            
            Vector3 blockingPointHover = new Vector3(
                MainCharacter.transform.position.x + direction.x * 1f,
                MainCharacter.transform.position.y >= .5f ? MainCharacter.transform.position.y : .5f ,
                MainCharacter.transform.position.z + direction.y * 1f
            );
            Ray ray = new Ray(blockingPointHover, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) 
            {
                int index = hit.triangleIndex * 3;
                MeshCollider mc = hit.collider as MeshCollider;
                if (mc == null) {
                    return;
                }
                Mesh mesh = mc.sharedMesh;
                Vector2 uv2 = mesh.uv2[mesh.triangles[index]];
                TerrainType terrainType = (TerrainType)(uv2.x);
                // if (!MainCharacter.AllowedTerrain.Contains(terrainType)) {
                //     return;
                // }
                MainCharacter.terrainType = terrainType;
            }
            else 
            {
                return;
            }

            MainCharacter.transform.position = new Vector3(
                MainCharacter.transform.position.x + direction.x * MainCharacter.ModifiedSpeed,
                MainCharacter.transform.position.y,
                MainCharacter.transform.position.z + direction.y * MainCharacter.ModifiedSpeed
            );

            Camera.main.transform.position = new Vector3(
                MainCharacter.transform.position.x,
                Camera.main.transform.position.y,
                MainCharacter.transform.position.z
            );
        }

        public void setMove() {
            int y = 0;
            int x = 0;
            KeyCode[] up = new KeyCode[]{KeyCode.W, KeyCode.UpArrow};
            KeyCode[] down = new KeyCode[]{KeyCode.S, KeyCode.DownArrow};
            KeyCode[] left = new KeyCode[]{KeyCode.A, KeyCode.LeftArrow};
            KeyCode[] right = new KeyCode[]{KeyCode.D, KeyCode.RightArrow};

            if (isPressed(up)) {
                y++;
            }
            if (isPressed(down)) {
                y--;
            }
            if (isPressed(left)) {
                x--;
            }
            if (isPressed(right)) {
                x++;
            }
            MainCharacter.movement = new Vector2(x, y);
        }

        private bool isPressed(KeyCode[] keyCodes) 
        {
            foreach ( KeyCode code in keyCodes) {
                if (Input.GetKey(code)) {
                    return true;
                }
            }
            return false;
        }


        
    }
}
