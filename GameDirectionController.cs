using JetBrains.Annotations;
using UnityEngine;

public class GameDirectionController : MonoBehaviour
{
    public static int buildedRoads;
    private float changePossibility;
    void Start()
    {
        buildedRoads = 0;
        changePossibility = 0;
    }

    void Update()
    {
        if (buildedRoads > 3)
        {
            changePossibility = 0.2f;
        }

        else if (buildedRoads > 4)
        {
            changePossibility = 0.3f;
        }
        else if (buildedRoads > 5)
        {
            changePossibility = 0.35f;
        }
        else if (buildedRoads > 8)
        {
            changePossibility = 0.5f;
        }

        if (Random.Range(0f,1f) < changePossibility)
        {
            RoadMovement.direction += (RoadMovement.direction != 3)
            ? Random.Range(RoadMovement.direction - 1, RoadMovement.direction + 1)
            : (RoadMovement.direction != 1)
            ? Random.Range(RoadMovement.direction - 1, RoadMovement.direction - 3)
            : Random.Range(RoadMovement.direction + 3, RoadMovement.direction + 1); 
        }
    }
}