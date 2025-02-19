using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
    public Transform player; // Oyuncunun Transform'u
    public Camera mainCamera; // Ana Kamera
    public float zoomInSize = 3f; // Yakýnlaþtýrma seviyesi
    public float zoomOutSize = 5f; // Normal kamera uzaklýðý
    public float zoomSpeed = 2f; // Yakýnlaþma/Uzaklaþma hýzý
    public Vector3 zoomOffset = new Vector3(0, 1, -10); // Kamera konum ofseti

    private bool isZoomed = false; // Kamera yakýn mý?
    private Vector3 originalPosition; // Kameranýn orijinal konumu
    private float originalSize; // Kameranýn orijinal zoom deðeri

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Eðer kamera atanmadýysa ana kamerayý al
        }

        // Kameranýn orijinal pozisyonunu ve büyüklüðünü kaydet
        originalPosition = mainCamera.transform.position;
        originalSize = mainCamera.orthographicSize;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleZoom();
        }
    }

    private void ToggleZoom()
    {
        if (isZoomed)
        {
            // Kamerayý eski haline döndür
            StopAllCoroutines();
            StartCoroutine(SmoothZoom(originalPosition, originalSize));
        }
        else
        {
            // Kamerayý oyuncuya yakýnlaþtýr
            StopAllCoroutines();
            Vector3 zoomPosition = player.position + zoomOffset; // Oyuncunun etrafýna odaklan
            StartCoroutine(SmoothZoom(zoomPosition, zoomInSize));
        }

        isZoomed = !isZoomed; // Zoom durumunu tersine çevir
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
