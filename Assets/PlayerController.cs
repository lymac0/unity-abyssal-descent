using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Karakterin yatay hareket h�z�
    [SerializeField] private float jumpForce = 15f; // Z�plama kuvveti
    private Rigidbody2D body;

    private void Awake()
    {
        // Rigidbody2D bile�enini al�yoruz
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Yatay hareketi kontrol et
        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        // Karakterin y�n�n� de�i�tirme (Sprite'� sa�a/sola �evirme)
        if (horizontalInput > 0.01f) // Sa� y�n
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f) // Sol y�n
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Z�plama kontrol�
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(body.linearVelocity.y) < 0.01f) // Yerdeyken z�plama
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        }
    }
}