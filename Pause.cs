using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Pause : MonoBehaviour
{
  private Button pauseButton;
  private Button returnButton;
  private Button menuButton;

  void OnEnable()
  {
    var uiDocument = GetComponent<UIDocument>();
    var root = uiDocument.rootVisualElement;

    pauseButton = root.Q<Button>("pauseButton");
    returnButton = root.Q<Button>("returnButton");
    menuButton = root.Q<Button>("menuButton");

    pauseButton.style.display = DisplayStyle.Flex;
    returnButton.style.display = DisplayStyle.None;
    menuButton.style.display = DisplayStyle.None;

    pauseButton.clicked -= PauseGame;
    returnButton.clicked -= ReturnGame;
    menuButton.clicked -= MenuGame;

    pauseButton.clicked += PauseGame;
    returnButton.clicked += ReturnGame;
    menuButton.clicked += MenuGame;
  }

  void PauseGame()
  {
    pauseButton.style.display = DisplayStyle.None;
    returnButton.style.display = DisplayStyle.Flex;
    menuButton.style.display = DisplayStyle.Flex;
    Time.timeScale = 0f;
  }
  
  void ReturnGame()
  {
    pauseButton.style.display = DisplayStyle.Flex;
    returnButton.style.display = DisplayStyle.None;
    menuButton.style.display = DisplayStyle.None;
    Time.timeScale = 1f;
  }
  
  void MenuGame()
  {
    SceneManager.LoadScene("Menu");
  }
}
