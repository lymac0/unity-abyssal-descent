﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IPlayerDependent
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;
    public GameObject closeButton;
    public GameObject interactionPrompt;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private Transform playerTransform;
    public float interactionRange = 3f;
    private Transform player;


  
    public void SetPlayer(Transform playerTransform)
    {
        this.player = playerTransform;
        Debug.Log("NPC player referansı atandı.");
    }
    private void OnEnable()
    {
        PlayerEvents.OnPlayerSpawned += SetPlayer;
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerSpawned -= SetPlayer;
    }

    private void SetPlayer(GameObject playerObj)
    {
        player = playerObj.transform;
        Debug.Log("🔗 Obeliks player'ı aldı: " + player.name);
    }
    private void Start()
    {
        interactionPrompt.SetActive(false);
        closeButton.SetActive(false);
    }

    private void Update()
    {
        if (player == null || player.Equals(null))
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null)
                player = found.transform;
            return;
        }

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
            else
                return;
        }

        if (isInRange(playerTransform, transform, interactionRange))
        {
            interactionPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isDialogueActive)
                {
                    NextLine(); // Diyalog devam ediyorsa ilerlet
                }
                else
                {
                    Interact(); // Diyalog başlamamışsa başlat
                }
            }
        }
        else
        {
            interactionPrompt.SetActive(false);
        }
    }

    public void Interact()
    {
        if (dialogueData == null)
            return;

        StartDialogue();
    }

    public void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        closeButton.SetActive(true);
        interactionPrompt.SetActive(false);

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
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        closeButton.SetActive(false);
        interactionPrompt.SetActive(true);
    }

    private bool isInRange(Transform player, Transform target, float range)
    {
        float distance = Vector2.Distance(player.position, target.position);
        return distance <= range;
    }
    

}
