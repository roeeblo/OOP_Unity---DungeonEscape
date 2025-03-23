using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GM is null");
            }
            return _instance;
        }
    }

    public bool HasKey { get; set; }
    public VictoryScreenController victoryScreenController;
    public AudioClip backgroundMusic;
    private AudioSource audioSource;

    public void Awake()
    {
        _instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    void Update()
    {
        if (HasKey && PlayerIsBeyondX(128))
        {
            victoryScreenController.ShowVictoryScreen();
        }
    }

    private bool PlayerIsBeyondX(float x)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            return player.transform.position.x > x;
        }
        return false;
    }
}
