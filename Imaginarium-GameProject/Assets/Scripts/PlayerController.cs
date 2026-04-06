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

    [SerializeField] bool backTurned;

    public bool flipped; // determines which direction the flip happens
    public float flipSpeed;

    Quaternion flipLeft = Quaternion.Euler(0, -180, 0); // quaternion refers to rotation
    Quaternion flipRight = Quaternion.Euler(0, 0, 0);

    Rigidbody rb;
    Animator anim;
    SpriteRenderer spriteRenderer;

    //public ThirdPersonCamera thirdPersonCamera;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.Instance.isDialogueActive)
        {
            moveInput.x = 0f;
            moveInput.y = 0f;
            return;
        }

        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        // Determines if flip is true or false
        if (!flipped && moveInput.x < 0)
        {
            flipped = true;
            //spriteRenderer.flipX = true;
        }
        else if (flipped && moveInput.x > 0)
        {
            flipped = false;
            //spriteRenderer.flipX = false;
        }

        // smoothly interpolate between two rotational values
        if (flipped)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, flipLeft, flipSpeed * Time.deltaTime);
            //thirdPersonCamera.transform.rotation = Quaternion.Euler(thirdPersonCamera._currentXRotation, thirdPersonCamera._currentYRotation, 0f);
        }
        else if (!flipped)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, flipRight, flipSpeed * Time.deltaTime);
            //thirdPersonCamera.transform.rotation = Quaternion.Euler(thirdPersonCamera._currentXRotation, thirdPersonCamera._currentYRotation, 0f);
        }

        // Determines if player's back is turned
        if (!backTurned && moveInput.y > 0) backTurned = true;
        else if (backTurned && moveInput.y < 0) backTurned = false;

            anim.SetBool("BackTurned", backTurned);

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
