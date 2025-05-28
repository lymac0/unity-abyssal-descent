using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("Player is dead!");

        if (animator != null)
            animator.SetBool("isDead", true);
        else
            Debug.LogError("Animator is missing on Player!");

        StartCoroutine(RestartAfterDelay(1.5f));
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 1. Sahneyi yeniden yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // 2. Kayıtlı pozisyona döndür
        // Sahne yüklendikten hemen sonra çalışması için gecikmeli çağır
        SceneManager.sceneLoaded += OnSceneReloaded;
    }

    // 3. Sahne yüklendikten sonra konumu geri getir
    private void OnSceneReloaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("↩ Sahne yeniden yüklendi, yeni Player oluşturuluyor...");


        // Eski player'ı sil (eğer varsa)
        GameObject oldPlayer = GameObject.FindGameObjectWithTag("Player");
        if (oldPlayer != null)
            Destroy(oldPlayer);

        // Player prefab'ını sahneye ekle (Resources klasöründen)
        GameObject playerPrefab = Resources.Load<GameObject>("Player"); // Assets/Resources/Player.prefab olmalı
        GameObject newPlayer = Instantiate(playerPrefab);
        newPlayer.tag = "Player";

        // Kayıtlı verileri uygula
        SaveController saveController = Object.FindFirstObjectByType<SaveController>();
        if (saveController != null)
        {
            saveController.ApplySavedDataToPlayer(newPlayer);
        }
        else
        {
            Debug.LogError("❌ SaveController bulunamadı!");
        }
        PlayerEvents.RaisePlayerSpawned(newPlayer);

        SceneManager.sceneLoaded -= OnSceneReloaded;
    }


    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Debug.Log("Lazerle çarpıştı!");

            Animator laserAnimator = collision.GetComponent<Animator>();
            if (laserAnimator == null)
            {
                Debug.LogError("HATA: Lazerin Animator bileşeni eksik!");
                return;
            }

            StartCoroutine(CheckLaserState(laserAnimator));
        }

        if (collision.CompareTag("lava"))
        {
            Debug.Log("Lava ile çarpıştı! Oyuncu ölüyor...");
            Die();
        }
    }

    private IEnumerator CheckLaserState(Animator laserAnimator)
    {
        yield return new WaitForSeconds(0.3f);

        if (laserAnimator.GetCurrentAnimatorStateInfo(0).IsName("laser_shooter"))
        {
            Debug.Log("Lazer aktif, oyuncu ölüyor!");
            Die();
        }
        else
        {
            Debug.Log("Lazer ateşlenmiyor, oyuncu ölmedi.");
        }
    }
}
