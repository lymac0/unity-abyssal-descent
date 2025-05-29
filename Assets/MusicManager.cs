using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void ToggleMusic(bool isOn)
    {
        audioSource.mute = !isOn;
    }

    public float GetVolume()
    {
        return audioSource.volume;
    }

    public bool IsMusicOn()
    {
        return !audioSource.mute;
    }
}
