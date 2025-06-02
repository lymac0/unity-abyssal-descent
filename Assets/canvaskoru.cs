using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Dialogue Panel")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Button closeButton;

    [Header("Eye Interactable UI")]
    public GameObject interactionText4;
    public GameObject fullScreenPanel4;
    public TextMeshProUGUI infoText4;
    public Button closeButton4;

    [Header("Choice Panel")]
    public GameObject choicePanel;
    public Button returnButton;
    public Button continueButton;
    public TMP_Text resultText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Kontroller
        if (dialoguePanel == null)
            Debug.LogWarning("🔍 dialoguePanel referansı atanmadı!");

        if (fullScreenPanel4 == null)
            Debug.LogWarning("🔍 fullScreenPanel4 atanmadı!");
    }

    public void ShowDialogue(string npcName, string line)
    {
        dialoguePanel.SetActive(true);
        nameText.text = npcName;
        dialogueText.text = line;
        closeButton.gameObject.SetActive(true);
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        closeButton.gameObject.SetActive(false);
        dialogueText.text = "";
    }

    public void ShowEyePanel(string infoText)
    {
        fullScreenPanel4.SetActive(true);
        infoText4.text = infoText;
        closeButton4.gameObject.SetActive(true);
    }

    public void HideEyePanel()
    {
        fullScreenPanel4.SetActive(false);
        closeButton4.gameObject.SetActive(false);
    }
    public bool IsDialogueActive()
    {
        return dialoguePanel != null && dialoguePanel.activeSelf;
    }

    public void ShowChoicePanel()
    {
        if (choicePanel != null)
        {
            choicePanel.SetActive(true);
            resultText.text = "";
        }
    }

    public void HideChoicePanel()
    {
        if (choicePanel != null)
            choicePanel.SetActive(false);
    }
}
