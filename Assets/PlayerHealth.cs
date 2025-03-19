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

    private bool isDead = false; // Oyuncunun �ld���n� takip etmek i�in de�i�ken ekle

    public void Die()
    {
        if (isDead) return; // E�er oyuncu zaten �ld�yse tekrar �a�r�lmas�n� engelle

        isDead = true; // Oyuncu �ld� olarak i�aretle

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
            Debug.Log("Lazerle �arp��t�!");

            Animator laserAnimator = collision.GetComponent<Animator>();

            if (laserAnimator == null)
            {
                Debug.LogError("HATA: Lazerin Animator bile�eni eksik!");
                return;
            }

            StartCoroutine(CheckLaserState(laserAnimator));
        }
    }


    private IEnumerator CheckLaserState(Animator laserAnimator)
    {
        yield return new WaitForSeconds(0.3f); // Bekleme s�resini 0.3 saniyeye ��kard�k

        Debug.Log("Lazer animasyon durumu: " + laserAnimator.GetCurrentAnimatorStateInfo(0).IsName("laser_shooter"));

        if (laserAnimator.GetCurrentAnimatorStateInfo(0).IsName("laser_shooter"))
        {
            Debug.Log("Lazer aktif, oyuncu �l�yor!");
            Die();
        }
        else
        {
            Debug.Log("Lazer ate�lenmiyor, oyuncu �lmedi.");
        }
    }


}
