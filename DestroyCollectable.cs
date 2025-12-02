using UnityEngine;

public class DestroyCollectable : MonoBehaviour
{
    void Update()
    {
        float rotationSpeed = 90f; // 90 graus por segundo
        transform.Rotate(Vector3.one * rotationSpeed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TempoRestante.incrementaTempo();
            Destroy(transform.gameObject);
        }
    }
}
