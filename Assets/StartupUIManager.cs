using UnityEngine;
using System.IO;

public class StartupUIManager : MonoBehaviour
{
    public GameObject playerPage;
    public GameObject inventoryPage;
    public GameObject settingsPage;

    public AudioSource startupMusic;
    public AudioSource mainMusic;

    private string saveLocation;

    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        if (!File.Exists(saveLocation))
        {
            // Ýlk defa giriyor: sadece PlayerPage aç, diðerleri kapalý
            ShowOnlyPlayerPage();

            // Giriþ müziðini çal
            if (startupMusic != null)
            {
                startupMusic.Play();
                Invoke(nameof(StartMainMusic), startupMusic.clip.length);
            }
        }
        else
        {
            // Normal oyun müziði direkt baþlasýn
            if (mainMusic != null)
                mainMusic.Play();
        }
    }

    void ShowOnlyPlayerPage()
    {
        playerPage.SetActive(true);
        inventoryPage.SetActive(false);
        settingsPage.SetActive(false);
    }

    void StartMainMusic()
    {
        if (mainMusic != null)
            mainMusic.Play();
    }
}
