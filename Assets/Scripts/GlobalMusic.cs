using UnityEngine;

public class GlobalMusic : MonoBehaviour
{
    private static GlobalMusic instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        // Aplicar mute según PlayerPrefs
        bool musicOn = PlayerPrefs.GetInt("Music", 1) == 1;
        audioSource.mute = !musicOn;
    }

    public void UpdateMuteStatus()
    {
        bool musicOn = PlayerPrefs.GetInt("Music", 1) == 1;
        if (audioSource != null)
            audioSource.mute = !musicOn;
    }
}
