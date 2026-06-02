using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // ── Singleton ────────────────────────────────────────────────────────────
    public static CameraShake Instance { get; private set; }

    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeMagnitude = 0.15f;

    private CameraMovement cameraMovement;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        cameraMovement = GetComponent<CameraMovement>();
    }

    public void TriggerShake()
    {
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float strength = Mathf.Lerp(shakeMagnitude, 0f, elapsed / shakeDuration);

            cameraMovement.shakeOffset = Random.insideUnitSphere * strength;

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraMovement.shakeOffset = Vector3.zero;
    }
}
