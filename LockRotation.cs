using UnityEngine;

public class LockRotation : MonoBehaviour
{
    // VARIÁVEIS PÚBLICAS
    public bool needToAjustHeight = false;
    public float heightAmount;

    // VARIÁVEIS PRIVADAS
    private bool hasAjusted = false;
    private Transform _transform; // Cache do Transform

    void Start()
    {
        // Cache do próprio Transform
        _transform = transform; 
    }

    void Update()
    {
        // Rotação Y em 90 graus: (0, 90, 0).
        // A comparação de Quaternion é mais robusta que Euler Angles (que podem ter representações diferentes)
        if (_transform.rotation != Quaternion.Euler(0, 90, 0))
        {
            _transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        if (needToAjustHeight && !hasAjusted)
        {
            // Otimização: usa o cache e acessa o position diretamente.
            _transform.position += Vector3.up * heightAmount;
            hasAjusted = true;
        }
    }
}