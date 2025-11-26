using UnityEngine;

public class DestroyObstacle : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            Destroy(transform.gameObject);
        }
    }
}
