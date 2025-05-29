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

    public GameObject interactionText; // "E Basýn" yazýsý
    public GameObject fullScreenPanel; // Bilgi ekraný
    public TextMeshProUGUI infoText; // Bilgilendirici metin
    public Camera mainCamera; // Ana Kamera
    public float zoomInSize = 3f; // Yakýnlaþtýrma seviyesi
    public float zoomOutSize = 5f; // Normal kamera uzaklýðý
    public float zoomSpeed = 2f; // Yakýnlaþma/Uzaklaþma hýzý
    public float interactRange = 2.0f; // Etkileþim mesafesi
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
            mainCamera = Camera.main; // Eðer kamera atanmadýysa ana kamerayý al
        }

        // Kameranýn orijinal pozisyonunu ve büyüklüðünü kaydet
        originalPosition = mainCamera.transform.position;
        originalSize = mainCamera.orthographicSize;

        spriteRenderer.enabled = false;
        interactionText.SetActive(false); // Baþlangýçta "E" tuþu simgesi kapalý
        fullScreenPanel.SetActive(false); // Bilgi paneli baþlangýçta kapalý
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
        if (other.CompareTag("Player")) // Oyuncu yaklaþýnca
        {
            spriteRenderer.enabled = true; // Görünür yap
            animator.Play("obeliks_effects"); // Efekt animasyonunu baþlat
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Oyuncu uzaklaþýnca
        {
            animator.Play("obeliks"); // Efekt bittiðinde idle animasyonuna geç
            interactionText.SetActive(false); // Oyuncu uzaklaþýnca "E" simgesini kaldýr
            canInteract = false;
        }
    }

    // Idle animasyonu bittiðinde "E" simgesi gözüksün
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
            infoMessage = infoText.text;  // Bilgi metnini güncelle
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
        float duration = 1f / zoomSpeed; // Geçiþ süresi

        Vector3 startPosition = mainCamera.transform.position;
        float startSize = mainCamera.orthographicSize;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Kamera konumunu ve büyüklüðünü yumuþakça deðiþtir
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        // Son deðerleri tam olarak uygula
        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetSize;
    }
    // Diðer using'ler yukarýda
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
