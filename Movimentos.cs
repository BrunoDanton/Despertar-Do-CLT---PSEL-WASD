using UnityEngine;

public class Movimentos : MonoBehaviour
{
    [Header("Velocidades")]
    public float laneSpeed = 10f;

    [Header("Lanes")]
    public float laneOffset = 3f;   
    private int currentLane = 0;    

    private float initialX;

    private Vector3 targetPosition;

    // Controle por botões da tela
    [HideInInspector] public bool moveLeftPressed = false;
    [HideInInspector] public bool moveRightPressed = false;

    void Start()
    {
        initialX = transform.position.x;
        targetPosition = transform.position;
    }

    void Update()
    {
        // INPUT DO TECLADO
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLeft();

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            MoveRight();

        // INPUT POR BOTÕES (pressionar e segurar)
        if (moveLeftPressed) MoveLeft();
        if (moveRightPressed) MoveRight();

        float targetX = initialX + (currentLane * laneOffset);

        // Calculando a posição alvo da faixa
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        // Mover suavemente até a faixa
        transform.position = Vector3.Lerp(transform.position, targetPosition, laneSpeed * Time.deltaTime);
    }

    public void MoveLeft()
    {
        if (currentLane > -1) 
            currentLane--;
    }

    public void MoveRight()
    {
        if (currentLane < 1) 
            currentLane++;
    }

    // Chamados pela UI
    public void OnLeftButtonDown()  { moveLeftPressed = true; }
    public void OnLeftButtonUp()    { moveLeftPressed = false; }

    public void OnRightButtonDown() { moveRightPressed = true; }
    public void OnRightButtonUp()   { moveRightPressed = false; }
}
