using UnityEngine;

public class Movimentos : MonoBehaviour
{
    [Header("Velocidades")]
    public float laneSpeed = 10f;

    [Header("Lanes")]
    public float laneOffset = 3f;   
    public int currentLane = 0;    
    public float maxStepsVolume;
    [HideInInspector] public int previousLane = 0;
    [HideInInspector] public bool isChangingLanes = false;

    // Controle por botões da tela
    [HideInInspector] public bool moveLeftPressed = false;
    [HideInInspector] public bool moveRightPressed = false;

    [Header("Pulo")]
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.2f;
    public Vector3 groundCheckOffset = new Vector3(0, 0.1f, 0); 
    
    private bool isGrounded = false;
    private bool canJump = true; 
    private Rigidbody rb;
    private AudioSource audioSource;
    private Transform _transform; // Cache do Transform

    private float initialX;
    
    // Novas variáveis para a mecânica de descida/recuperação
    [Header("Recuperação")]
    public float recupForce = 15f; // Força aplicada para descer rapidamente

    void Start()
    {
        _transform = transform; // Cache do Transform
        initialX = _transform.position.x;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {     
        
        // inputs
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLeft();

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            MoveRight();

        // PULO
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            Jump();

        // DESCIDA / RECUPERAÇÃO
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            Recuperacao();

        // INPUT POR BOTÕES
        if (moveLeftPressed) MoveLeft();
        if (moveRightPressed) MoveRight();

        float targetX = initialX + (currentLane * laneOffset);
        
        // Otimização: Cria a posição alvo completa
        Vector3 targetPosition = new Vector3(targetX, _transform.position.y, _transform.position.z);

        // Mover suavemente até a faixa
        _transform.position = Vector3.Lerp(_transform.position, targetPosition, laneSpeed * Time.deltaTime);

        // Detecção do chão 
        CheckGround();

        // Otimização: Usa Mathf.Approximately para comparar floats com precisão
        float distanceToTargetX = Mathf.Abs(_transform.position.x - targetX);
        float tolerance = 0.05f;

        // Se o jogador está na posição alvo (dentro da tolerância), a mudança de faixa terminou.
        if (distanceToTargetX <= tolerance) 
        {
            isChangingLanes = false;
            // Otimização: Força a posição X exata para garantir que não haja desvios
            _transform.position = targetPosition; 
        }

        Debug.Log("Is Changing Lanes = " + isChangingLanes);
        // Otimização: Evita desvios de rotação no player.
        if (transform.rotation !=  Quaternion.Euler(new Vector3 (0, 180, 0)))
        {
            transform.rotation = Quaternion.Euler(new Vector3 (0, 180, 0));
        }
    }

    public void MoveLeft()
    {
        // Verifica se já está na faixa mais à esquerda (1)
        if (currentLane > -1) 
        {
            previousLane = currentLane;
            currentLane--;
            isChangingLanes = true;
        }
    }

    public void MoveRight()
    {
        // Verifica se já está na faixa mais à direita (+1)
        if (currentLane < 1) 
        {
            previousLane = currentLane;
            currentLane++;
            isChangingLanes = true;
        }
    }

    // PULO
    public void Jump()
    {
        if (isGrounded && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false; 
            isGrounded = false; 
        }
    }
    
    // Recuperação de altura no pulo
    public void Recuperacao()
    {
        // Aplica uma força para baixo
        rb.AddForce(Vector3.down * recupForce, ForceMode.Impulse);
    }


    // Detecção do chão 
    void CheckGround()
    {
        // Lança o raio do cache do transform
        Vector3 rayOrigin = _transform.position + groundCheckOffset;
        
        // Raycast sem alocação de memória (melhor que criar um 'new Ray')
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hitInfo, groundCheckDistance, groundLayer))
        {
            isGrounded = true;
            audioSource.volume = maxStepsVolume;
            canJump = true; 
        }
        else
        {
            isGrounded = false;
            audioSource.volume = 0;
        }
        
        // Debug.DrawRay(rayOrigin, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }

    // Chamados pela UI
    public void OnLeftButtonDown()  { moveLeftPressed = true; }
    public void OnLeftButtonUp()    { moveLeftPressed = false; }

    public void OnRightButtonDown() { moveRightPressed = true; }
    public void OnRightButtonUp()   { moveRightPressed = false; }
}


