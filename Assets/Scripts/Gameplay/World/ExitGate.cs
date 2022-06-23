using UnityEngine;

public class ExitGate : MonoBehaviour
{
    public void Place()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        Vector2 direction = new Vector2(x, y);
        float distance = 4000;
        transform.position = new Vector3(
            Camera.main.transform.position.x + direction.x * distance,
            0.5f,
            Camera.main.transform.position.z + direction.y * distance
        );
    }
}
