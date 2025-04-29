using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FadeRunner : MonoBehaviour
{
    public Image fadeImage;
    private Action onFadeComplete;

    public void RunFadeIn(Action onComplete)
    {
        onFadeComplete = onComplete;
        StartCoroutine(FadeFromBlack());
    }

    private IEnumerator FadeFromBlack()
    {
        Debug.Log("Fade from Black başlıyor...");
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

        onFadeComplete?.Invoke(); // Fade bitince pasif yap
        Destroy(gameObject); // Sadece FadeRunner'ı yok et
    }
}
