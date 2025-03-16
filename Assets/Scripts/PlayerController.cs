using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool canDash = true;
    private bool canJump;
    private bool isDashing;
    private bool isWallSliding;

    public bool isTouchingWall;
    public bool isGrounded;
    public bool isFacingRight = false;

    private float xInput;
    private float movementSpeed;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 1f;

    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    [SerializeField] private float dashingPower = 20f;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private GameObject smokeEffectPrefab;

    private Rigidbody2D body;
    private Animator anim;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundMask;

    private int jumpCount;
    private int facingDirection = 1;

    public int maxJumps = 2;

    [Range(0f, 1f)]
    public float groundDecay;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpCount = maxJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }
        GetInput();
        CheckMovementDirection();
        UpdateAnimations();
        MoveWithInput();
        CheckIfCanJump();
        CheckIfWallSliding();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        CheckSurroundings();
        ApplyFriction();
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && body.linearVelocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && xInput < 0)
        {
            Flip();
        }
        else if (!isFacingRight && xInput > 0)
        {
            Flip();
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("xVelocity", Mathf.Abs(body.linearVelocity.x));
        anim.SetFloat("yVelocity", body.linearVelocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void GetInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && canJump)
        {
            Jump();
        }

        if (Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetButtonUp("Jump"))
        {
            body.linearVelocity = new Vector2(body.linearVelocityX, body.linearVelocity.y * variableJumpHeightMultiplier);
        }
    }
    private void MoveWithInput()
    {
        if (isDashing)
        {
            return;
        }
        if (Mathf.Abs(xInput) > 0 && isGrounded)
        {
            body.linearVelocity = new Vector2(xInput * movementSpeed, body.linearVelocity.y);
        }
        else if (!isGrounded && !isWallSliding && Mathf.Abs(xInput) != 0)
        {
            Vector2 forceToAdd = new Vector2(movementForceInAir * xInput, 0);
            body.AddForce(forceToAdd);

            if (Mathf.Abs(body.linearVelocity.x) > movementSpeed)
            {
                body.linearVelocity = new Vector2(movementSpeed * xInput, body.linearVelocity.y);
            }
        }
        else if (!isGrounded && !isWallSliding && xInput == 0)
        {
            body.linearVelocity = new Vector2(body.linearVelocityX * airDragMultiplier, body.linearVelocityY);
        }

        // WALL SLIDE
        if (isWallSliding && body.linearVelocityY < -wallSlideSpeed)
        {
            body.linearVelocity = new Vector2(body.linearVelocityX, -wallSlideSpeed);
        }

        // Koşma mekaniği
        if (Input.GetButton("Sprint") && Mathf.Abs(xInput) > 0 && isGrounded)
        {
            movementSpeed = runSpeed;
            anim.SetBool("isRunning", true);
        }
        else
        {
            movementSpeed = walkSpeed;
            anim.SetBool("isRunning", false);
        }



        // Hızın sıfırlanması
        if (Mathf.Abs(body.linearVelocity.x) < 0.3f)
        {
            body.linearVelocity = new Vector2(0, body.linearVelocityY);
        }
    }

    private void Flip()
    {
        if (!isWallSliding)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            if (!isWallSliding) transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundMask);

    }

    private void CheckIfCanJump()
    {
        if ((isGrounded && body.linearVelocity.y <= 0) || isWallSliding)
        {
            jumpCount = maxJumps;
        }

        canJump = jumpCount > 0;
    }

    private void ApplyFriction()
    {
        if (isGrounded && xInput == 0)
            body.linearVelocity *= groundDecay;
    }

    private void Jump()
    {
        if (canJump && !isWallSliding)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            jumpCount--;
        }

        if (isWallSliding && xInput == 0 && canJump) // WALL HOP
        {
            isWallSliding = false;
            jumpCount--;
            Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
            body.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
        else if (isWallSliding && xInput != 0 && canJump)
        {
            isWallSliding = false;
            jumpCount--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * xInput, wallJumpForce * wallJumpDirection.y);
            body.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        anim.SetBool("isDashing", true);

        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;

        float dashDirection = isFacingRight ? 1 : -1;

        Vector3 smokePosition = transform.position + new Vector3(dashDirection * -0.5f, -0.75f, 0);
        GameObject smoke = Instantiate(smokeEffectPrefab, smokePosition, Quaternion.identity);
        smoke.transform.localScale = new Vector3(dashDirection, 1, 1);
        Destroy(smoke, 0.5f);

        body.linearVelocity = new Vector2(dashDirection * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        body.gravityScale = originalGravity;
        isDashing = false;
        anim.SetBool("isDashing", false);
        body.linearVelocity = new Vector2(0, body.linearVelocityY);
        yield return new WaitForSeconds(0.15f);

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}