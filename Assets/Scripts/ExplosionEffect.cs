using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class ExplosionEffect : MonoBehaviour
{
    [Header("Sprites")]
    [Tooltip("Drag all sprites from the sprite-sheet in the right order")]
    [SerializeField] private Sprite[] frames;

    [Header("Audio")]
    [SerializeField] private AudioClip explosionClip;

    [Header("Playback")]
    [SerializeField] private float fps = 15f;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private float timer;
    private int currentFrame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    private void OnEnable()
    {
        currentFrame = 0;
        timer = 0f;

        if (frames != null && frames.Length > 0)
            spriteRenderer.sprite = frames[0];

        if (explosionClip != null)
            audioSource.PlayOneShot(explosionClip);
    }

    private void Update()
    {
        if (frames == null || frames.Length == 0) return;

        timer += Time.deltaTime;
        float frameInterval = 1f / fps;

        if (timer >= frameInterval)
        {
            timer -= frameInterval;
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                gameObject.SetActive(false);
                return;
            }

            spriteRenderer.sprite = frames[currentFrame];
        }
    }

    private void LateUpdate()
    {
        if (Camera.main != null) 
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}