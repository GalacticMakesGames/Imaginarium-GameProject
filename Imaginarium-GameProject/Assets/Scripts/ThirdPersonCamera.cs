using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    //public Transform orientation;
    //public Transform player;
    //public Transform playerObj;
    //public Rigidbody rb;

    //public float rotationSpeed;

    //public Transform PlayerController;
    //public Vector3 offset = new Vector3(5, 0, 0); // Adjust Z as needed

    //private float rotationX = 5;

    //public float _currentXRotation = 5f;
    //public float _currentYRotation = 0f;

    public Transform player;         // Drag player here in inspector
    public Vector3 offset = new Vector3(0, 5, 0);           // Set desired offset (e.g., 0, 5-10)

    void Start()
    {
        // Optional: Calculate initial offset if not set in inspector
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        //if (PlayerController != null)
        //{
        //    // Set position to player position + offset, keep current rotation
        //    transform.position = PlayerController.position + offset;
        //}

        // Camera follows position, but does not use player rotation
        transform.position = player.position + offset;
        transform.LookAt(player);
    }

    private void Update()
    {
        // rotate orientation
        //Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        //orientation.forward = viewDirection.normalized;

        //rotationX = Mathf.Clamp(rotationX, 0.0f, 0.0f);

        //transform.rotation = new Vector3 (rotationX, 0, 0);

        //_currentXRotation = Mathf.Clamp(_currentXRotation, 5, 5);

        //_currentYRotation = Mathf.Clamp(0, _currentYRotation, 0);

        //transform.rotation = Quaternion.Euler(_currentXRotation, _currentYRotation, 0f);
    }


}
