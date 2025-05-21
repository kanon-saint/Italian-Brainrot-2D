using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Mixer & Sliders")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Music Toggle UI")]
    [SerializeField] private GameObject musicOnBtn;
    [SerializeField] private GameObject musicMutedBtn;

    [Header("SFX Toggle UI")]
    [SerializeField] private GameObject sfxOnBtn;
    [SerializeField] private GameObject sfxMutedBtn;

    private const string MusicPref = "MusicVolume";
    private const string SFXPref = "SFXVolume";

    private float lastMusicVolume = 1f;
    private float lastSFXVolume = 1f;

    void Start()
    {
        // Load saved values
        float savedMusic = PlayerPrefs.GetFloat(MusicPref, 1f);
        float savedSFX = PlayerPrefs.GetFloat(SFXPref, 1f);

        lastMusicVolume = savedMusic;
        lastSFXVolume = savedSFX;

        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;

        SetMusicVolume(savedMusic);
        SetSFXVolume(savedSFX);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        UpdateMusicToggleUI();
        UpdateSFXToggleUI();
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);

        if (value > 0.01f)
            lastMusicVolume = value;

        UpdateMusicToggleUI();
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);

        if (value > 0.01f)
            lastSFXVolume = value;

        UpdateSFXToggleUI();
    }

    public void ToggleMusicMute()
    {
        if (musicSlider.value > 0.01f)
        {
            // Save current volume before muting
            lastMusicVolume = musicSlider.value;
            musicSlider.value = 0f;
        }
        else
        {
            musicSlider.value = lastMusicVolume;
        }
    }

    public void ToggleSFXMute()
    {
        if (sfxSlider.value > 0.01f)
        {
            // Save current volume before muting
            lastSFXVolume = sfxSlider.value;
            sfxSlider.value = 0f;
        }
        else
        {
            sfxSlider.value = lastSFXVolume;
        }
    }

    private void UpdateMusicToggleUI()
    {
        bool isMuted = musicSlider.value <= 0.01f;
        musicOnBtn.SetActive(!isMuted);
        musicMutedBtn.SetActive(isMuted);
    }

    private void UpdateSFXToggleUI()
    {
        bool isMuted = sfxSlider.value <= 0.01f;
        sfxOnBtn.SetActive(!isMuted);
        sfxMutedBtn.SetActive(isMuted);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
        PlayerPrefs.SetFloat(SFXPref, sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void CancelChanges()
    {
        float savedMusic = PlayerPrefs.GetFloat(MusicPref, 1f);
        float savedSFX = PlayerPrefs.GetFloat(SFXPref, 1f);

        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;

        SetMusicVolume(savedMusic);
        SetSFXVolume(savedSFX);
    }
}
