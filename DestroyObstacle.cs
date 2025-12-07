using UnityEngine;

public class DestroyObstacle : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            // Destrói o objeto no qual este script está anexado
            Destroy(gameObject); 
        }
    }
}
