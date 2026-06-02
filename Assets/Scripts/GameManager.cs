using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // ── Singleton ────────────────────────────────────────────────────────────
    public static GameManager Instance { get; private set; }

    // ── Player ───────────────────────────────────────────────────────────────
    [Header("Player")]
    [SerializeField] private PlayerStats playerStats;

    // ── Player ───────────────────────────────────────────────────────────────
    //[Header("Camera")]
    //[SerializeField] private GameObject;

    // ── HUD ──────────────────────────────────────────────────────────────────
    [Header("HUD Text")]
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private TextMeshProUGUI healthText;

    // ── UI Panels ────────────────────────────────────────────────────────────
    [Header("UI Panels")]
    [SerializeField] private GameObject winPanel;     
    [SerializeField] private GameObject losePanel;    

    // ── Music ─────────────────────────────────────────────────────────────────
    [Header("Result Music")]
    [SerializeField] private AudioSource musicSource; // separate AudioSource for GameManager
    [SerializeField] private AudioClip winMusic;      
    [SerializeField] private AudioClip loseMusic;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip[] dominations;

    // ── State ─────────────────────────────────────────────────────────────────
    [HideInInspector] public bool gameEnded = false;

    // ─────────────────────────────────────────────────────────────────────────

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;

        // Show start values for HP and Goal
        RefreshGoalText();
        RefreshHealthText();
    }

    private void Update()
    {
        if (gameEnded) return;
    }

    // ── Calls from EnemyMovement, when enemy is killed by bullet ───────────────────
    public void EnemyKilled()
    {
        if (gameEnded) return;

        playerStats.DecreaseGoal();
        RefreshGoalText();

        if (playerStats.goal <= 0)
            TriggerWin();
    }

    // ── Calls from EnemyMovement, when enemy touches the player ──────────────
    public void PlayerHit()
    {
        if (gameEnded) return;

        playerStats.DecreaseHealth();
        musicSource.PlayOneShot(hitSound);
        RefreshHealthText();

        if (playerStats.health <= 0)
            TriggerLose();
    }

    // ── HUD ───────────────────────────────────────────────────────────────────
    private void RefreshGoalText()
    {
        goalText.text = $"Enemies left: {playerStats.goal}";
    }

    private void RefreshHealthText()
    {
        healthText.text = $"Health: {playerStats.health}";
    }

    // ── Game Over ────────────────────────────────────────────────────────────
    private void TriggerWin()
    {
        if (gameEnded) return;
        gameEnded = true;
        ShowResultScreen(winPanel, winMusic);
    }

    private void TriggerLose()
    {
        if (gameEnded) return;
        gameEnded = true;
        ShowResultScreen(losePanel, loseMusic);
        musicSource.PlayOneShot(dominations[Random.Range(0, dominations.Length)]);
    }

    private void ShowResultScreen(GameObject panel, AudioClip music)
    {
        // Stop ALL sounds in the game
        AudioSource[] allSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource src in allSources)
        {
            if (src != musicSource)  // except our AudioSorce
                src.mute = !src.mute;
        }

        // Play win/lose music
        if (musicSource != null && music != null)
        {
            musicSource.clip = music;
            musicSource.loop = false;
            musicSource.Play();
        }

        // Show panel and freeze the game
        panel.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }
}