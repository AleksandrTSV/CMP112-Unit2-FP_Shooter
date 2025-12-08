using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;

    public Transform orientation;

    float movementX;
    float movementY;

    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    private void FixedUpdate()
    {
        transform.rotation = orientation.rotation;

        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddRelativeForce(movement * speed);
    }

    void OnMove(InputValue moveValue) 
    {
        Vector2 moveVector = moveValue.Get<Vector2>();

        movementX = moveVector.x;
        movementY = moveVector.y;
    }
}
