using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Data;
public class temp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameSettings settings = new GameSettings();
        settings.Init();
    }
}
