using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    [Header("UI Document")]
    [SerializeField] UIDocument uiDocument;
    VisualElement healthInfoBox;
    VisualElement goalInfo;
    VisualElement goalFill;
    Label goalLabel;
    int theGoal;
    [HideInInspector]public VisualElement crosshair;
    VisualElement heatBar;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        healthInfoBox = uiDocument.rootVisualElement.Q<VisualElement>("HealthInfo");
        goalInfo = uiDocument.rootVisualElement.Q<VisualElement>("goalInfo");
        goalFill = uiDocument.rootVisualElement.Q<VisualElement>("goalFill");
        goalLabel = uiDocument.rootVisualElement.Q<Label>("goalLabel");

        goalLabel.text = $"Enemies left: {playerStats.goal}";

        theGoal = playerStats.goal;
        goalFill.style.width = Length.Percent(100f);

        crosshair = uiDocument.rootVisualElement.Q<VisualElement>("Crosshair");
        heatBar = uiDocument.rootVisualElement.Q<VisualElement>("HeatBarContainer");
    }

    private void Start()
    {
        Time.timeScale = 1;
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
        UpdateUIGoal();

        if (playerStats.goal <= 0)
            TriggerWin();
    }

    // ── Calls from EnemyMovement, when enemy touches the player ──────────────
    public void PlayerHit()
    {
        if (gameEnded) return;

        playerStats.DecreaseHealth();
        musicSource.PlayOneShot(hitSound);
        UpdateUIHearts();

        if (playerStats.health <= 0)
            TriggerLose();
    }

    // ── HUD ───────────────────────────────────────────────────────────────────
    private void UpdateUIGoal()
    {
        if (gameEnded)
        {
            goalInfo.style.display = DisplayStyle.None;
        }

        float progress = (float)playerStats.goal / theGoal;
        goalFill.style.width = Length.Percent(progress * 100f);
        goalLabel.text = $"Enemies left: {playerStats.goal}";
    }

    private void UpdateUIHearts()
    {
        if (gameEnded)
        {
            healthInfoBox.style.display = DisplayStyle.None;
        }

        for (int i = 0; i < healthInfoBox.childCount; i++)
        {
            VisualElement heartImage = healthInfoBox[i];
            bool visible = i < playerStats.health;
            heartImage.visible = visible;
        }
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
        crosshair.style.display = DisplayStyle.None;
        heatBar.style.display = DisplayStyle.None;
        UpdateUIHearts();
        UpdateUIGoal();
        Time.timeScale = 0;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }
}