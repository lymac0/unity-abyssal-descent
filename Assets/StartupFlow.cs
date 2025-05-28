using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class StartupFlow : MonoBehaviour
{
    public GameObject playerPage;
    public GameObject inventoryPage;
    public GameObject settingsPage;

    public AudioSource startupMusic;
    public AudioSource mainMusic;

    public Button startGameButton;

    private string savePath;
    private bool hasSaveFile = false;

    void Start()
    {
        // 1. Kayıt dosyası var mı kontrol et
        savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        hasSaveFile = File.Exists(savePath);

        // 2. PlayerPage her zaman açık
        playerPage.SetActive(true);
        inventoryPage.SetActive(false);
        settingsPage.SetActive(false);

        // 3. Müzik yönetimi
        if (!hasSaveFile && startupMusic != null)
        {
            startupMusic.Play();
            Invoke(nameof(PlayMainMusic), startupMusic.clip.length);
        }
        else
        {
            PlayMainMusic();
        }

        // 4. Başlat butonuna basıldığında oyun başlasın
        if (startGameButton != null)
            startGameButton.onClick.AddListener(OnStartGame);
    }

    void PlayMainMusic()
    {
        if (mainMusic != null && !mainMusic.isPlaying)
            mainMusic.Play();
    }

    public void OnStartGame()
    {
        Debug.Log("▶️ Oyuna başla butonuna basıldı!");

        playerPage.SetActive(false);
        inventoryPage.SetActive(true);
        settingsPage.SetActive(false);

        // → Burada LoadGame() çağrılırsa oyun kayıtlı yerden başlar
        if (hasSaveFile)
        {
            Object.FindFirstObjectByType<SaveController>()?.LoadGame();

        }
        else
        {
            Debug.Log("📌 Kayıt bulunamadı, yeni oyun başlatılıyor.");
            // inventoryController zaten boş slotlarla açılıyor
        }
    }
}
