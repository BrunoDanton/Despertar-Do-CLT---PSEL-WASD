using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TempoRestante : MonoBehaviour
{
    public TextMeshProUGUI textLabel;
    public TextMeshProUGUI textLabel2;
    public ObstacleCollision obstacleCollision;

    // VARIÁVEIS ESTÁTICAS DE CONTROLE
    public static float contador;
    public static float multiplicadorVelTempo = 1f;
    public static float incremento = 2.5f;
    private const float MAX_TIME_CAP = 10f; // Tempo MÁXIMO (10 segundos)
    
    // VARIÁVEIS PRIVADAS DE INSTÂNCIA
    private AudioSource audioSource;
    private bool wereKilled = false;
    private static int oldValue;
    private float tempoTotal = 0;
    private static float tempoPassado = 0f;
    private static float tempoDeDuracao = 0f;
    public float initialDelay = 10f; // Atraso inicial para começar a contagem

    void Start()
    {
        // Garante que o contador estático esteja limpo (bom para restarts)
        contador = MAX_TIME_CAP; 
        oldValue = (int)contador;
        textLabel.text = "Tempo Restante: " + (int)contador + "s";
        textLabel2.text = "Ganhou " + (int)tempoTotal + "s a \nMais de Sono...";
        audioSource = GetComponent<AudioSource>();
        
        // Reset das variáveis estáticas que controlam a velocidade/duração
        multiplicadorVelTempo = 1f;
        tempoPassado = 0f;
        tempoDeDuracao = 0f;
    }

    void Update()
    {
        // Atraso inicial
        if (initialDelay > 0)
        {
            initialDelay -= Time.deltaTime;
            // Não continua se o atraso ainda estiver ativo
            if (initialDelay > 0) return;
        }

        // 1. Duração do efeito de velocidade de tempo
        if (tempoPassado < tempoDeDuracao)
        {
            tempoPassado += Time.deltaTime;
        }
        else if (multiplicadorVelTempo != 1f)
        {
            // Otimização: Aplica o reset só uma vez
            multiplicadorVelTempo = 1f;
            RoadMovement.multiplicador = 1f; // Reseta a velocidade de movimento também
        }
        
        // 2. Contagem regressiva
        if (contador > 0)
        {
            contador -= Time.deltaTime * multiplicadorVelTempo;
            tempoTotal += Time.deltaTime;
        }

        // 3. Efeito sonoro de contagem
        if ((int)contador < oldValue && audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            oldValue = (int)contador;
        }
        
        // 4. Atualização da UI
        textLabel.text = "TEMPO RESTANTE: " + Mathf.Max(0, (int)contador) + "s"; // Garante que não mostra "-1s"
        textLabel2.text = "Ganhou " + (int)tempoTotal + "s a \nMais de Sono...";

        // 5. Condição de Morte
        if (contador <= 0f && !wereKilled)
        {
            wereKilled = true;
            // Garante que a chamada HandleDeath só é feita no ObstacleCollision
            if (obstacleCollision != null) 
            {
                obstacleCollision.HandleDeath();
            }
            else
            {
                // Alternativa: Se o ObstacleCollision estiver no player, procure-o
                Debug.LogError("ObstacleCollision não foi atribuído no Inspector do TempoRestante.");
            }
        }
    }

    public static void incrementaTempo()
    {
        // 1. Adiciona o valor de incremento
        contador += incremento;

        // 2. Limita o contador ao valor máximo
        contador = Mathf.Min(contador, MAX_TIME_CAP);
        
        // 3. Atualiza o oldValue para disparar o som na próxima mudança de segundo
        oldValue = (int) contador;
    }

    public static void MudaVelocidadeTempo(float duracaoEmSegundos, float multiplicador)
    {
        // Reseta o tempo de efeito para o início
        tempoPassado = 0f;
        tempoDeDuracao = duracaoEmSegundos;
        multiplicadorVelTempo = multiplicador;
    }
}
