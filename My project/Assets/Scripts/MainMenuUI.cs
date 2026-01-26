using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{
   
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Load()
    {
        SceneManager.LoadScene("NewGame");
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

}