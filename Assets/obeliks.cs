using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;

public class Obeliks : MonoBehaviour, IPlayerDependent
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool canInteract = false;
    private bool isPanelOpen = false;

    public GameObject interactionText; // "E Basın" yazısı
    public GameObject fullScreenPanel; // Bilgi ekranı
    public TextMeshProUGUI infoText; // Bilgilendirici metin
    public Camera mainCamera; // Ana Kamera
    public float zoomInSize = 3f; // Yakınlaştırma seviyesi
    public float zoomOutSize = 5f; // Normal kamera uzaklığı
    public float zoomSpeed = 2f; // Yakınlaşma/Uzaklaşma hızı
    public float interactRange = 2.0f; // Etkileşim mesafesi
    public Transform player; // Oyuncu karakteri

    private Vector3 originalPosition;
    private float originalSize;

    [TextArea(3, 10)]
    public string infoMessage; // Bilgi metni


    public void SetPlayer(Transform playerTransform)
    {
        this.player = playerTransform;
        Debug.Log("Obeliks player referansı atandı.");
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Eğer kamera atanmadıysa ana kamerayı al
        }

        // Kameranın orijinal pozisyonunu ve büyüklüğünü kaydet
        originalPosition = mainCamera.transform.position;
        originalSize = mainCamera.orthographicSize;

        spriteRenderer.enabled = false;
        interactionText.SetActive(false); // Başlangıçta "E" tuşu simgesi kapalı
        fullScreenPanel.SetActive(false); // Bilgi paneli başlangıçta kapalı
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
        // ❗ interactionText'e güvenli erişim
        if (interactionText != null && !interactionText.Equals(null))
        {
            float distance = Vector2.Distance(player.position, transform.position);
            interactionText.SetActive(distance < 2f);
        }

        // ✅ Player varsa normal davranışa devam et
        if (isInRange(player, transform, 2f))
        {
            // etkileşim, yazı gösterme, vs.
        }
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
        if (other.CompareTag("Player")) // Oyuncu yaklaşınca
        {
            spriteRenderer.enabled = true; // Görünür yap
            animator.Play("obeliks_effects"); // Efekt animasyonunu başlat
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Oyuncu uzaklaşınca
        {
            animator.Play("obeliks"); // Efekt bittiğinde idle animasyonuna geç
            interactionText.SetActive(false); // Oyuncu uzaklaşınca "E" simgesini kaldır
            canInteract = false;
        }
    }

    // Idle animasyonu bittiğinde "E" simgesi gözüksün
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
        float duration = 1f / zoomSpeed; // Geçiş süresi

        Vector3 startPosition = mainCamera.transform.position;
        float startSize = mainCamera.orthographicSize;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Kamera konumunu ve büyüklüğünü yumuşakça değiştir
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        // Son değerleri tam olarak uygula
        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetSize;
    }
    

    private void HandlePlayerSpawned(GameObject newPlayer)
    {
        player = newPlayer.transform;
    }

}
