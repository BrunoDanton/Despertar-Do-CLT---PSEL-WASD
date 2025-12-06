using UnityEngine;
using UnityEngine.UIElements;

public class Creditos : MonoBehaviour
{
  private Button startButton;
  private Button creditsButton;
  public GameObject Fundo;
  public GameObject Credit;
  
  void OnEnable()
  {
    var uiDocument = GetComponent<UIDocument>();
    var root = uiDocument.rootVisualElement;

    startButton = root.Q<Button>("startButton");
    creditsButton = root.Q<Button>("creditsButton");
    backButton = root.Q<Button>("backButton");

    //esse DisplayStyle serve para deixar o botão de voltar inativo no menu inicial
    backButton.style.display = DisplayStyle.None;

    if (creditsButton != null)
    {
      creditsButton.clicked += Credito;
    }
    else
    {
      Debug.LogError("Botão não encontrado");
    }
  }

  void Credito()
  {
    startButton.style.display = DisplayStyle.None;
    creditsButton.style.display = DisplayStyle.None;
    backButton.style.display = DisplayStyle.Flex;
    Fundo.SetActive(false);
    //Deixa o GameObject dos creditos desativado
    Credit.SetActive(true);

    OnBack();
  }

  // Código para o botão de voltar
  void OnBack()
  {
    var uiDocument = GetComponent<UIDocument>();
    var root = uiDocument.rootVisualElement;

    startButton = root.Q<Button>("startButton");
    creditsButton = root.Q<Button>("creditsButton");
    backButton = root.Q<Button>("backButton");

    if (backButton != null)
    {
      backButton.clicked += Back;
    }
    else
    {
      Debug.LogError("Botão não encontrado");
    }
  }
  
  void Back()
  {
    startButton.style.display = DisplayStyle.Flex;
    creditsButton.style.display = DisplayStyle.Flex;
    backButton.style.display = DisplayStyle.None;
    Fundo.SetActive(true);
    //Desativa o GameObject dos creditos novamente
    Credit.SetActive(false);
  }
}
