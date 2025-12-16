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

    public int health = 3;
    public int goal = 20;

    void Start()
    {
        character = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
    }

    private void FixedUpdate()
    {
        character.Move((GetForward() * move.y + GetRight() * move.x) * currentSpeed * Time.deltaTime);
        Debug.Log($"Heath: {health}");
    }

    private Vector3 GetForward() 
    {
        Vector3 forward = orientation.transform.forward;
        forward.y = 0;

        return forward.normalized;
    }
    private Vector3 GetRight()
    {
        Vector3 right = orientation.transform.right;
        right.y = 0;

        return right.normalized;
    }

    void OnMove(InputValue moveValue) 
    {
        move = moveValue.Get<Vector2>();
    }

    void OnSprint(InputValue value) 
    {
        if (value.Get<float>() > 0.5f) currentSpeed = sprintSpeed;
        else currentSpeed = walkSpeed;
    }

    public void DeceraseGoal() 
    {
        goal -= 1;
        goal = Mathf.Max(goal, 0);
    }

    public void DeceraseHealth()
    {
        health -= 1;
        health = Mathf.Max(health, 0);
    }
}
