using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpSpeed;
    private float groundSpeed;
    private Rigidbody2D body;
    private Animator anim;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    private int jumpCount;

    private float xInput;
    private float yInput;

    public int maxJumps = 2;
    public bool grounded;
    [Range(0f, 1f)]
    public float groundDecay;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpCount = maxJumps;
    }

    private void Update()
    {
        GetInput();
        MoveWithInput();
    }

    private void FixedUpdate()
    {
        CheckGround();
        ApplyFriction();
    }

    private void GetInput()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }
    private void MoveWithInput()
    {

        if (Mathf.Abs(xInput) > 0)
        {
            body.linearVelocity = new Vector2(xInput * groundSpeed, body.linearVelocity.y);
            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);


            if (Input.GetButton("Sprint"))
            {
                groundSpeed = runSpeed;
                anim.SetBool("isRunning", true);
            }
            else
            {
                groundSpeed = walkSpeed;
                anim.SetBool("isRunning", false);
            }


        }

        if (Mathf.Abs(yInput) > 0)
            body.linearVelocity = new Vector2(body.linearVelocity.x, yInput * groundSpeed);


        if (grounded && body.linearVelocity.y == 0)
        {
            jumpCount = maxJumps;
        }
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
            jumpCount--;
        }


        //anim.SetBool("isWalking", xInput != 0);
        anim.SetFloat("xVelocity", Mathf.Abs(body.linearVelocity.x));
        anim.SetFloat("yVelocity", body.linearVelocity.y);

    }

    private void CheckGround()
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
        anim.SetBool("isGrounded", grounded);

    }

    private void ApplyFriction()
    {
        if (grounded && xInput == 0)
            body.linearVelocity *= groundDecay;
    }
}
