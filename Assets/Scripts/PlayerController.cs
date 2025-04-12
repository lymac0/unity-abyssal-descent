using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isWalking = false;
    private bool canDash = true;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isDashing;
    private bool isWallSliding;
    private bool isAttemtingToJump;
    private bool checkJumpMuliplier;
    private bool canMove;
    private bool canFlip;
    private bool hasWallJumped;
    private bool isTouchingLedge;
    private bool canClimbLedge = false;
    private bool ledgeDetected;

    public bool isTouchingWall;
    public bool isGrounded;
    public bool isFacingRight = false;

    private float jumpTimer;
    private float xInput;
    private float movementSpeed;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 1f;
    private float turnTimer;
    private float wallJumpTimer;
    private float lastImageXpos;

    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;
    public float ledgeClimbXOffset1 = 0f;
    public float ledgeClimbYOffset1 = 0f;
    public float ledgeClimbXOffset2 = 0f;
    public float ledgeClimbYOffset2 = 0f;
    public float distanceBetweenImages;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;
    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    [SerializeField] private float dashingPower = 20f;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private GameObject smokeEffectPrefab;

    private Rigidbody2D body;
    private Animator anim;
    public Transform groundCheck;
    public Transform wallCheck;
    public Transform ledgeCheck;

    public LayerMask groundMask;

    private int jumpCount;
    private int facingDirection = 1;
    private int lastWallJumpedDirection;

    public int maxJumps = 1;

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
        CheckDash();
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
        CheckJump();
        CheckLedgeClimb();

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
        if (isTouchingWall && xInput == facingDirection && body.linearVelocityY < 0 && !canClimbLedge)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

    }

    private void CheckLedgeClimb()
    {
        if (ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            if (isFacingRight)
            {
                ledgePos1 = new Vector2(ledgePosBot.x + wallCheckDistance - ledgeClimbXOffset1, ledgePosBot.y + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(ledgePosBot.x + wallCheckDistance + ledgeClimbXOffset2, ledgePosBot.y + ledgeClimbYOffset2);
            }
            else
            {
                ledgePos1 = new Vector2(ledgePosBot.x - wallCheckDistance + ledgeClimbXOffset1, ledgePosBot.y + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(ledgePosBot.x - wallCheckDistance - ledgeClimbXOffset2, ledgePosBot.y + ledgeClimbYOffset2);
            }
            canMove = false;
            canFlip = false;

            anim.SetBool("canClimbLedge", canClimbLedge);
        }
        if (canClimbLedge)
        {
            transform.position = ledgePos1;
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
        if (xInput != 0)
        {
            anim.SetBool("isRunning", !isWalking);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    private void GetInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || (jumpCount > 0 && isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemtingToJump = true;
            }
        }

        if (Input.GetButtonDown("Dash") && canDash)
        {
            AttemptToDash();
            StartCoroutine(Dash());
        }

        if (Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if (!isGrounded && xInput != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if (turnTimer >= 0)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (checkJumpMuliplier && !Input.GetButton("Jump"))
        {
            checkJumpMuliplier = false;
            body.linearVelocity = new Vector2(body.linearVelocityX, body.linearVelocity.y * variableJumpHeightMultiplier);
        }
    }
    private void MoveWithInput()
    {
        if (isDashing)
        {
            return;
        }
        if (!isGrounded && !isWallSliding && xInput == 0)
        {
            body.linearVelocity = new Vector2(body.linearVelocityX * airDragMultiplier, body.linearVelocityY);
        }
        else if (canMove)
        {
            body.linearVelocity = new Vector2(xInput * movementSpeed, body.linearVelocity.y);
        }


        // WALL SLIDE
        if (isWallSliding && body.linearVelocityY < -wallSlideSpeed)
        {
            body.linearVelocity = new Vector2(body.linearVelocityX, -wallSlideSpeed);
        }

        // Koþma mekaniði
        if (Input.GetButtonDown("Walk"))
        {
            isWalking = !isWalking;
        }

        movementSpeed = isWalking ? walkSpeed : runSpeed;

        // Hýzýn sýfýrlanmasý
        if (Mathf.Abs(body.linearVelocity.x) < 0.3f)
        {
            body.linearVelocity = new Vector2(0, body.linearVelocityY);
        }
    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    private void Flip()
    {
        if (!isWallSliding && canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            if (!isWallSliding) transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        anim.SetBool("canClimbLedge", canClimbLedge);
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundMask);
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, groundMask);

        if (isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && body.linearVelocity.y <= 0.01f)
        {
            jumpCount = maxJumps;
        }

        if (isTouchingWall)
        {
            checkJumpMuliplier = false;
            canWallJump = true;
        }

        canNormalJump = jumpCount > 0;
    }

    private void ApplyFriction()
    {
        if (isGrounded && xInput == 0)
            body.linearVelocity *= groundDecay;
    }

    private void CheckJump()
    {
        if (jumpTimer > 0)
        {
            // Wall Jump
            if (!isGrounded && isTouchingWall && xInput != 0 && xInput != facingDirection)
            {
                Debug.Log("Wall Jump atýldý!");
                WallJump();
            }
            else if (isGrounded)
            {
                NormalJump();
            }
        }
        if (isAttemtingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (wallJumpTimer > 0)
        {
            if (hasWallJumped && xInput == -lastWallJumpedDirection)
            {
                body.linearVelocity = new Vector2(body.linearVelocityX, 0.0f);
                hasWallJumped = false;
            }
            else if (wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }
    }

    private void NormalJump()
    {
        if (canNormalJump)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            jumpCount--;
            jumpTimer = 0;
            isAttemtingToJump = false;
            checkJumpMuliplier = true;
        }
    }

    private void WallJump()
    {
        if (canWallJump)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, 0.0f);
            isWallSliding = false;
            jumpCount = maxJumps;
            jumpCount--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * xInput, wallJumpForce * wallJumpDirection.y);
            body.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemtingToJump = false;
            checkJumpMuliplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpedDirection = -facingDirection;
        }
    }

    private void AttemptToDash()
    {
        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImageXpos = transform.position.x;
            }
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
        Gizmos.DrawLine(ledgePos1, ledgePos2);
    }
}