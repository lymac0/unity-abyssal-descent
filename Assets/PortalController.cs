﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalController : MonoBehaviour
{
    public Image fadeImage;
    public Animator portalAnimator;
    public string targetSceneName;
    public string spawnPointTag = "SpawnPoint";

    private GameObject player;

    private void OnEnable()
    {
        PlayerEvents.OnPlayerSpawned += HandlePlayerSpawned;
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerSpawned -= HandlePlayerSpawned;
    }

    private void HandlePlayerSpawned(GameObject newPlayer)
    {
        player = newPlayer;
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            DontDestroyOnLoad(player);
            StartCoroutine(PortalTransition());
        }
    }

    private IEnumerator PortalTransition()
    {
        Debug.Log("PortalTransition başladı, oyuncu: " + player.name);
        portalAnimator.SetTrigger("Activate");
        yield return new WaitForSeconds(1.5f);

        fadeImage.gameObject.SetActive(true);
        Debug.Log("Fade to Black başlıyor...");
        yield return StartCoroutine(FadeToBlack());

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(targetSceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Yeni sahne yüklendi: " + scene.name);

        GameObject spawnPoint = GameObject.FindWithTag(spawnPointTag);
        if (spawnPoint != null && player != null)
        {
            player.transform.position = spawnPoint.transform.position;
            Debug.Log("Player taşındı: " + player.transform.position);
        }
        else
        {
            Debug.LogWarning("SpawnPoint veya Player bulunamadı!");
        }

        if (player != null)
            SceneManager.MoveGameObjectToScene(player, SceneManager.GetActiveScene());

        GameObject fadeObj = new GameObject("FadeRunner");
        FadeRunner runner = fadeObj.AddComponent<FadeRunner>();
        runner.fadeImage = fadeImage;
        runner.RunFadeIn(() =>
        {
            fadeImage.gameObject.SetActive(false);
        });

        SceneManager.sceneLoaded -= OnSceneLoaded;
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
    }
}
