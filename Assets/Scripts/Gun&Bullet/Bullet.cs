using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    private Rigidbody rb;

    [Header("Explosion")]
    [Tooltip("Drag here prefab ExplosionEffect from Prefabs folder")]
    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private int explosionPoolSize = 5;
    private GameObject[] explosionPool;
    private int explosionPoolIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        explosionPool = new GameObject[explosionPoolSize];
        for (int i = 0; i < explosionPoolSize; i++)
        {
            explosionPool[i] = Instantiate(explosionPrefab);
            explosionPool[i].SetActive(false);
        }
    }

    public void Fire(Vector3 velocity) // Gives live to our bullet
    {
        rb.linearVelocity = velocity;
        CancelInvoke(); // Ensure that the bullet will not disappear sooner than we need 
        Invoke(nameof(Disable), lifeTime); // After 3 seconds in the air bullet disappers
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            SpawnExplosion();
            Disable();
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Ground")) Disable();
    }

    private void SpawnExplosion()
    {
        if (explosionPrefab == null) return;

        GameObject explosion = explosionPool[explosionPoolIndex];
        explosionPoolIndex = (explosionPoolIndex + 1) % explosionPoolSize;

        explosion.transform.position = transform.position;
        explosion.SetActive(true); // OnEnable() will start the animation
    }
}
