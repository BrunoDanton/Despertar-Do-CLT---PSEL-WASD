using UnityEngine;

public class Movimentos : MonoBehaviour
{
    [Header("Velocidades")]
    public float forwardSpeed = 10f;
    public float laneSpeed = 10f;

    [Header("Lanes")]
    public float laneOffset = 3f;   // distância entre as faixas
    private int currentLane = 0;    // 0 = centro, -1 = esquerda, +1 = direita

    private Vector3 targetPosition;

    // Controle por botões da tela
    [HideInInspector] public bool moveLeftPressed = false;
    [HideInInspector] public bool moveRightPressed = false;

    void Update()
    {
        // Movimento automático para frente
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // INPUT DO TECLADO
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLeft();

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            MoveRight();

        // INPUT POR BOTÕES (pressionar e segurar)
        if (moveLeftPressed) MoveLeft();
        if (moveRightPressed) MoveRight();

        // Calculando a posição alvo da faixa (lane)
        targetPosition = new Vector3(currentLane * laneOffset, transform.position.y, transform.position.z);

        // Mover suavemente até a faixa
        transform.position = Vector3.Lerp(transform.position, targetPosition, laneSpeed * Time.deltaTime);
    }

    public void MoveLeft()
    {
        if (currentLane > -1) // limite esquerda
            currentLane--;
    }

    public void MoveRight()
    {
        if (currentLane < 1) // limite direita
            currentLane++;
    }

    // Chamados por botões da UI
    public void OnLeftButtonDown()  { moveLeftPressed = true; }
    public void OnLeftButtonUp()    { moveLeftPressed = false; }

    public void OnRightButtonDown() { moveRightPressed = true; }
    public void OnRightButtonUp()   { moveRightPressed = false; }
}