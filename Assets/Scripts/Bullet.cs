using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 velocity) 
    {
        rb.linearVelocity = velocity;
        CancelInvoke();
        Invoke(nameof(Disable), lifeTime);
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
