using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalToLevel3 : MonoBehaviour
{
    public Image fadeImage; // Fade geçiþi için UI Image
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

        if (fadeImage == null)
        {
            GameObject fadeObj = GameObject.Find("FadeImage");
            if (fadeObj != null)
            {
                fadeImage = fadeObj.GetComponent<Image>();
                DontDestroyOnLoad(fadeImage.transform.root.gameObject);
            }
            else
            {
                Debug.LogWarning("FadeImage bulunamadý!");
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

        // Seçim paneli gösteriliyor
        UIManager.Instance.ShowChoicePanel();
        UIManager.Instance.returnButton.onClick.RemoveAllListeners();
        UIManager.Instance.continueButton.onClick.RemoveAllListeners();
        UIManager.Instance.returnButton.onClick.AddListener(EndGame);
        UIManager.Instance.continueButton.onClick.AddListener(GoToNextLevel);
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

    private void EndGame()
    {
        UIManager.Instance.resultText.text = "Ethan’ý kurtarmamayý seçtiniz.";
        UIManager.Instance.returnButton.interactable = false;
        UIManager.Instance.continueButton.interactable = false;
        StartCoroutine(QuitAfterDelay());
    }

    private IEnumerator QuitAfterDelay()
    {
        yield return new WaitForSeconds(3f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void GoToNextLevel()
    {
        UIManager.Instance.HideChoicePanel();
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
}
