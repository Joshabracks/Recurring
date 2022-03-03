using UnityEngine;
using Gameplay.State;

namespace Gameplay.Player
{

    public class Bullet : MonoBehaviour
    {
        public float life;
        public float damage;
        public AudioClip[] shootNoise;
        public AudioClip[] hitNoise;
        public Character character;

        private void Awake()
        {
            AudioSource source = GetComponent<AudioSource>();
            source.clip = shootNoise[Random.Range(0, shootNoise.Length - 1)];
            randoPitch(1f, 1.1f);
            source.Play();
        }

        private void randoPitch(float a, float b)
        {
            AudioSource source = GetComponent<AudioSource>();
            source.pitch = Random.Range(a, b);
        }

        private void HitSound()
        {
            AudioSource source = GetComponent<AudioSource>();
            source.clip = hitNoise[Random.Range(0, hitNoise.Length - 1)];
            randoPitch(1, 1.1f);
            source.Play();
            damage = 0;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, source.clip.length);
        }
        private void Update()
        {
            if (life > 0)
            {
                transform.Translate(new Vector3(0, 0, Time.deltaTime * 15), Space.Self);
                foreach (Character c in GameObject.Find("StateMachine").GetComponent<StateMachine>()._characterContainer.GetComponentsInChildren<Character>())
                {
                    if (c == character) {
                        continue;
                    }
                    Vector2 center = new Vector2(transform.position.x, transform.position.z);
                    float dist;
                    if (c.gear.umbrella != null)
                    {
                        Vector3 uPos = c.gear.umbrella.GetComponentInChildren<UmbrellaHead>().transform.position;
                        dist = Vector2.Distance(center, new Vector2(uPos.x, uPos.z));
                        if (dist <= 1)
                        {
                            c.gear.umbrella.TakeDamage(damage);
                            
                            HitSound();
                            return;
                        }
                    }

                    dist = Vector2.Distance(center, new Vector2(c.transform.position.x, c.transform.position.z));
                    if (dist <= 1)
                    {
                        if (c.gear.balloon != null) {
                            c.gear.balloon.TakeDamage(damage);
                            HitSound();
                            return;
                        }
                        else if (c.gear.innertube != null)
                        {
                            c.gear.innertube.TakeDamage(damage);
                            
                            HitSound();
                            return;
                        }
                        else
                        {
                            c.TakeDamage(damage);
                            
                            HitSound();
                            return;
                        }
                    }
                }
            }
            else
            {
                life -= Time.deltaTime;
            }
        }
    }

}