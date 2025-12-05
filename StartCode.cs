using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuUI : MonoBehaviour
{
  private Button startButton;
  private Button creditsButton;
  public GameObject Fundo;
  public GameObject C2;

  void OnEnable()
  {
    var uiDocument = GetComponent<UIDocument>();
    var root = uiDocument.rootVisualElement;

    startButton = root.Q<Button>("startButton");
    creditsButton = root.Q<Button>("creditsButton");

    if(startButton != null)
    {
      startButton.clicked += StartGame;
    }
    else
    {
      Debug.logError("Botão de start não encontrado");
    }
  }
  void StartGame()
  {
    Debug.Log("Começou");

    //esconde os botões e o fundo (esse C2 foi uma segunda camera que eu coloquei e o fundo é um bloco que eu coloquei como fundo)
    startButton.style.display = DisplayStyle.None;
    creditsButton.style.display = DisplayStyle.None;
    Fundo.SetActive(false);
    C2.SetActive(false);
  }
}
