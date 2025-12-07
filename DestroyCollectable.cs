using TMPro;
using UnityEngine;

public class DestroyCollectable : MonoBehaviour
{
    private AudioSource audioSource;
    private MeshRenderer meshRenderer;
    private Collider itemCollider;
    public TextMeshProUGUI textLabel;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Pré-cache dos componentes que serão desativados
        meshRenderer = GetComponent<MeshRenderer>();
        itemCollider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Lógica de efeito do coletável
            if (transform.CompareTag("Contador"))
            {
                // Chamando a função de feedback...
                TempoRestante.incrementaTempo();
                if(FeedbackManager.Instance != null)
                    FeedbackManager.Instance.ShowMessage("+" + TempoRestante.incremento.ToString("N2") + "s!");
            }
            else if (transform.CompareTag("Ovelha"))
            {
                // Chamando a função de feedback...
                ObstacleCollision.canDie = false;
                TempoRestante.MudaVelocidadeTempo(5, 0.5f);
                if(FeedbackManager.Instance != null)
                    FeedbackManager.Instance.ShowMessage("Velocidade do \nContador Reduzida!");
            }
            
            bool hasSound = audioSource != null && audioSource.clip != null;
            
            if (hasSound)
            {
                // 1. Desativa a visualização e colisão imediatamente
                if (meshRenderer != null) meshRenderer.enabled = false; 
                if (itemCollider != null) itemCollider.enabled = false; 

                // Toca o som e destrói após o som terminar
                audioSource.Play();
                
                // Destrói o GameObject após a duração do som
                Destroy(gameObject, audioSource.clip.length);
            }
            else
            {
                // 2. Se não houver som, destrói imediatamente
                Destroy(gameObject);
            }
        }
    }
}
