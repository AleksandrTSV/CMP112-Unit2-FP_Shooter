using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 velocity) // Gives live to our bullet
    {
        rb.linearVelocity = velocity;
        CancelInvoke(); // In case if we are out of available bullets in our pool 
        Invoke(nameof(Disable), lifeTime); // After 3 seconds in the air bullet disappers
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) Disable();
        else if (other.CompareTag("Wall") || other.CompareTag("Ground")) Disable();
    }
}
