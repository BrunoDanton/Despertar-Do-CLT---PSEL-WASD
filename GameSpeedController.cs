using UnityEngine;

public class GameSpeedController : MonoBehaviour
{
    private const float MAX_GLOBAL_VELOCITY = -30f; // Velocidade máxima atingível
    private const float MIN_INCREMENT = 1f; // Incremento mínimo de tempo (para evitar valor zero ou negativo)
    
    void Start()
    {
        // Garante que começa com o valor base (bom para restarts)
        RoadMovement.globalVelocity = -10f;
        TempoRestante.incremento = 2.5f;
    }

    void Update()
    {
        // Acelera a globalVelocity (que é negativa, então reduzindo o valor)
        if (RoadMovement.globalVelocity > MAX_GLOBAL_VELOCITY)
        {
            RoadMovement.globalVelocity -= 0.1f * Time.deltaTime; 
        }
        else
        {
            // Otimização: Força o valor máximo para evitar flutuação
            RoadMovement.globalVelocity = MAX_GLOBAL_VELOCITY;
        }

        // Reduz o incremento de tempo (tornando o jogo mais difícil)
        if (TempoRestante.incremento > MIN_INCREMENT)
        {
            TempoRestante.incremento -= 0.01f * Time.deltaTime;
        }
        else
        {
            // Otimização: Força o valor mínimo
            TempoRestante.incremento = MIN_INCREMENT;
        }
    }
}
