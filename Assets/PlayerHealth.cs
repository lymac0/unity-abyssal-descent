using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private Animator animator; // Animator bileþeni

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator bileþenini al
    }

    public void Die()
    {
        Debug.Log("Player is dead!");

        if (animator != null) // Animator'un null olup olmadýðýný kontrol et
        {
            animator.SetBool("isDead", true); // "Died" animasyonunu tetikle
        }
        else
        {
            Debug.LogError("Animator is missing on Player!");
        }

        // Bir süre sonra sahneyi yeniden yükle
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
