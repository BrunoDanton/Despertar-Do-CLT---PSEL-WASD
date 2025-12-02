using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TempoRestante : MonoBehaviour
{
    public TextMeshProUGUI textLabel;
    private static float tempoTotal;
    public static float contador;
    private static int oldValue;
    public static float incremento = 2f;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tempoTotal = 0;
        contador = 10;
        oldValue = (int) contador;
        textLabel.text = "Tempo Restante: " + (int) contador + "s";
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        tempoTotal += Time.deltaTime;
        if (tempoTotal >= 10)
            contador -= Time.deltaTime;

        if (oldValue > (int) contador)
        {
            audioSource.Play();
            oldValue = (int) contador;
        }

        textLabel.text = "Tempo Restante: " + (int) contador + "s";

        if (contador <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public static void incrementaTempo()
    {
        contador += incremento;

        if(contador >= 10)
        {
            contador -= contador - 10;
        }
        oldValue = (int) contador;
    }
}
