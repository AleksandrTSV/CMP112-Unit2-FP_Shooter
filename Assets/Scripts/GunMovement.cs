using UnityEngine;
using UnityEngine.UIElements;

public class GunMovement : MonoBehaviour
{
    public Transform orientation;
    Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - orientation.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = orientation.position + offset;
        transform.rotation = orientation.rotation;
    }
}
