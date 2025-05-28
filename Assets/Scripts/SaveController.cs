using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private static bool alreadyLoaded = false;
    private string saveLocation;
    private InventoryController inventoryController;

    public SettingsMenu settingsMenuUI; // UI'de otomatik bağla

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = Object.FindFirstObjectByType<InventoryController>();

        if (!alreadyLoaded)
        {
            LoadGame();
            alreadyLoaded = true;
        }
    }

    public void SaveGame()
    {
        Debug.Log("📥 SaveGame fonksiyonu ÇALIŞTI!");

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("❌ Player bulunamadı!");
            return;
        }

        if (inventoryController == null)
        {
            Debug.LogError("❌ inventoryController null!");
            return;
        }

        if (MusicManager.Instance == null)
        {
            Debug.LogError("❌ MusicManager.Instance null!");
            return;
        }

        saveData saveData = new saveData();
        saveData.playerPosition = player.transform.position;
        saveData.inventorySaveData = inventoryController.GetInventoryItems();
        saveData.musicVolume = MusicManager.Instance.GetVolume();
        saveData.isMusicOn = MusicManager.Instance.IsMusicOn();

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveLocation, json);

        Debug.Log("✅ saveData.json yazıldı: " + saveLocation);
        Debug.Log("📦 JSON içeriği:\n" + json);
    }



    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            string content = File.ReadAllText(saveLocation);
            Debug.Log("📂 JSON Yükleniyor:\n" + content);

            saveData saveData = JsonUtility.FromJson<saveData>(content);

            Debug.Log("📍 Yüklenen pozisyon: " + saveData.playerPosition);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                player.transform.position = saveData.playerPosition;

            inventoryController.SetInventoryItems(saveData.inventorySaveData);

            MusicManager.Instance?.SetVolume(saveData.musicVolume);
            MusicManager.Instance?.ToggleMusic(saveData.isMusicOn);

            if (settingsMenuUI != null)
                settingsMenuUI.RefreshUI();

            Debug.Log("✅ Kayıt yüklendi.");
        }
        else
        {
            Debug.Log("ℹ Kayıt dosyası bulunamadı. Envanter sıfırlanıyor.");

            // Envanteri temizle (UI dahil)
            inventoryController.ClearInventory(); // böyle bir metodun varsa

            // Veya tüm slotları boş olarak oluştur
            inventoryController.InitializeEmptySlots();
        }
    }
    public void ApplySavedDataToPlayer(GameObject player)
    {
        if (!File.Exists(saveLocation)) return;

        string content = File.ReadAllText(saveLocation);
        saveData saveData = JsonUtility.FromJson<saveData>(content);

        player.transform.position = saveData.playerPosition;

        InventoryController inv = Object.FindFirstObjectByType<InventoryController>();
        if (inv != null)
        {
            inv.SetInventoryItems(saveData.inventorySaveData);
        }

        MusicManager.Instance?.SetVolume(saveData.musicVolume);
        MusicManager.Instance?.ToggleMusic(saveData.isMusicOn);

        Debug.Log("✅ Yeni player'a kayıtlı veriler yüklendi.");
    }


}
