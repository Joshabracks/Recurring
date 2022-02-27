using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReminderScreen : MonoBehaviour
{
    private float timer = 5f;
    

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) {
            SceneManager.LoadScene("Game");
        }
    }
}
