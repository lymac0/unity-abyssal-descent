using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private Animator anim;
    public bool grounded;
    public float drag;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(xInput) > 0)
            body.linearVelocity = new Vector2(xInput * speed, body.linearVelocity.y);
        if (Mathf.Abs(yInput) > 0)
            body.linearVelocity = new Vector2(body.linearVelocity.x, yInput * speed);


        if (xInput > 0)
            transform.localScale = new Vector3(math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        if (xInput < 0)
            transform.localScale = new Vector3(-math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        if (Input.GetKey(KeyCode.Space))
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, speed);
        }

        anim.SetBool("isWalking", xInput != 0);
    }

    private void FixedUpdate()
    {
        CheckGround();

        if (grounded)
        {
            body.linearVelocity *= drag;
        }
    }

    private void CheckGround()
    {
    }
}
