using System;
using System.Collections;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public float health = 80;
    public float speed;
    public float wallCheckDistance;
    private bool isTouchingWall;
    public Transform wallCheck;
    public LayerMask groundMask;
    private Rigidbody2D rb;
    private Animator anim;
    private int facingDirection = 1;
    private SpriteRenderer spriteRenderer;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        Patrolling();
    }

    void FixedUpdate()
    {
        CheckSurroundings();
    }

    private void Patrolling()
    {
        rb.linearVelocity = new Vector2(facingDirection * speed, rb.linearVelocity.y);
        anim.SetBool("isRunning", rb.linearVelocity.x != 0);
        if (isTouchingWall)
        {
            Flip();
        }
    }

    private void CheckSurroundings()
    {
        isTouchingWall = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, groundMask);
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(facingDirection, transform.localScale.y, transform.localScale.z);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Damage taken!!");
        StartCoroutine(FlashWhite());
        StartCoroutine(Knockback());
    }

    private IEnumerator FlashWhite()
    {
        spriteRenderer.material.SetInt("_Hit",1);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.SetInt("_Hit",0);
    }

    private IEnumerator Knockback()
    {
        float originalSpeed = speed;
        speed = 0;
        rb.linearVelocity = new Vector2(-facingDirection * knockbackForce, rb.linearVelocity.y);
        yield return new WaitForSeconds(knockbackDuration);
        speed = originalSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y, wallCheck.position.z));
    }
}