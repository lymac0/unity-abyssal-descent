using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private Animator animator; // Animator bile�eni

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator bile�enini al
    }

    public void Die()
    {
        Debug.Log("Player is dead!");

        if (animator != null) // Animator'un null olup olmad���n� kontrol et
        {
            animator.SetBool("isDead", true); // "Died" animasyonunu tetikle
        }
        else
        {
            Debug.LogError("Animator is missing on Player!");
        }

        // Bir s�re sonra sahneyi yeniden y�kle
        Invoke("RestartLevel", 1.5f);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Die();
        }
    }
}
