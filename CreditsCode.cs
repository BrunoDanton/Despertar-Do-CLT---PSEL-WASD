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

    if (startButton != null)
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
    Fundo.SetActive(false);
    //Deixa o GameObject dos creditos desativado
    Credit.SetActive(true);
  }
}
