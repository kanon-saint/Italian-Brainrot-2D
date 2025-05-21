using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private const string MusicPref = "MusicVolume";
    private const string SFXPref = "SFXVolume";

    void Start()
    {
        // Load saved values or use default (1.0f)
        float savedMusic = PlayerPrefs.GetFloat(MusicPref, 1f);
        float savedSFX = PlayerPrefs.GetFloat(SFXPref, 1f);

        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;

        SetMusicVolume(savedMusic);
        SetSFXVolume(savedSFX);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
        PlayerPrefs.SetFloat(SFXPref, sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void CancelChanges()
    {
        // Revert to last saved values
        float savedMusic = PlayerPrefs.GetFloat(MusicPref, 1f);
        float savedSFX = PlayerPrefs.GetFloat(SFXPref, 1f);

        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;

        SetMusicVolume(savedMusic);
        SetSFXVolume(savedSFX);
    }
}
