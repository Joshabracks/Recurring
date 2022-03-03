using UnityEngine;
using Gameplay.Player;
using Gameplay.Terrain;
using Gameplay.Data;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gameplay.State
{


    public class StateMachine : MonoBehaviour
    {
        public Text _damageHud;
        public RectTransform _healthBar;
        public RectTransform _armorBar;
        public Character _characterTemplate;
        public Innertube _innertubeTemplate;
        public Balloon _balloonTemplate;
        public Umbrella _umbrellaTemplate;
        public Gun _gunTemplate;
        public Hammer _hammerTemplate;
        public VoiceSet[] _voiceSets;
        public Light sunLight;
        public int drawDistance = 4;
        public WorldController worldController;
        public PlayerController playerController;
        public ExitGate exitGate;
        public QuestIndicatorArrow arrow;
        public CharacterType[] _characterTypes;
        public GameObject _characterContainer;
        public float nightmareIntensity = 0f;
        private float spawnRate = 25;
        public float GameOverCountdown = 5;
        public AudioClip[] musicLibrary;
        public float maxDist = -1;
        private float fadeout = 5;
        private float maxMusicVolume = .025f;
        private bool volumeSet = false;

        private void Start()
        {
            
            exitGate.Place();
            worldController.Initialize();
            playerController.MainCharacter = Instantiate(_characterTemplate, new Vector3(0, .5f, 0), Quaternion.identity);


            Hammer hammer = Instantiate(_hammerTemplate, new Vector3(0, .5f, 0), Quaternion.identity);
            hammer.Randomize();
            hammer.SetCustomizationValues();
            hammer.damage = 4f;

            Gun gun = Instantiate(_gunTemplate, new Vector3(0, .5f, 0), Quaternion.identity);
            gun.Randomize();
            gun.SetCustomizationValues();
            gun.damage = 1f;
            gun.speed = 4f;
            playerController.Initialize();
            _characterContainer = new GameObject();
            CreateCharacterTypes();

            arrow.playerCharacter = playerController.MainCharacter.gameObject;

        }

        void Update()
        {
            _damageHud.text = $"Gun Damage: {playerController.MainCharacter.gunDamageLevel}\nGun Speed: {playerController.MainCharacter.gunSpeedLevel}\nHammer Damage: {playerController.MainCharacter.hammerLevel}";
            float healthLevel = playerController.MainCharacter.Health / playerController.MainCharacter.MaxHealth;
            if (healthLevel < 0) {
                healthLevel = 0;
            }
            _healthBar.transform.localScale = new Vector3(
                healthLevel,
                1, 1
            );
            _healthBar.gameObject.GetComponent<Image>().color = new Color(
                1 - healthLevel,
                healthLevel,
                0
            );

            float armorLevel = 0;
            if (playerController.MainCharacter.gear.innertube != null) {
                armorLevel =  playerController.MainCharacter.gear.innertube.health / playerController.MainCharacter.MaxHealth;
                if (armorLevel < 0) {
                    armorLevel = 0;
                }
            }
            _armorBar.transform.localScale = new Vector3(
                armorLevel,
                1, 1
            );
            
            TurnSun();
            if (Vector3.Distance(exitGate.transform.position, playerController.MainCharacter.transform.position) < 3)
            {
                GameSettings.seed++;
                SceneManager.LoadScene("CreditsScreen");
            }
            if (playerController.MainCharacter.Health <= 0)
            {
                GameOverCountdown -= Time.deltaTime;
                if (GameOverCountdown < 0)
                {
                    GameSettings.seed++;
                    SceneManager.LoadScene("ReminderScreen");
                }
            }
            nightmareIntensity += Time.deltaTime * 0.0025f;
            checkChunks();
            checkCharacters();
            playMusic();
        }

        public void playMusic() {
            if (maxDist == -1) {
                maxDist = Vector3.Distance(playerController.MainCharacter.transform.position, exitGate.transform.position);
            }
            float dist = Vector3.Distance(playerController.MainCharacter.transform.position, exitGate.transform.position);
            int index = Mathf.FloorToInt(Mathf.Lerp(0, musicLibrary.Length, (maxDist - dist) / maxDist));
            AudioClip clip = musicLibrary[index];
            if (clip != null) {
                AudioSource source = GetComponent<AudioSource>();
                if (!volumeSet) {
                    source.volume = maxMusicVolume;
                    volumeSet = true;
                }
                if (GameOverCountdown < 5) {
                    source.volume = Mathf.Lerp(0, maxMusicVolume, GameOverCountdown);
                }
                if (!source.isPlaying) {
                    source.clip = clip;
                    source.Play();
                } else if (clip != source.clip) {
                    if (fadeout > 0) {
                        fadeout -= Time.deltaTime;
                        source.volume = Mathf.Lerp(0, maxMusicVolume, fadeout);
                    } else {
                        source.clip = clip;
                        source.volume = maxMusicVolume;
                        source.Play();
                        fadeout = 5;
                        
                    }
                }
            }
        }

        private float minSpawnDistance()
        {
            // ClipPlanePoints bounds;
            float height = Camera.main.pixelHeight;
            float width = Camera.main.pixelWidth;
            float distance = Vector3.Distance(Camera.main.transform.position, playerController.MainCharacter.transform.position);

            float UpperLeft = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0, height, distance)), playerController.MainCharacter.transform.position);
            float UpperRight = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(width, height, distance)), playerController.MainCharacter.transform.position);
            float LowerLeft = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distance)), playerController.MainCharacter.transform.position);
            float LowerRight = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(width, 0, distance)), playerController.MainCharacter.transform.position);

            float max = UpperLeft > UpperRight ? UpperLeft : UpperRight;
            max = max > LowerLeft ? max : LowerLeft;
            max = max > LowerRight ? max : LowerRight;


            return max + 20;
        }

        private void checkCharacters()
        {
            Character[] _characters = _characterContainer.transform.GetComponentsInChildren<Character>();
            if (_characters.Length < nightmareIntensity * spawnRate)
            {
                int newCharacters = Mathf.CeilToInt(nightmareIntensity * spawnRate) - _characters.Length;
                for (int i = 0; i < newCharacters; i++)
                {
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
                    character.ai._characterContainer = _characterContainer;
                    character.ai.mainCharacter = playerController.MainCharacter;
                    character.ai.aggression = Random.Range(0f, 1f);
                    character.ai.selfPreservation = Random.Range(0f, 1f);
                    character.ai.mood = Random.Range(0f, 1f);
                    character.ai.nightmareIntensity = nightmareIntensity;
                    character.voiceSet = _voiceSets[Random.Range(0, _voiceSets.Length - 1)];
                    character.voicePitch = Random.Range(.8f, 1.2f);
                    float getGun = Random.Range(0, 2);
                    if (getGun > 0)
                    {

                        Gun gun = Instantiate(_gunTemplate, position, Quaternion.identity);
                        gun.Randomize();
                        gun.SetCustomizationValues();
                        gun.damage = Random.Range(0f, nightmareIntensity);
                    }
                    else
                    {
                        Hammer hammer = Instantiate(_hammerTemplate, position, Quaternion.identity);
                        hammer.Randomize();
                        hammer.SetCustomizationValues();
                        hammer.damage = Random.Range(0f, nightmareIntensity * 10);
                    }
                    character.movement = new Vector2(
                        Random.Range(-1f, 1f),
                        Random.Range(-1f, 1f)
                    );
                    character.transform.parent = _characterContainer.transform;

                    character.SetTerrainType();
                    switch (character.terrainType)
                    {
                        case TerrainType.Water:
                            Innertube innertube = Instantiate(_innertubeTemplate, character.transform.position, Quaternion.identity);
                            innertube.Randomize();
                            innertube.SetCustomizationValues();
                            break;
                        case TerrainType.Lava:
                            Umbrella umbrella = Instantiate(_umbrellaTemplate, character.transform.position, Quaternion.identity);
                            umbrella.Randomize();
                            umbrella.SetCustomizationValues();
                            break;
                        case TerrainType.Hole:
                            Balloon balloon = Instantiate(_balloonTemplate, character.transform.position, Quaternion.identity);
                            balloon.Randomize();
                            balloon.SetCustomizationValues();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void CreateCharacterTypes()
        {
            _characterTypes = new CharacterType[10];
            for (int i = 0; i < _characterTypes.Length; i++)
            {
                _characterTypes[i] = new CharacterType(true);
            }
        }

        private void TurnSun()
        {   
            Vector3 d = (exitGate.transform.position - Camera.main.transform.position);
            d.x = Mathf.Clamp(d.x, -30, 30);
            d.z = Mathf.Clamp(d.z, -30, 30);
            Vector3 _direction = d.normalized;

            //create the rotation we need to be in to look at the target
            Quaternion _lookRotation = Quaternion.LookRotation(_direction);
            //rotate us over time according to speed until we are in the required rotation
            sunLight.transform.rotation = _lookRotation;
        }

        private void checkChunks()
        {
            GameObject currentChunkObject = playerController.GetCurrentChunk();
            if (currentChunkObject == null)
            {
                return;
            }
            string[] coordsString = currentChunkObject.name.Split(',');
            Vector2 coords = new Vector2(int.Parse(coordsString[0]), int.Parse(coordsString[1]));
            worldController.initChunk(coords);
            worldController.EnableDisableChunksByDistance(currentChunkObject, drawDistance);
        }
    }

}