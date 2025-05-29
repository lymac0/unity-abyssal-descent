using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;

public class Obeliks : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool canInteract = false;
    private bool isPanelOpen = false;

    public GameObject interactionText; // "E Bas�n" yaz�s�
    public GameObject fullScreenPanel; // Bilgi ekran�
    public TextMeshProUGUI infoText; // Bilgilendirici metin
    public Camera mainCamera; // Ana Kamera
    public float zoomInSize = 3f; // Yak�nla�t�rma seviyesi
    public float zoomOutSize = 5f; // Normal kamera uzakl���
    public float zoomSpeed = 2f; // Yak�nla�ma/Uzakla�ma h�z�
    public float interactRange = 2.0f; // Etkile�im mesafesi
    public Transform player; // Oyuncu karakteri

    private Vector3 originalPosition;
    private float originalSize;

    [TextArea(3, 10)]
    public string infoMessage; // Bilgi metni

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main; // E�er kamera atanmad�ysa ana kameray� al
        }

        // Kameran�n orijinal pozisyonunu ve b�y�kl���n� kaydet
        originalPosition = mainCamera.transform.position;
        originalSize = mainCamera.orthographicSize;

        spriteRenderer.enabled = false;
        interactionText.SetActive(false); // Ba�lang��ta "E" tu�u simgesi kapal�
        fullScreenPanel.SetActive(false); // Bilgi paneli ba�lang��ta kapal�
    }

    private void Update()
    {
        if (isInRange(player, transform, interactRange))
        {
            interactionText.SetActive(true);
            canInteract = true;
        }
        else
        {
            interactionText.SetActive(false);
            canInteract = false;
        }

        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            ToggleInfoPanel();
        }
    }

    private bool isInRange(Transform player, Transform target, float range)
    {
        float distance = Vector2.Distance(player.position, target.position);
        return distance <= range;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Oyuncu yakla��nca
        {
            spriteRenderer.enabled = true; // G�r�n�r yap
            animator.Play("obeliks_effects"); // Efekt animasyonunu ba�lat
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Oyuncu uzakla��nca
        {
            animator.Play("obeliks"); // Efekt bitti�inde idle animasyonuna ge�
            interactionText.SetActive(false); // Oyuncu uzakla��nca "E" simgesini kald�r
            canInteract = false;
        }
    }

    // Idle animasyonu bitti�inde "E" simgesi g�z�ks�n
    public void ShowInteractionText()
    {
        interactionText.SetActive(true);
        canInteract = true;
    }

    public void ToggleInfoPanel()
    {
        isPanelOpen = !isPanelOpen;
        fullScreenPanel.SetActive(isPanelOpen);

        if (isPanelOpen)
        {
            infoMessage = infoText.text;  // Bilgi metnini g�ncelle
            StopAllCoroutines();
            Vector3 zoomPosition = new Vector3(player.position.x, player.position.y, -10);
            StartCoroutine(SmoothZoom(zoomPosition, zoomInSize));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(SmoothZoom(originalPosition, originalSize));
        }
    }

    private IEnumerator SmoothZoom(Vector3 targetPosition, float targetSize)
    {
        float elapsedTime = 0f;
        float duration = 1f / zoomSpeed; // Ge�i� s�resi

        Vector3 startPosition = mainCamera.transform.position;
        float startSize = mainCamera.orthographicSize;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Kamera konumunu ve b�y�kl���n� yumu�ak�a de�i�tir
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        // Son de�erleri tam olarak uygula
        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetSize;
    }
    // Di�er using'ler yukar�da
    private void OnEnable()
    {
        PlayerEvents.OnPlayerSpawned += HandlePlayerSpawned;
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerSpawned -= HandlePlayerSpawned;
    }

    private void HandlePlayerSpawned(GameObject newPlayer)
    {
        player = newPlayer.transform;
    }

}
