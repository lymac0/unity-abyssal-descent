using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public int health;
    public float speed;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isRunning", true);
    }
    void Update()
    {
        if(health <= 0) {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    public void TakeDamage(int damage) {
        health -= damage;
        Debug.Log("Damage taken!!");
    }
}
