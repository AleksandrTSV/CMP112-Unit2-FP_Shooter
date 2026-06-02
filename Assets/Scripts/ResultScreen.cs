using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is for Win and Lose Panels.
/// Buttons: Restart / Main Menu / Quit
/// </summary>
public class ResultScreen : MonoBehaviour
{
    /// <summary> Restart the current scene </summary>
    public void Restart()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("MainGame");
    }

    /// <summary> Main Menu Button </summary>
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    /// <summary> Quit Button </summary>
    public void Quit()
    {
        Application.Quit();
    }
}