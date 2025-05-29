using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro kullan�yorsan

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle musicToggle;
    public Text musicToggleLabel; // Normal Text
    // public TMP_Text musicToggleLabel; // E�er TextMeshPro ise bunu kullan

    void Start()
    {
        RefreshUI();
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        musicToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    public void RefreshUI()
    {
        if (MusicManager.Instance == null) return;

        volumeSlider.value = MusicManager.Instance.GetVolume();
        musicToggle.isOn = MusicManager.Instance.IsMusicOn();

        UpdateToggleLabel(musicToggle.isOn);
    }

    private void OnVolumeChanged(float value)
    {
        MusicManager.Instance.SetVolume(value);
    }

    private void OnToggleChanged(bool isOn)
    {
        MusicManager.Instance.ToggleMusic(isOn);
        UpdateToggleLabel(isOn);
    }

    private void UpdateToggleLabel(bool isOn)
    {
        musicToggleLabel.text = isOn ? "M�zik A��k" : "M�zik Kapal�";
    }
}
