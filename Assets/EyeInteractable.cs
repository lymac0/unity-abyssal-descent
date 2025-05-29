using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EyeInteractable : MonoBehaviour
{
    public Transform player;
    public GameObject interactionText;
    public GameObject fullScreenPanel;
    public TextMeshProUGUI infoText;
    public Button closeButton;

    public float interactRange = 2f;
    private bool isPanelOpen = false;

    [TextArea]
    public string infoMessage;

    private void Start()
    {
        // 🧠 PLAYER BUL
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        // 🧠 CANVAS UI ELEMANLARINI BUL
        if (interactionText == null)
            interactionText = GameObject.Find("InteractionText4"); // isme göre bul

        if (fullScreenPanel == null)
            fullScreenPanel = GameObject.Find("FullScreenPanel4");

        if (infoText == null)
            infoText = GameObject.Find("infoText4")?.GetComponent<TextMeshProUGUI>();

        if (closeButton == null)
            closeButton = GameObject.Find("CloseButton4")?.GetComponent<Button>();

        // 🧠 BUTON KAPATMA EVENT
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);

        // Başlangıçta gizle
        interactionText?.SetActive(false);
        fullScreenPanel?.SetActive(false);

        // Metni göster
        if (infoText != null && !string.IsNullOrEmpty(infoMessage))
            infoText.text = infoMessage;
    }


    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);

        if (distance <= interactRange && !isPanelOpen)
        {
            if (interactionText != null)
                interactionText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
                ShowPanel();
        }
        else
        {
            if (interactionText != null)
                interactionText.SetActive(false);
        }
    }

    void ShowPanel()
    {
        isPanelOpen = true;

        interactionText?.SetActive(false);
        fullScreenPanel?.SetActive(true);

        if (infoText != null)
            infoText.text = infoMessage;
    }

    void ClosePanel()
    {
        isPanelOpen = false;
        fullScreenPanel?.SetActive(false);
    }
}
