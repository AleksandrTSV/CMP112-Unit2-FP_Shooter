using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;
    [SerializeField] private CameraMovement camera;

    private void Start()
    {
        slider.value = camera.mouseSensitivity;
        UpdateSensitivity(slider.value);
        slider.onValueChanged.AddListener(UpdateSensitivity);
    }

    private void UpdateSensitivity(float value) 
    {
        camera.mouseSensitivity = value;
        sliderText.text = $"Mouse sensitivity: {value.ToString("F0")}";
    }
}
