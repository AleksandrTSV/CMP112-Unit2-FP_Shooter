using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity = 60f;
    private InputSystem_Actions input;

    float xRotation;
    float yRotation;

    private Rigidbody rb;

    private float pitch = 0.0f;
    private float yaw = 0.0f;
    private float limit;
    private Vector2 lookInput;
    private Vector2 moveInput;

    private float movementX;
    private float movementY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;

        input = new InputSystem_Actions();
        input.Player.Enable();

        input.Player.Look.started += ctx => lookInput = ctx.ReadValue<Vector2>();
        input.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        input.Player.Move.performed += _ => moveInput = _.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        OnMouseMove(lookInput);
    }

    private void FixedUpdate()
    {
        OnMove(moveInput);
    }

    void OnMove(Vector2 movementVector) 
    {
        movementX = movementVector.x;
        movementY = movementVector.y;

        rb.AddForce(movementX, 0.0f, movementY);
    }

    private void OnMouseMove(Vector2 delta)
    {
        pitch = delta.y * mouseSensitivity * Time.deltaTime;
        yaw = delta.x * mouseSensitivity * Time.deltaTime;

        xRotation -= pitch;
        xRotation = Mathf.Clamp(xRotation, -44, 44);
        yRotation += yaw;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        //transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up) * transform.rotation;
        //transform.rotation = transform.rotation * Quaternion.AngleAxis(-pitch, Vector3.right);

    }
}
