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
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        // UIManager'dan çek
        interactionText = UIManager.Instance.interactionText4;
        fullScreenPanel = UIManager.Instance.fullScreenPanel4;
        infoText = UIManager.Instance.infoText4;
        closeButton = UIManager.Instance.closeButton4;

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);

        interactionText?.SetActive(false);
        fullScreenPanel?.SetActive(false);

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
            infoMessage = infoText.text;
    }

    void ClosePanel()
    {
        isPanelOpen = false;
        fullScreenPanel?.SetActive(false);
    }
}
