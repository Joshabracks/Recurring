using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.State;
namespace Gameplay.Player
{

    public class Bullet : MonoBehaviour
    {
        public float life;
        public float damage;
        private void Update()
        {
            if (life > 0)
            {
                transform.Translate(new Vector3(0, 0, Time.deltaTime * 15), Space.Self);
                foreach (Character c in GameObject.Find("StateMachine").GetComponent<StateMachine>()._characterContainer.GetComponentsInChildren<Character>())
                {
                    Vector3 center = transform.position;
                    float dist;
                    if (c.gear.umbrella != null)
                    {
                        dist = Vector3.Distance(center, c.gear.umbrella.GetComponentInChildren<UmbrellaHead>().transform.position);
                        if (dist <= 1)
                        {
                            c.gear.umbrella.TakeDamage(damage);
                            Destroy(gameObject);
                            return;
                        }
                    }
                    dist = Vector3.Distance(center, c.transform.position);
                    if (dist <= 1)
                    {
                        if (c.gear.innertube != null)
                        {
                            c.gear.innertube.TakeDamage(damage);
                            Destroy(gameObject);
                            return;
                        }
                        else
                        {
                            Debug.Log("HIT: " + damage);
                            c.Health -= damage;
                            Destroy(gameObject);
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