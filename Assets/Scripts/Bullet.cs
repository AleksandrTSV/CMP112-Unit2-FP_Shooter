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

    private void OnCollisionEnter(Collision collision)
    {
        Disable();
    }
}
