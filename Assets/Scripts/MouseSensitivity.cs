using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.UI;

public class MouseSensitivity : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;
    [SerializeField] private CameraMovement camera;

    private const string SENSITIVITY_KEY = "MouseSensitivity";

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat(SENSITIVITY_KEY, camera.mouseSensitivity); ;
        UpdateSensitivity(slider.value);
        slider.onValueChanged.AddListener(UpdateSensitivity);
    }

    private void UpdateSensitivity(float value) 
    {
        camera.mouseSensitivity = value;
        sliderText.text = $"Mouse sensitivity: {value.ToString("F0")}";

        PlayerPrefs.SetFloat(SENSITIVITY_KEY, value);
    }
}
