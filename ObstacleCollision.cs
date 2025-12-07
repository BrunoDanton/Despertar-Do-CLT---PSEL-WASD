using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleCollision : MonoBehaviour
{
    [Header("Configurações")]
    public float restartDelay = 0.5f; 
    public Movimentos movimentos; // Deve ser atribuído no Inspector
    public AudioClip deathSound;
    public AudioClip collisionSound;
    public float invulnerabilityDuration = 5f; // Duração da invulnerabilidade após ovelha

    private AudioSource audioSource;
    private Rigidbody rb;
    private Collider playerCollider; // Cache do Collider do player

    // VARIÁVEIS ESTÁTICAS DE CONTROLE
    public static bool canDie = true; // Deve começar como TRUE
    
    // VARIÁVEIS PRIVADAS DE INSTÂNCIA
    private float timeSinceInvulnerable = 0f; // Tempo desde que canDie foi setado para false
    private float timeSinceLastHit = 0f;
    private int timesCollided = 0; // Removido do Update, mantido apenas na colisão

    void Start()
    {
        // Cache de Componentes
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        
        // Reset no início da cena
        canDie = true; 
        timeSinceInvulnerable = invulnerabilityDuration; // Inicia com o máximo para começar morrendo
    }

    void Update()
    {
        // Se está invulnerável
        if (!canDie)
        {
            timeSinceInvulnerable += Time.deltaTime;
            
            if (timeSinceInvulnerable >= invulnerabilityDuration)
            {
                canDie = true;
            }
        }
        else
        {
            // Se pode morrer, garante que o contador é mantido no máximo
            timeSinceInvulnerable = invulnerabilityDuration;
        }

        if (timeSinceLastHit >= 2 && timesCollided == 1)
        {
            timesCollided = 0;
            timeSinceLastHit = 0;
        }

        if (timesCollided == 1)
        {
            timeSinceLastHit += Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (canDie)
            {
                if (movimentos != null && movimentos.isChangingLanes == false)
                {
                    // Colisão sem trocar de faixa: Morre
                    HandleDeath();
                }
                else if (movimentos != null && timesCollided == 0)
                { 
                    if (other.TryGetComponent<DetectObjectRoad>(out DetectObjectRoad roadInfo))
                    {
                        if(movimentos.previousLane != roadInfo.actualLane)
                        {
                            movimentos.currentLane = movimentos.previousLane;
                            timesCollided = 1; // Incrementa para o próximo hit ser morte
                            TempoRestante.contador -= 1;
                            FeedbackManager.Instance.ShowMessage("-1s! :(");
                            audioSource.PlayOneShot(collisionSound);
                        }
                        else
                        {
                            HandleDeath();
                        }
                    }
                    else
                    {
                        // Se o obstáculo não tiver o script (por erro), mata o player para não travar o jogo
                        HandleDeath();
                    }
                }
                else
                {
                    // 2ª Colisão trocando de faixa: Morre
                    HandleDeath();
                }
            }
            else
            {
                Destroy(other.gameObject);
            }
            
        }
    }

    public void HandleDeath()
    {
        if (canDie)
        {
            canDie = false; 

            // Desabilita os movimentos imediatamente
            if (movimentos != null) movimentos.enabled = false;
            RoadMovement.globalVelocity = 0;

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true; 
            }

            if (playerCollider != null) playerCollider.enabled = false;

            StartCoroutine(DeathSequence());
        }
    }

    // Corrotina para gerenciar o som e o reload
    IEnumerator DeathSequence()
    {
        // 1. Toca o som de morte
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound, 1);
        }

        FeedbackManager.Instance.BigAdvice("Acordando...");

        // 2. Define quanto tempo esperar
        // Se houver som, espera a duração do som. Se não, usa o delay padrão.
        // O Mathf.Max garante que esperamos pelo menos o restartDelay, caso o som seja muito curto.
        float tempoDeEspera = (deathSound != null) ? deathSound.length : restartDelay;
        
        yield return new WaitForSeconds(tempoDeEspera);

        // 3. Reinicia a cena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
