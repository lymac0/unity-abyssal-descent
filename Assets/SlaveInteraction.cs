using UnityEngine;
using System.Collections;

public class SlaveInteraction : MonoBehaviour
{
    public Transform player;
    public float interactionRange = 4f;
    public LayerMask playerLayer;
    public string npcName = "Tutsak";
    [TextArea(2, 5)] public string[] firstDialogue;
    [TextArea(2, 5)] public string[] secondDialogue;

    public GameObject cageDoor;
    public GameObject brokenEffect;
    public Transform walkTarget;
    public float walkSpeed = 2f;

    private bool hasTalkedOnce = false;
    private bool hasBeenFreed = false;
    private int dialogueIndex = 0;
    private bool isTyping = false;
    private bool isWalking = false;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        // 🧠 PLAYER BUL
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogError("❌ Player tag'li obje sahnede bulunamadı!");
        }

        if (UIManager.Instance == null)
            {
            Debug.LogError("❌ UIManager sahnede bulunamadı!");
            }
        else
            {
            // Canvas veya panel inaktifse burada aktif et
            UIManager.Instance.dialoguePanel.transform.parent.gameObject.SetActive(true);
            UIManager.Instance.closeButton.onClick.AddListener(NextLine);
            }
    }

    void Update()
    {
        if (isWalking)
        {
            transform.position = Vector2.MoveTowards(transform.position, walkTarget.position, walkSpeed * Time.deltaTime);
            return;
        }

        

        if (Vector2.Distance(transform.position, player.position) <= interactionRange && !hasBeenFreed)
        {
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("🟢 E tuşuna basıldı, mesafeye bakılıyor.");
                if (!hasTalkedOnce)
                {
                    Debug.Log("🟡 StartDialogue çağrılıyor.");
                    StartDialogue(firstDialogue);
                    hasTalkedOnce = true;
                }
            }

        }

        // Eğer diyalog açıkken E'ye basıldıysa bir sonraki satıra geç
        if ((hasTalkedOnce || hasBeenFreed) && UIManager.Instance.IsDialogueActive() && Input.GetKeyDown(KeyCode.E))
        {
            NextLine();
        }

    }

    public void BreakCage() // CageDoor.cs çağırır
    {
        if (hasBeenFreed) return;

        hasBeenFreed = true;

        if (cageDoor != null)
        {
            if (brokenEffect != null)
            {
                Quaternion rotated = Quaternion.Euler(0, 0, 90);
                Instantiate(brokenEffect, cageDoor.transform.position, rotated);
            }
            Destroy(cageDoor);
        }

        StartDialogue(secondDialogue);
    }

    void StartDialogue(string[] lines)
    {
        if (UIManager.Instance == null || isTyping) return;

        dialogueIndex = 0;
        StartCoroutine(TypeLine(lines));
    }

    void NextLine()
    {
        if (isTyping) return;

        dialogueIndex++;

        string[] currentLines = hasBeenFreed ? secondDialogue : firstDialogue;

        if (dialogueIndex < currentLines.Length)
        {
            StartCoroutine(TypeLine(currentLines));
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine(string[] lines)
    {
        isTyping = true;
        UIManager.Instance.ShowDialogue(npcName, "");

        foreach (char c in lines[dialogueIndex])
        {
            UIManager.Instance.dialogueText.text += c;
            yield return new WaitForSeconds(0.025f);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        UIManager.Instance.HideDialogue();

        if (hasBeenFreed)
        {
            isWalking = true;
            if (anim != null)
                anim.SetBool("isWalking", true);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
