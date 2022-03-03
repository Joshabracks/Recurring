using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ReminderScreen : MonoBehaviour
{
    private float timer = 5f;
    public Text text;
    
    string[] tips = new string[]{
        "To wake up, you must reach the portal. \nFollow your shadow...",
        "The red arrow points to the portal and tells you how far you have to go",
        "When the music changes, the nightmare begins",
        "Collect enemy weapons to improve your own",
        "Innertubes are for armor and floating",
        "Drowning hurts",
        "Lava can pop your balloon",
        "Umbrellas block bullets and catch hot to help you over lava. \nBut not at the same time",
        "Hammers hit harder than bullets",
        "The longer your dream, the stronger your enemies",
        "Innertubes will get you over water\nBalloons will get you over the void\nUmbrellas will get you over lava\nBut nothing will save you from quicksand",
        "A balloon will transport your safely over most terrain.  But it will also slow you down."
    };

    private void Start() {
        text.text = tips[Random.Range(0, tips.Length)];
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) {
            SceneManager.LoadScene("Game");
        }
    }
}
