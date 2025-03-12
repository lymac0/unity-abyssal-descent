using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public Transform player; // Oyuncunun Transform'u
    public GameObject interactionText; // "E Basýn" yazýsý
    public GameObject fullScreenPanel; // Bilgi ekraný
    public TextMeshProUGUI infoText; // Bilgilendirici metin
    public Camera mainCamera; // Ana Kamera
    public float zoomInSize = 3f; // Yakýnlaþtýrma seviyesi
    public float zoomOutSize = 5f; // Normal kamera uzaklýðý
    public float zoomSpeed = 2f; // Yakýnlaþma/Uzaklaþma hýzý
    public float interactRange = 2.0f; // Etkileþim mesafesi

    private bool canInteract = false; // Oyuncu etkileþim mesafesinde mi?
    private bool isPanelOpen = false; // Panel açýk mý?
    private Vector3 originalPosition; // Kameranýn orijinal konumu
    private float originalSize; // Kameranýn orijinal zoom deðeri

    [TextArea(3, 10)]
    public string infoMessage; // Bilgi metni

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Eðer kamera atanmadýysa ana kamerayý al
        }

        // Kameranýn orijinal pozisyonunu ve büyüklüðünü kaydet
        originalPosition = mainCamera.transform.position;
        originalSize = mainCamera.orthographicSize;

        interactionText.SetActive(false); // Baþlangýçta "E Basýn" yazýsýný kapat
        fullScreenPanel.SetActive(false); // Bilgi panelini baþlangýçta kapalý tut
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

    public void ToggleInfoPanel()
    {
        isPanelOpen = !isPanelOpen; // Panelin açýk olup olmadýðýný tersine çevir
        fullScreenPanel.SetActive(isPanelOpen);

        if (isPanelOpen)
        {
            infoMessage = infoText.text; // Bilgi metnini güncelle
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

}
