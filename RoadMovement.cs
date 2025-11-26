using UnityEngine;

public class RoadMovement : MonoBehaviour
{
    public static float globalVelocity = -10; 
    public static int direction = 0;

    private void Update()
    {

        if (direction == 0)
        {
            transform.position += new Vector3(0, 0, globalVelocity * Time.deltaTime);
        }
        else if (direction == 1)
        {
            transform.position += new Vector3(globalVelocity * Time.deltaTime, 0, 0);
        }
        else if (direction == 2)
        {
            transform.position += new Vector3(0, 0, -globalVelocity * Time.deltaTime);
        }
        else
        {
            transform.position += new Vector3(-globalVelocity * Time.deltaTime, 0, 0);
        }
    }
}