using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Labels")]
    [SerializeField] private TextMeshProUGUI masterLabel;
    [SerializeField] private TextMeshProUGUI musicLabel;
    [SerializeField] private TextMeshProUGUI sfxLabel;

    private const string KeyMaster = "MasterVolume";
    private const string KeyMusic = "MusicVolume";
    private const string KeySFX = "SFXVolume";
    public void Resume() 
    {
        playerManager.UnpauseGame();
    }

    public void MainMenu() 
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Start()
    {
        float master = PlayerPrefs.GetFloat(KeyMaster, 1f);
        float music = PlayerPrefs.GetFloat(KeyMusic, 1f);
        float sfx = PlayerPrefs.GetFloat(KeySFX, 1f);

        // Sliders
        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;

        // Subscribe to changes
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        SetMasterVolume(1f);
        SetMusicVolume(1f);
        SetSFXVolume(1f);
    }

    public void SetMasterVolume(float value)
    {
        ApplyVolume(KeyMaster, value);
        UpdateLabel(masterLabel, "Master", value);
    }

    public void SetMusicVolume(float value)
    {
        ApplyVolume(KeyMusic, value);
        UpdateLabel(musicLabel, "Music", value);
    }

    public void SetSFXVolume(float value)
    {
        ApplyVolume(KeySFX, value);
        UpdateLabel(sfxLabel, "SFX", value);
    }

    private void ApplyVolume(string parameterName, float linearValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(linearValue, 0.0001f, 1f)) * 20f;
        mixer.SetFloat(parameterName, dB);
        PlayerPrefs.SetFloat(parameterName, linearValue);
    }

    private void UpdateLabel(TextMeshProUGUI label, string name, float value)
    {
        if (label != null)
            label.text = $"{name}: {Mathf.RoundToInt(value * 100)}%";
    }
}
