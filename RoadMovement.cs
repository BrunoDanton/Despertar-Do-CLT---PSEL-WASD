using UnityEngine;

public class RoadMovement : MonoBehaviour
{
    // VARIÁVEIS ESTÁTICAS GLOBAIS
    public static float globalVelocity = -10; 
    public static int direction = 0; // 0: Z Positivo (Default) | 1: X Positivo | 2: Z Negativo | 3: X Negativo
    public static float multiplicador = 1;

    private void Update()
    {
        float movementAmount = globalVelocity * Time.deltaTime * multiplicador;

        switch (direction)
        {
            case 0: // Movimento Padrão (Frente/Z positivo)
                transform.position += new Vector3(0, 0, movementAmount);
                break;
            case 1: // Virar para X Positivo
                transform.position += new Vector3(movementAmount, 0, 0);
                break;
            case 2: // Virar para Z Negativo (Volta, se globalVelocity for negativo)
                transform.position += new Vector3(0, 0, -movementAmount);
                break;
            case 3: // Virar para X Negativo
                transform.position += new Vector3(-movementAmount, 0, 0);
                break;
        }
    }
}
