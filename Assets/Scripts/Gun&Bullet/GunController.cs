using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class GunController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform creator;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource source;

    [Header("Bullet")]
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] int poolSize = 30;
    [SerializeField] float bulletSpeed = 40f;

    [Header("Fire")]
    [SerializeField] float fireRate = 0.2f;

    [Header("Overheat")]
    [SerializeField] float maxHeatTime = 5f;       // Seconds of continuous fire before overheat
    [SerializeField] float cooldownDuration = 3f;  // Seconds locked out after overheat
    [SerializeField] ParticleSystem gunDeadParticles; // Assign GunDead particle system here


    [Header("Audio")]
    [SerializeField] AudioClip fireLoop;
    [SerializeField] AudioClip fireEnd;
    [SerializeField] AudioClip overheat;

    [Header("UI Document")]
    [SerializeField] UIDocument uiDocument;

    // ── Bullet pool ───────────────────────────────────────────────────────────
    Bullet[] pool;
    int poolIndex;

    // ── Fire state ────────────────────────────────────────────────────────────
    bool isFiring = false;
    private float nextFireTime = 0.2f; //Fire rate

    // ── Overheat state ────────────────────────────────────────────────────────
    float heatTimer = 0f;          // How long the player has been continuously firing
    bool isOverheated = false;     // True while gun is locked in cooldown

    // ── UI References ─────────────────────────────────────────────────────────
    VisualElement heatFill;        // The fill bar element
    VisualElement heatBarContainer;
    Label heatLabel;

    [SerializeField] PlayerManager gameInfo;

    private void Awake()
    { 
        pool = new Bullet[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(bulletPrefab);  //Create 30 instances of bullets 
            pool[i].gameObject.SetActive(false);  // and we will only ue these ones(instead of using Instantiate and then Destroy)
        }

        // Grab heat bar elements from UI Toolkit
        if (uiDocument != null)
        {
            var root = uiDocument.rootVisualElement;
            heatBarContainer = root.Q<VisualElement>("heatBarContainer");
            heatFill = root.Q<VisualElement>("heatFill");
            heatLabel = root.Q<Label>("heatLabel");
        }
    }

    void Update()
    {
        if (isOverheated) return;

        // ── Fire bullets ──────────────────────────────────────────────────────
        if (isFiring && Time.time > nextFireTime) //Checks if you are holding fire button
        {
            Bullet bullet = pool[poolIndex];
            poolIndex = (poolIndex + 1) % poolSize; //so we dont have OutOfBoundsException

            bullet.transform.position = creator.position;
            bullet.transform.parent = null;

            Vector3 direction = creator.forward;

            bullet.gameObject.SetActive(true);
            bullet.Fire(direction.normalized * bulletSpeed); //gives the direction to the bullet

            nextFireTime = Time.time + fireRate;
        }

        if (isFiring)
        {
            heatTimer += Time.deltaTime;

            if (heatTimer >= maxHeatTime)
            {
                TriggerOverheat();
                return;
            }
        }
        else
        {
            // Cool down passively when not shooting
            heatTimer = Mathf.Max(0f, heatTimer - Time.deltaTime);
        }

        UpdateHeatUI();
    }

    // ── Input ─────────────────────────────────────────────────────────────────
    void OnAttack(InputValue value) //Check if fire button is pressed 
    {
        if (value.Get<float>() > 0.5f && !gameInfo.GetIsPaused())
        {
            StartFire();
        }

        else 
        {
            StopFire();
        }
    }
    
    bool isAudioPlaying = false; //to ensure that our SFX will activate ones
    void StartFire() 
    {
        if (isOverheated) return; // Silently ignore input during cooldown

        anim.SetBool("Shoot", true); //Activate shooting animation
        //---------------------
        if (!isAudioPlaying)
        {
            source.clip = fireLoop;
            source.loop = true;
            source.Play();
            isAudioPlaying = true;
        }

        isFiring = true;
    }
    void StopFire()
    {
        if (isAudioPlaying)
        {
            anim.SetBool("Shoot", false);
            source.Stop();
            source.PlayOneShot(fireEnd);
            isAudioPlaying = false;
        }

        isFiring = false;
    }

    // ── Overheat ──────────────────────────────────────────────────────────────
    void TriggerOverheat()
    {
        isOverheated = true;
        StopFire();
        source.PlayOneShot(overheat);

        // Play the GunDead particle effect
        if (gunDeadParticles != null)
            gunDeadParticles.Play();

        // Show heat bar as full / red while overheated
        UpdateHeatUI(forceMax: true);

        StartCoroutine(CooldownRoutine());
    }


    IEnumerator CooldownRoutine()
    {
        float elapsed = 0f;

        while (elapsed < cooldownDuration)
        {
            elapsed += Time.deltaTime;
            // Visually drain the bar back to 0 during the cooldown period
            float remainingHeat = Mathf.Lerp(maxHeatTime, 0f, elapsed / cooldownDuration);
            heatTimer = remainingHeat;
            UpdateHeatUI();
            yield return null;
        }

        // Stop particles once cooled
        if (gunDeadParticles != null)
            gunDeadParticles.Stop();

        heatTimer = 0f;
        isOverheated = false;
        UpdateHeatUI();
    }

    // ── UI ────────────────────────────────────────────────────────────────────
    void UpdateHeatUI(bool forceMax = false)
    {
        if (heatFill == null) return;

        float ratio = forceMax ? 1f : Mathf.Clamp01(heatTimer / maxHeatTime);
        heatFill.style.width = Length.Percent(ratio * 100f);

        // Colour: green → yellow → red
        Color barColor;
        if (isOverheated)
        {
            barColor = new Color(1f, 0.2f, 0.2f); // bright red while locked out
        }
        else if (ratio < 0.5f)
        {
            barColor = Color.Lerp(new Color(0.1f, 0.85f, 0.25f), new Color(1f, 0.85f, 0f), ratio * 2f);
        }
        else
        {
            barColor = Color.Lerp(new Color(1f, 0.85f, 0f), new Color(1f, 0.2f, 0.1f), (ratio - 0.5f) * 2f);
        }

        heatFill.style.backgroundColor = new StyleColor(barColor);

        // Optional label
        if (heatLabel != null)
        {
            heatLabel.text = isOverheated ? "OVERHEATED!" : $"Heat: {Mathf.RoundToInt(ratio * 100f)}%";
        }
    }
}





