using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private bool isDead = false; // Oyuncunun öldüðünü takip etmek için deðiþken ekle

    public void Die()
    {
        if (isDead) return; // Eðer oyuncu zaten öldüyse tekrar çaðrýlmasýný engelle

        isDead = true; // Oyuncu öldü olarak iþaretle

        Debug.Log("Player is dead!");

        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }
        else
        {
            Debug.LogError("Animator is missing on Player!");
        }

        StartCoroutine(RestartAfterDelay(1.5f));
    }


    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Debug.Log("Lazerle çarpýþtý!");

            Animator laserAnimator = collision.GetComponent<Animator>();

            if (laserAnimator == null)
            {
                Debug.LogError("HATA: Lazerin Animator bileþeni eksik!");
                return;
            }

            StartCoroutine(CheckLaserState(laserAnimator));
        }
    }


    private IEnumerator CheckLaserState(Animator laserAnimator)
    {
        yield return new WaitForSeconds(0.3f); // Bekleme süresini 0.3 saniyeye çýkardýk

        Debug.Log("Lazer animasyon durumu: " + laserAnimator.GetCurrentAnimatorStateInfo(0).IsName("laser_shooter"));

        if (laserAnimator.GetCurrentAnimatorStateInfo(0).IsName("laser_shooter"))
        {
            Debug.Log("Lazer aktif, oyuncu ölüyor!");
            Die();
        }
        else
        {
            Debug.Log("Lazer ateþlenmiyor, oyuncu ölmedi.");
        }
    }


}
