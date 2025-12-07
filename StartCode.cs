using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuUI : MonoBehaviour
{
  public UIDocument uiDocument;
  
  private Button startButton;
  private Button creditsButton;

  void Start()
  {
    var uiDocument = GetComponent<UIDocument>();
    var root = uiDocument.rootVisualElement;

    startButton = root.Q<Button>("startButton");

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
    SceneManeger.LoadScene("Jogo");
  }
}
