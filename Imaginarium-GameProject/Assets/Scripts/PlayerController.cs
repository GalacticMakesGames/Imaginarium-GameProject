using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    Vector2 moveInput;

    public float jumpForce;
    [SerializeField] bool jumpInput;

    public Transform groundChecker;
    public LayerMask ground;
    public float rayLength;
    [SerializeField] bool grounded;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && grounded) jumpInput = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveInput.x * moveSpeed, rb.velocity.y, moveInput.y * moveSpeed);

        RaycastHit hit;
        if (Physics.Raycast(groundChecker.position, Vector3.down, out hit, rayLength, ground)) grounded = true;
        else grounded = false;

        Debug.DrawRay(groundChecker.position, Vector2.down, Color.red);

        if (jumpInput) Jump();
    }

    void Jump()
    {
        rb.velocity = new Vector3(0f, jumpForce, 0f);
        jumpInput = false;
    }
}
