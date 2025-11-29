using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleCollision : MonoBehaviour
{
    [Header("Configurações")]
    public float restartDelay = 0.5f; // Delay antes de reiniciar
    public Movimentos movimentos;
    private int returnQtd;

    void Start()
    {
        movimentos = GetComponent<Movimentos>();
    }

    void Update()
    {
        if (returnQtd != 0 && movimentos.isChangingLanes == false)
        {
            returnQtd = 0;
        }
    }

// ObstacleCollision.cs | OnTriggerEnter()

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (movimentos.isChangingLanes == false)
            {
                // Colisão sem trocar de faixa: Morre imediatamente.
                HandleDeath();
            }
            else if (returnQtd == 0)
            { 
                // 1ª Colisão trocando de faixa: Volta para a faixa anterior.
                Debug.Log("Voltando para a faixa anterior...");
                movimentos.currentLane = movimentos.previousLane;
                returnQtd++; 
            }
            else
            {
                // 2ª Colisão trocando de faixa: Morre.
                Debug.Log("2º Hit. Morte!");
                HandleDeath();
            }
        }
    }

    void HandleDeath()
    {
        // Aqui você pode adicionar efeitos visuais/sonoros
        Debug.Log("Player colidiu com obstáculo!");

        movimentos.enabled = false;

        // Para o movimento do player (se tiver um script de movimento)
        RoadMovement.globalVelocity = 0;

        // Congela o Rigidbody para parar a física
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            // Torna o Rigidbody cinemático para ignorar novas forças de colisão
            rb.isKinematic = true; 
        }

        // DESABILITA o script de colisão para evitar múltiplas chamadas
        // antes que a cena reinicie.
        GetComponent<Collider>().enabled = false;
        this.enabled = false; 

        // REINICIA a cena APÓS o delay
        Invoke("RestartLevel", restartDelay); 
    }

    // NOVA FUNÇÃO
    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}