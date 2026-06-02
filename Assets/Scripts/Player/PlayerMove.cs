using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float currentSpeed;
    float walkSpeed = 8;
    float sprintSpeed = 15;

    public Transform orientation;
    private Vector2 move;
    CharacterController character;

    void Start()
    {
        character = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
        Time.timeScale = 1; //To ensure scene will load properly and will not be frozened
    }

    private void FixedUpdate()
    {
        character.Move((GetForward() * move.y + GetRight() * move.x) * currentSpeed * Time.deltaTime); //Looks which direction you're looking at and 
                                                                                                       //and moves depending on that direction
    }

    private Vector3 GetForward() //Gives X direction
    {
        Vector3 forward = orientation.transform.forward;
        forward.y = 0;

        return forward.normalized;
    }
    private Vector3 GetRight() //Gives Y direction
    {
        Vector3 right = orientation.transform.right;
        right.y = 0;

        return right.normalized;
    }

    void OnMove(InputValue moveValue) // Reads WASD
    {
        move = moveValue.Get<Vector2>();
    }

    void OnSprint(InputValue value) //Sprint function
    {
        if (value.Get<float>() > 0.5f) currentSpeed = sprintSpeed;
        else currentSpeed = walkSpeed;
    }
}
