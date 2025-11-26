using UnityEngine;

public class GameSpeedController : MonoBehaviour
{
    void Start()
    {
        RoadMovement.globalVelocity = -10;
    }

    void Update()
    {
        if (RoadMovement.globalVelocity > -30)
        {
            RoadMovement.globalVelocity -= 0.1f * Time.deltaTime; 
        }
    }
}