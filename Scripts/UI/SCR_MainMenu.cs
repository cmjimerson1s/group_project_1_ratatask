using UnityEngine;
using UnityEngine.SceneManagement;


public class SCR_MainMenu : MonoBehaviour
{
    public void PlayGame() //Put the right scenes in -> build settings -> add the scenes and reference right
  {
      SceneManager.LoadScene(1);
  }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void SettingsMenu()
    {
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGameWithAccessibility() {
        SceneManager.LoadScene(2);
    }
}
