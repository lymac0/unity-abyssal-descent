using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private static bool alreadyLoaded = false; // ✅ yalnızca ilk sefer çalışması için

    private string saveLocation;
    private InventoryController inventoryController;

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
        saveData saveData = new saveData();

        saveData.playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        saveData.inventorySaveData = inventoryController.GetInventoryItems();

        string json = JsonUtility.ToJson(saveData, true); // pretty print
        Debug.Log("📦 JSON içeriği:\n" + json);

        File.WriteAllText(saveLocation, json);
        Debug.Log("✅ saveData.json yazıldı: " + saveLocation);
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            saveData saveData = JsonUtility.FromJson<saveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;
            inventoryController.SetInventoryItems(saveData.inventorySaveData);

            Debug.Log("✅ Kayıt yüklendi.");
        }
        else
        {
            Debug.Log("ℹ Kayıt dosyası bulunamadı.");
        }
    }
}
