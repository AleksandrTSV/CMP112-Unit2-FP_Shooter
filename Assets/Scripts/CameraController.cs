using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity = 60f;
    private InputSystem_Actions input;

    public GameObject player;
    private Vector3 offset;

    float xRotation;
    float yRotation;
    private float pitch = 0.0f;
    private float yaw = 0.0f;
    private Vector2 lookInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //To hide cursor and stay in the center of the screen

        input = new InputSystem_Actions();
        input.Player.Enable();

        input.Player.Look.started += ctx => lookInput = ctx.ReadValue<Vector2>(); //when we move the mouse, it reads the vector 
        input.Player.Look.canceled += ctx => lookInput = Vector2.zero; //if we dont move the mouse, the vector is zero

        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        OnMouseMove(lookInput);
        transform.position = player.transform.position + offset;
    }

    private void OnMouseMove(Vector2 delta)
    {
        pitch = delta.y * mouseSensitivity * Time.deltaTime;
        yaw = delta.x * mouseSensitivity * Time.deltaTime;

        xRotation -= pitch;
        xRotation = Mathf.Clamp(xRotation, -44f, 50f); //so the camera could not make a flip 
        yRotation += yaw;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        //transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up) * transform.rotation;              //this is the
        //transform.rotation = transform.rotation * Quaternion.AngleAxis(-pitch, Vector3.right);        //other option

    }
}
