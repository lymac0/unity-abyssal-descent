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
            // �lk defa giriyor: sadece PlayerPage a�, di�erleri kapal�
            ShowOnlyPlayerPage();

            // Giri� m�zi�ini �al
            if (startupMusic != null)
            {
                startupMusic.Play();
                Invoke(nameof(StartMainMusic), startupMusic.clip.length);
            }
        }
        else
        {
            // Normal oyun m�zi�i direkt ba�las�n
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
