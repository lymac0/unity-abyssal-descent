using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalController : MonoBehaviour
{
    public Image fadeImage;
    public Animator portalAnimator;
    public string targetSceneName; // Ge�ilecek sahne ad�
    public string spawnPointTag = "SpawnPoint"; // Yeni sahnedeki oyuncu do�ma yeri i�in tag

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DontDestroyOnLoad(other.gameObject); // Oyuncuyu sahne de�i�iminde koru
            StartCoroutine(PortalTransition(other.gameObject));
        }
    }

    private IEnumerator PortalTransition(GameObject player)
    {
        portalAnimator.SetTrigger("Activate");
        yield return new WaitForSeconds(2.1f);
        yield return StartCoroutine(FadeToBlack());

        // Sahneyi y�kle
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Yeni sahnedeki spawn noktas�n� bul
        GameObject spawnPoint = GameObject.FindWithTag(spawnPointTag);
        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
        }

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadeFromBlack());
    }

    private IEnumerator FadeToBlack()
    {
        float duration = 1f;
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeFromBlack()
    {
        float duration = 1f;
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsed / duration);
            fadeImage.color = color;
            yield return null;
        }
    }
}
