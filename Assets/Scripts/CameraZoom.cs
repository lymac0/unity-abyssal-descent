using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
    public Transform player; // Oyuncunun Transform'u
    public Camera mainCamera; // Ana Kamera
    public float zoomInSize = 3f; // Yak�nla�t�rma seviyesi
    public float zoomOutSize = 5f; // Normal kamera uzakl���
    public float zoomSpeed = 2f; // Yak�nla�ma/Uzakla�ma h�z�
    public Vector3 zoomOffset = new Vector3(0, 1, -10); // Kamera konum ofseti

    private bool isZoomed = false; // Kamera yak�n m�?
    private Vector3 originalPosition; // Kameran�n orijinal konumu
    private float originalSize; // Kameran�n orijinal zoom de�eri

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // E�er kamera atanmad�ysa ana kameray� al
        }

        // Kameran�n orijinal pozisyonunu ve b�y�kl���n� kaydet
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
            // Kameray� eski haline d�nd�r
            StopAllCoroutines();
            StartCoroutine(SmoothZoom(originalPosition, originalSize));
        }
        else
        {
            // Kameray� oyuncuya yak�nla�t�r
            StopAllCoroutines();
            Vector3 zoomPosition = player.position + zoomOffset; // Oyuncunun etraf�na odaklan
            StartCoroutine(SmoothZoom(zoomPosition, zoomInSize));
        }

        isZoomed = !isZoomed; // Zoom durumunu tersine �evir
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
}
