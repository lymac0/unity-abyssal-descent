using UnityEngine;

public class SceneAutoSaver : MonoBehaviour
{
    void Start()
    {
        Debug.Log("✅ 2. Katman yüklendi, pozisyon otomatik kaydediliyor...");
        Object.FindFirstObjectByType<SaveController>()?.SaveGame();
    }
}
