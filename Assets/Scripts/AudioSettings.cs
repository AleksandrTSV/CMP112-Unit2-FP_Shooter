using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    // Must match the keys used in PauseMenu.cs
    private const string KeyMaster = "MasterVolume";
    private const string KeyMusic = "MusicVolume";
    private const string KeySFX = "SFXVolume";

    private void Start()
    {
        ApplyVolume(KeyMaster, PlayerPrefs.GetFloat(KeyMaster, 1f));
        ApplyVolume(KeyMusic, PlayerPrefs.GetFloat(KeyMusic, 1f));
        ApplyVolume(KeySFX, PlayerPrefs.GetFloat(KeySFX, 1f));
    }

    private void ApplyVolume(string parameterName, float linearValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(linearValue, 0.0001f, 1f)) * 20f;
        mixer.SetFloat(parameterName, dB);
    }
}