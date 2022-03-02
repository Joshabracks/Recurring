using UnityEngine;
using UnityEngine.UI;

public class QuestIndicatorArrow : MonoBehaviour
{
    public GameObject target;
    public GameObject playerCharacter;
    public GameObject arrow;
    public Text distance;
    

    // Update is called once per frame
    void Update()
    {
    // Wherever you get these to from
    Vector3 origin = playerCharacter.transform.position;
    Vector3 direction = -(playerCharacter.transform.position - target.transform.position).normalized;

    Ray ray = new Ray(origin, direction);

    float currentMinDistance = Screen.width > Screen.height ? Screen.width : Screen.height;
    Vector3 hitPoint = Vector3.zero;
    Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
    for(var i = 0; i < 4; i++)
    {
        // Raycast against the plane
        if(planes[i].Raycast(ray, out var distance))
        {
            // Since a plane is mathematical infinite
            // what you would want is the one that hits with the shortest ray distance
            if(distance < currentMinDistance)
            {
                hitPoint = ray.GetPoint(distance - .75f);
                currentMinDistance = distance;
            } 
        }
    }  

    // playerCharacter.transform.LookAt(target.transform);
    arrow.transform.position = Camera.main.WorldToScreenPoint(hitPoint);
    distance.transform.position = arrow.transform.position;
    distance.text = Mathf.FloorToInt(Vector3.Distance(playerCharacter.transform.position, target.transform.position)).ToString();
    Vector2 dir = Camera.main.ScreenToWorldPoint(playerCharacter.transform.position);
    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    arrow.transform.rotation = Quaternion.Slerp(arrow.transform.rotation, rotation, 1);
    }
}
