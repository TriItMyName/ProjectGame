using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject levelSelectMenu;

    public void PlayGame()
    {
        levelSelectMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
