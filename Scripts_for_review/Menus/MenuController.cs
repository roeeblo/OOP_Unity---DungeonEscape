using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject popupWindow;
    public Player player;

    public AudioClip popupSound;
    public AudioClip buttonClickSound;
    private AudioSource audioSource;

    private bool isPopupVisible = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void TogglePopupWindow()
    {
        isPopupVisible = !isPopupVisible;
        popupWindow.SetActive(isPopupVisible);
        Time.timeScale = isPopupVisible ? 0 : 1;

        if (popupSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(popupSound);
        }
    }

    public void ResetPlayerLocation()
    {
        if (player != null)
        {
            player.ResetPosition();
            HidePopupWindow();
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        StartCoroutine(QuitGameAfterSound());
    }

    IEnumerator QuitGameAfterSound()
    {
        // Wait for the audio clip to finish playing
        if (buttonClickSound != null && audioSource != null)
        {
            yield return new WaitForSeconds(buttonClickSound.length);
        }
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ResumeGame()
    {
        HidePopupWindow();
    }

    public void ResetGame()
    {
        ResetPlayerLocation();
        ResetProgress();
        HidePopupWindow();
    }

    private void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void HidePopupWindow()
    {
        isPopupVisible = false;
        popupWindow.SetActive(false);
        Time.timeScale = 1;
    }
}
