using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Player;
using Gameplay.Terrain;

namespace Gameplay.State
{
    

    public class StateMachine : MonoBehaviour
    {
        public Character _characterTemplate;
        public Innertube _innertubeTemplate;
        public Balloon _balloonTemplate;
        public Umbrella _umbrellaTemplate;
        public Gun _gunTemplate;
        public Hammer _hammerTemplate;
        public int drawDistance = 4;
        public WorldController worldController;
        public PlayerController playerController;
        public CharacterType[] _characterTypes;
        public GameObject _characterContainer;
        public float nightmareIntensity = .25f;
        private void Start() {
            worldController.Initialize();
            playerController.MainCharacter = Instantiate(_characterTemplate, new Vector3(0, .5f, 0), Quaternion.identity);
            playerController.Initialize();
            _characterContainer = new GameObject();
            CreateCharacterTypes();
        }

        void Update()
        {
            nightmareIntensity += Time.deltaTime * 0.001f;
            checkChunks();
            checkCharacters();
        }

        private float minSpawnDistance() {
            // ClipPlanePoints bounds;
            float height = Camera.main.pixelHeight;
            float width = Camera.main.pixelWidth;
            float distance = Vector3.Distance(Camera.main.transform.position, playerController.MainCharacter.transform.position);

            float UpperLeft = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0,height, distance)), playerController.MainCharacter.transform.position);
            float UpperRight = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(width, height,distance)), playerController.MainCharacter.transform.position);
            float LowerLeft = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0, 0,  distance)), playerController.MainCharacter.transform.position);
            float LowerRight = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(width, 0,  distance)), playerController.MainCharacter.transform.position);

            float max = UpperLeft > UpperRight ? UpperLeft : UpperRight;
            max = max > LowerLeft ? max : LowerLeft;
            max = max > LowerRight ? max : LowerRight;

            return max + 10;
        }

        private void checkCharacters() {
            // Character[] characters = _characterContainer.transform.GetComponentsInChildren<Character>();
            // foreach (Character character in characters) {
            //     if (Vector2.Distance(new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z), new Vector2(character.transform.position.x, character.transform.position.z)) > drawDistance * 30) {
            //         Destroy(character);
            //     }
            // }
            Character[] _characters = _characterContainer.transform.GetComponentsInChildren<Character>();
            if (_characters.Length < nightmareIntensity * 20) {
                int newCharacters = Mathf.CeilToInt(nightmareIntensity * 20) - _characters.Length;
                for (int i = 0; i < newCharacters; i++) {
                    CharacterType type = _characterTypes[Random.Range(0, _characterTypes.Length - 1)];
                    float x = Random.Range(-1f, 1f);
                    float y = Random.Range(-1f, 1f);
                    Vector2 direction = new Vector2(x, y);
                    float distance = Random.Range(minSpawnDistance(), drawDistance * 30);
                    Vector3 position = new Vector3(
                        Camera.main.transform.position.x + direction.x * distance,
                        0.5f,
                        Camera.main.transform.position.z + direction.y * distance
                    );

                    Character character = Instantiate(_characterTemplate, position, Quaternion.identity);
                    character.Randomize(type);
                    character.SetCustomizationValues();
                    character.ai = new AI();
                    character.transform.parent = _characterContainer.transform;
                }
            }
        }

        private void CreateCharacterTypes() {
            _characterTypes = new CharacterType[10];
            for (int i = 0; i < _characterTypes.Length; i++) {
                _characterTypes[i] = new CharacterType(true);
            }
        }



        private void checkChunks() {
            GameObject currentChunkObject = playerController.GetCurrentChunk();
            if (currentChunkObject == null) {
                return;
            }
            string[] coordsString = currentChunkObject.name.Split(',');
            Vector2 coords = new Vector2(int.Parse(coordsString[0]), int.Parse(coordsString[1]));
            worldController.initChunk(coords);
            worldController.EnableDisableChunksByDistance(currentChunkObject, drawDistance);
        }
    }

}