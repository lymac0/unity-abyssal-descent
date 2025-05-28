using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalToLevel3 : MonoBehaviour
{
    public Image fadeImage; // Inspector’dan atanmazsa, sahnede ismiyle bulacaðýz
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
                player = playerObj;
        }

        // Eðer fadeImage atanmadýysa, sahnede ismiyle bulmaya çalýþ
        if (fadeImage == null)
        {
            GameObject fadeObj = GameObject.Find("FadeImage");
            if (fadeObj != null)
            {
                fadeImage = fadeObj.GetComponent<Image>();
                if (fadeImage != null)
                {
                    DontDestroyOnLoad(fadeImage.transform.root.gameObject); // root canvas’ý koru
                }
            }
            else
            {
                Debug.LogWarning("FadeImage ismiyle bir UI nesnesi bulunamadý!");
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
        portalAnimator.SetTrigger("Activate");
        yield return new WaitForSeconds(1.5f);

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            yield return StartCoroutine(FadeToBlack());
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(targetSceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject spawnPoint = GameObject.FindWithTag(spawnPointTag);
        if (spawnPoint != null && player != null)
        {
            player.transform.position = spawnPoint.transform.position;
        }

        if (player != null)
        {
            SceneManager.MoveGameObjectToScene(player, SceneManager.GetActiveScene());
        }

        if (fadeImage != null)
        {
            GameObject fadeObj = new GameObject("FadeRunner");
            FadeRunner runner = fadeObj.AddComponent<FadeRunner>();
            runner.fadeImage = fadeImage;
            runner.RunFadeIn(() =>
            {
                fadeImage.gameObject.SetActive(false);
            });
        }

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
}
