using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;

    [Header("Referências")]
    public TextMeshProUGUI textLabel;
    public Image bigAdvicePanel;

    [Header("Configurações Gerais")]
    public float fadeDuration = 0.5f;
    public float displayTime = 2.0f;
    
    [Header("Configuração: Texto Simples")]
    public float NormalFontSize = 36f;
    public Vector2 NormalBoxSize = new Vector2(600f, 100f);
    public Vector2 defaultMsgPosition = Vector2.zero;

    [Header("Configuração: Big Advice (Morte)")]
    public float BigAdviceSize = 65f;
    public Vector2 BigAdviceBoxSize = new Vector2(1200f, 150f);

    private bool isBigAdvice = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Start da Cena
        isBigAdvice = true;
        SetSpecificAlpha(panelAlpha: 1f, textAlpha: 0f);
        StartCoroutine(StartSceneSequence());
    }

    public void ShowMessage(string message, Vector2? position = null, Color? color = null)
    {
        isBigAdvice = false; 

        // Configura tamanho
        textLabel.enableAutoSizing = true;
        textLabel.fontSize = NormalFontSize;
        textLabel.rectTransform.sizeDelta = NormalBoxSize;

        // Se foi passada uma posição customizada, usa ela. Se não, usa a padrão do Inspector.
        if (position.HasValue)
        {
            textLabel.rectTransform.anchoredPosition = position.Value;
        }
        else
        {
            textLabel.rectTransform.anchoredPosition = defaultMsgPosition;
        }

        // Configura texto e cor
        if (color.HasValue) textLabel.color = color.Value;
        textLabel.text = message;

        StopAllCoroutines();
        StartCoroutine(FadeSequence(persistPanel: false));
    }

    public void BigAdvice(string message, Color? color = null)
    {
        isBigAdvice = true; 
        
        // BigAdvice é sempre centralizado e grande
        textLabel.enableAutoSizing = false;
        textLabel.fontSize = BigAdviceSize;
        textLabel.rectTransform.sizeDelta = BigAdviceBoxSize;
        textLabel.rectTransform.anchoredPosition = Vector2.zero;

        if (color.HasValue) textLabel.color = color.Value;
        textLabel.text = message;

        StopAllCoroutines();
        // persistPanel = true (Fundo fica, texto some)
        StartCoroutine(FadeSequence(persistPanel: true));
    }

    IEnumerator FadeSequence(bool persistPanel)
    {
        float time = 0;

        // Fade in
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, time / fadeDuration);
            SetSpecificAlpha(panelAlpha: isBigAdvice ? alpha : 0, textAlpha: alpha);
            yield return null;
        }
        SetSpecificAlpha(panelAlpha: isBigAdvice ? 1 : 0, textAlpha: 1);

        // Espera
        yield return new WaitForSeconds(displayTime);

        // Fade out
        time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alphaOut = Mathf.Lerp(1, 0, time / fadeDuration);

            if (persistPanel)
            {
                // Painel continua (1), Texto some (alphaOut)
                SetSpecificAlpha(panelAlpha: 1f, textAlpha: alphaOut);
            }
            else
            {
                // Tudo some
                SetSpecificAlpha(panelAlpha: 0f, textAlpha: alphaOut);
            }
            yield return null;
        }

        if (persistPanel) SetSpecificAlpha(1f, 0f); // Painel Fica
        else SetSpecificAlpha(0f, 0f); // Tudo Some
    }

    IEnumerator StartSceneSequence()
    {
        yield return new WaitForSeconds(0.2f);
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, time / fadeDuration);
            SetSpecificAlpha(panelAlpha: alpha, textAlpha: 0f); 
            yield return null;
        }
        SetSpecificAlpha(0f, 0f);
        isBigAdvice = false;
    }

    void SetSpecificAlpha(float panelAlpha, float textAlpha)
    {
        Color t = textLabel.color;
        t.a = textAlpha;
        textLabel.color = t;

        if (bigAdvicePanel != null)
        {
            Color p = bigAdvicePanel.color;
            p.a = panelAlpha;
            bigAdvicePanel.color = p;
        }
    }
}