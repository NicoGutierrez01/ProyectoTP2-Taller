using UnityEngine;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour
{
    private static SoundOptions instance;

    [Header("Referencias UI")]
    public GameObject soundPanel;
    public Toggle musicToggle;
    public Toggle fxToggle;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource[] fxSources;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        if (soundPanel != null)
            soundPanel.SetActive(false);

        // Buscar el AudioSource global si no está asignado
        if (musicSource == null)
        {
            GlobalMusic globalMusic = FindObjectOfType<GlobalMusic>();
            if (globalMusic != null)
                musicSource = globalMusic.GetComponent<AudioSource>();
        }

        // Leer valores previos
        bool musicOn = PlayerPrefs.GetInt("Music", 1) == 1;
        bool fxOn = PlayerPrefs.GetInt("FX", 1) == 1;

        if (musicToggle != null)
        {
            musicToggle.isOn = musicOn;
            musicToggle.onValueChanged.AddListener(delegate { ToggleMusic(); });
        }

        if (fxToggle != null)
        {
            fxToggle.isOn = fxOn;
            fxToggle.onValueChanged.AddListener(delegate { ToggleFX(); });
        }

        ApplySettings();
    }


    public void ToggleOptionsPanel()
    {
        if (soundPanel != null)
            soundPanel.SetActive(!soundPanel.activeSelf);
    }

    public void ToggleMusic()
    {
        bool isOn = musicToggle.isOn;
        if (musicSource != null)
            musicSource.mute = !isOn;

        PlayerPrefs.SetInt("Music", isOn ? 1 : 0);
    }

    public void ToggleFX()
    {
        bool isOn = fxToggle.isOn;

        foreach (AudioSource fx in fxSources)
        {
            if (fx != null)
                fx.mute = !isOn;
        }

        PlayerPrefs.SetInt("FX", isOn ? 1 : 0);
    }

    private void ApplySettings()
    {
        if (musicSource != null)
            musicSource.mute = !musicToggle.isOn;

        foreach (AudioSource fx in fxSources)
        {
            if (fx != null)
                fx.mute = !fxToggle.isOn;
        }
    }
}
