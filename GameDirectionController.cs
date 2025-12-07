using UnityEngine;

public class GameDirectionController : MonoBehaviour
{
    public static int buildedRoads;
    private float changePossibility;

    void Start()
    {
        buildedRoads = 0;
        changePossibility = 0f;
    }

    void Update()
    {
        // Otimização: Verificar as condições maiores PRIMEIRO
        if (buildedRoads > 8)
        {
            changePossibility = 0.5f;
        }
        else if (buildedRoads > 5)
        {
            changePossibility = 0.35f;
        }
        else if (buildedRoads > 4)
        {
            changePossibility = 0.3f;
        }
        else if (buildedRoads > 3)
        {
            changePossibility = 0.2f;
        }
        else
        {
            changePossibility = 0f;
        }

        // Lógica de sorteio para mudança de direção
        if (Random.value < changePossibility) // Random.value é Random.Range(0f, 1f)
        {
            // Sorteia um novo valor entre 0 e 3 (os 4 eixos)
            int newDirection = Random.Range(0, 4);

            // Garante que o novo valor não seja o inverso do atual (evitar virar e voltar imediatamente)
            // Os opostos são: (current + 2) % 4
            while (newDirection == RoadMovement.direction || newDirection == (RoadMovement.direction + 2) % 4)
            {
                newDirection = Random.Range(0, 4);
            }

            RoadMovement.direction = newDirection;
            
            // Opcional: Você pode querer resetar buildedRoads aqui para começar a contagem de novo
            // buildedRoads = 0;
        }
    }
}
