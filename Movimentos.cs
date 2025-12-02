using UnityEngine;

public class Movimentos : MonoBehaviour
{
    [Header("Velocidades")]
    public float laneSpeed = 10f;

    [Header("Lanes")]
    public float laneOffset = 3f;   
    public int currentLane = 0;    
    public int previousLane = 0;
    public bool isChangingLanes = false;

    private float initialX;

    private Vector3 targetPosition;

    // Controle por botões da tela
    [HideInInspector] public bool moveLeftPressed = false;
    [HideInInspector] public bool moveRightPressed = false;

    [Header("Pulo")]
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.2f; // Distância menor e ajustável
    public Vector3 groundCheckOffset = new Vector3(0, 0.1f, 0); // Offset do ponto de origem do raycast
    
    private bool isGrounded = false;
    private bool canJump = true; // Nova variável para controlar pulo duplo
    private Rigidbody rb;
    private AudioSource audioSource;

    void Start()
    {
        initialX = transform.position.x;
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {     
        // INPUT DO TECLADO
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLeft();

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            MoveRight();

        //PULO
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            Jump();

        // INPUT POR BOTÕES (pressionar e segurar)
        if (moveLeftPressed) MoveLeft();
        if (moveRightPressed) MoveRight();

        float targetX = initialX + (currentLane * laneOffset);

        // Calculando a posição alvo da faixa
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        // Mover suavemente até a faixa
        transform.position = Vector3.Lerp(transform.position, targetPosition, laneSpeed * Time.deltaTime);

        //Detecção do chão 
        CheckGround();

        float distanceToTarget = Mathf.Abs(transform.position.x - targetX);
    
        // Se a distância for maior que a tolerância (ex: 0.01), ainda está mudando.
        // Use um valor pequeno como 0.01f ou Vector3.kEpsilon.
        if (distanceToTarget > 0.01f) 
        {
            isChangingLanes = true;
        }
        else
        {
            isChangingLanes = false;
            // Opcional: Força a posição exata para evitar qualquer erro de float
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        }
    }

    public void MoveLeft()
    {
        previousLane = currentLane;
        if (currentLane > -1) 
            currentLane--;
    }

    public void MoveRight()
    {
        previousLane = currentLane;
        if (currentLane < 1) 
            currentLane++;
    }

    //PULO
    public void Jump()
    {
        if (isGrounded && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false; // Impede pulo duplo
            isGrounded = false; // Força o estado de não estar no chão
        }
    }

    //Detecção do chão 
    void CheckGround()
    {
        // Lança o raio de um ponto ligeiramente acima do centro do personagem
        Vector3 rayOrigin = transform.position + groundCheckOffset;
        Ray ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, groundCheckDistance, groundLayer))
        {
            isGrounded = true;
            audioSource.volume = 1;
            canJump = true; // Permite pular novamente quando tocar o chão
        }
        else
        {
            isGrounded = false;
            audioSource.volume = 0;
        }
        
        // Visualizar o raio no Editor (opcional, remova em produção)
        Debug.DrawRay(rayOrigin, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }

    // Chamados pela UI
    public void OnLeftButtonDown()  { moveLeftPressed = true; }
    public void OnLeftButtonUp()    { moveLeftPressed = false; }

    public void OnRightButtonDown() { moveRightPressed = true; }
    public void OnRightButtonUp()   { moveRightPressed = false; }
}
