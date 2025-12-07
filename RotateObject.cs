using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("Configuração de Rotação")]
    // Se o objeto deve girar em torno de seu eixo Y (padrão para coletáveis)
    public bool rotateContinuously = true; 
    
    // Velocidade de rotação (graus por segundo)
    public float rotationSpeed = 90f; 

    // O Transform do objeto em cache
    private Transform _transform;

    void Start()
    {
        _transform = transform;
        
        // 1. Rotação Inicial Aleatória
        // Aplica uma rotação aleatória inicial em todos os eixos
        float randomX = Random.Range(0f, 360f);
        float randomY = Random.Range(0f, 360f);
        float randomZ = Random.Range(0f, 360f);

        _transform.rotation = Quaternion.Euler(randomX, randomY, randomZ);
    }

    void Update()
    {
        if (rotateContinuously)
        {
            // 2. Rotação Contínua
            // Rotaciona o objeto em torno do eixo Y local.
            // Time.deltaTime garante que a rotação seja independente da taxa de quadros (FPS).
            _transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}