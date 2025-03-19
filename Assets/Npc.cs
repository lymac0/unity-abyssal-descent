using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    public GameObject interactionPrompt; // "E'ye basarak konu�" mesaj� i�in UI nesnesi

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private bool playerInRange;

    private void Start()
    {
        interactionPrompt.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange)
        {
            Debug.Log("Oyuncu NPC'nin yan�nda.");
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E tu�una bas�ld�, diyalog ba�l�yor!");
                Interact();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Oyuncu NPC'nin etkile�im alan�na girdi!"); // TEST ���N
            interactionPrompt.SetActive(true); // "E'ye bas" UI mesaj�n� a�
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Oyuncu NPC'den uzakla�t�!"); // TEST ���N
            interactionPrompt.SetActive(false); // "E'ye bas" mesaj�n� kapat
            playerInRange = false;
        }
    }


    public void Interact()
    {
        if (dialogueData == null)
            return;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        interactionPrompt.SetActive(false); // Diyalog ba�lad�, "E'ye bas" mesaj�n� kald�r

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        interactionPrompt.SetActive(true); // Diyalog bitti�inde tekrar "E'ye bas" mesaj�n� g�ster
    }
}
