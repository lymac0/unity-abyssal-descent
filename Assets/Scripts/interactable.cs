using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public Transform player; // Oyuncunun Transform'u
    public GameObject interactionText; // "E Bas�n" yaz�s�
    public GameObject fullScreenPanel; // Bilgi ekran�
    public TextMeshProUGUI infoText; // Bilgilendirici metin
    public Camera mainCamera; // Ana Kamera
    public float zoomInSize = 3f; // Yak�nla�t�rma seviyesi
    public float zoomOutSize = 5f; // Normal kamera uzakl���
    public float zoomSpeed = 2f; // Yak�nla�ma/Uzakla�ma h�z�
    public float interactRange = 2.0f; // Etkile�im mesafesi

    private bool canInteract = false; // Oyuncu etkile�im mesafesinde mi?
    private bool isPanelOpen = false; // Panel a��k m�?
    private Vector3 originalPosition; // Kameran�n orijinal konumu
    private float originalSize; // Kameran�n orijinal zoom de�eri

    [TextArea(3, 10)]
    public string infoMessage; // Bilgi metni

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // E�er kamera atanmad�ysa ana kameray� al
        }

        // Kameran�n orijinal pozisyonunu ve b�y�kl���n� kaydet
        originalPosition = mainCamera.transform.position;
        originalSize = mainCamera.orthographicSize;

        interactionText.SetActive(false); // Ba�lang��ta "E Bas�n" yaz�s�n� kapat
        fullScreenPanel.SetActive(false); // Bilgi panelini ba�lang��ta kapal� tut
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
        isPanelOpen = !isPanelOpen; // Panelin a��k olup olmad���n� tersine �evir
        fullScreenPanel.SetActive(isPanelOpen);

        if (isPanelOpen)
        {
            infoMessage = infoText.text; // Bilgi metnini g�ncelle
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

}
