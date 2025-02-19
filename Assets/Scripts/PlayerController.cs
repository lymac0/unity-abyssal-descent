using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class PlayerController : MonoBehaviour
{
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower = 20f;
    private float dashingTime = 0.183f;
    private float dashingCooldown = 1f;
    [SerializeField] private GameObject smokeEffectPrefab; // Dash sırasında çıkacak duman efekti


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
        if (isDashing)
            return;
        GetInput();
        MoveWithInput();
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;
        CheckGround();
        ApplyFriction();
    }

    private void GetInput()
    {
        xInput = Input.GetAxis("Horizontal");
        // yInput = Input.GetAxis("Vertical");
    }
    private void MoveWithInput()
    {

        if (Mathf.Abs(xInput) > 0)
        {
            body.linearVelocity = new Vector2(xInput * groundSpeed, body.linearVelocity.y);
            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);

        }
        else if (!isDashing)
        {
            body.linearVelocity = new Vector2(0, body.linearVelocity.y);
        }

        if (Input.GetButton("Sprint") && Mathf.Abs(xInput) > 0)
        {
            groundSpeed = runSpeed;
            anim.SetBool("isRunning", true);
        }
        else
        {
            groundSpeed = walkSpeed;
            anim.SetBool("isRunning", false);
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
        if (Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(Dash());
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
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        anim.SetBool("isDashing", true); // Dash Start animasyonu başlasın

        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;

        float dashDirection = Mathf.Sign(transform.localScale.x);

        // 🚀 Dash başladığında Smoke Effect oluştur
        Vector3 smokePosition = transform.position + new Vector3(dashDirection * -0.5f, -0.75f, 0);
        GameObject smoke = Instantiate(smokeEffectPrefab, smokePosition, Quaternion.identity);
        smoke.transform.localScale = new Vector3(dashDirection, 1, 1);
        Destroy(smoke, 0.5f); // 0.5 saniye sonra efekti yok et

        body.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        body.gravityScale = originalGravity;
        isDashing = false;
        anim.SetBool("isDashing", false);
        yield return new WaitForSeconds(0.15f); 
        
        
        

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }



}