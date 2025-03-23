using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private AudioClip menuSelectSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = menuSelectSound;
        audioSource.playOnAwake = false;

        startButton.onClick.AddListener(() => StartCoroutine(PlaySoundAndStartGame()));
        closeButton.onClick.AddListener(() => StartCoroutine(PlaySoundAndCloseGame()));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(PlaySoundAndCloseGame());
        }
    }

    IEnumerator PlaySoundAndStartGame()
    {
        PlaySound();
        yield return new WaitForSeconds(audioSource.clip.length);
        StartGame();
    }

    IEnumerator PlaySoundAndCloseGame()
    {
        PlaySound();
        yield return new WaitForSeconds(audioSource.clip.length);
        CloseGame();
    }

    void PlaySound()
    {
        audioSource.Play();
    }

    void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    void CloseGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
