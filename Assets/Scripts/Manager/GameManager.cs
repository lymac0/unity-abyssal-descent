using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;   // Resources klasöründe "Player.prefab" olmalı
    [SerializeField] private float respawnDelay = 2f;    // ⏳ Inspector’dan ayarlanabilir doğum süresi

    private SaveController saveController;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        saveController = FindFirstObjectByType<SaveController>();
    }

    public void Respawn()
    {
        Debug.Log($"💀 Respawn tetiklendi. {respawnDelay} sn sonra sahne yeniden yüklenecek...");
        StartCoroutine(DelayedReload());
    }

    private IEnumerator DelayedReload()
    {
        yield return new WaitForSeconds(respawnDelay);
        SceneManager.sceneLoaded += OnSceneReloaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneReloaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("↩ Sahne yeniden yüklendi, yeni Player oluşturuluyor...");

        // Yeni player oluştur (Resources klasöründen prefab yüklenecek)
        GameObject prefab = Resources.Load<GameObject>("Player");
        if (prefab == null)
        {
            Debug.LogError("❌ Player prefab Resources klasöründe bulunamadı!");
            return;
        }

        GameObject newPlayer = Instantiate(prefab);
        newPlayer.tag = "Player";

        // Referans bildirme
        PlayerEvents.RaisePlayerSpawned(newPlayer);
        AssignPlayerReferences(newPlayer);

        // Save verisini uygula
        if (saveController != null)
        {
            StartCoroutine(DelayedApplyData(newPlayer));
        }
        else
        {
            Debug.LogError("❌ SaveController bulunamadı!");
        }

        SceneManager.sceneLoaded -= OnSceneReloaded;
    }

    private IEnumerator DelayedApplyData(GameObject player)
    {
        yield return new WaitForEndOfFrame();

        saveController.ApplySavedDataToPlayer(player);

        // Kamera hedefini ayarla
        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
        {
            cam.target = player.transform;
        }

        Debug.Log("✅ Veri yeni oyuncuya başarıyla yüklendi.");
    }

    public void AssignPlayerReferences(GameObject newPlayer)
    {
        Transform playerTransform = newPlayer.transform;

        var dependents = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                               .OfType<IPlayerDependent>()
                               .ToArray();

        foreach (var dependent in dependents)
        {
            dependent.SetPlayer(playerTransform);
        }

        Debug.Log("✅ Tüm objelere yeni player referansı atandı.");
    }
}
