using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreenController : MonoBehaviour
{
    public GameObject victoryCanvas;

    public void ShowVictoryScreen()
    {
        victoryCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game"); 
    }

    // Quit the game
    public void QuitGame()
    {
        Time.timeScale = 1;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
