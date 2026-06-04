using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool is_Paused = false;

    //[SerializeField] GameManager gameInfo;
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Enemy")) 
        {
            GameManager.Instance?.PlayerHit();
            CameraShake.Instance?.TriggerShake();
        }
    }

    public bool GetIsPaused() { return is_Paused; }

    void OnPause(InputValue value)
    {
        if (!value.isPressed) return;

        if (is_Paused)
        {
            UnpauseGame();
        }
        else if (!is_Paused && !GameManager.Instance.gameEnded)
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        GameManager.Instance.crosshair.style.display = DisplayStyle.None;
        Time.timeScale = 0;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        is_Paused = true;
    }

    public void UnpauseGame()
    {
        pauseMenu.SetActive(false);
        GameManager.Instance.crosshair.style.display = DisplayStyle.Flex;
        Time.timeScale = 1;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        is_Paused = false;
    }
}
