using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interactable : MonoBehaviour
{
    public Transform player;
    public GameObject interactionText;
    public GameObject fullScreenPanel;
    public TextMeshProUGUI infoText;
    public Camera mainCamera;
    public float zoomInSize = 3f;
    public float zoomOutSize = 5f;
    public float interactRange = 2.0f;
    public Button closeButton;

    private bool canInteract = false;
    private bool isPanelOpen = false;
    private Vector3 originalPosition;
    private float originalSize;

    [TextArea(3, 10)]
    public string infoMessage;

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

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        originalPosition = mainCamera.transform.position;
        originalSize = mainCamera.orthographicSize;

        interactionText.SetActive(false);
        fullScreenPanel.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseInfoPanel);

        if (infoText != null && !string.IsNullOrEmpty(infoMessage))
            infoText.text = infoMessage;
    }

    private void Update()
    {
        if (player == null) return;

        if (IsInRange(player, transform, interactRange))
        {
            interactionText.SetActive(!isPanelOpen);
            canInteract = true;
        }
        else
        {
            interactionText.SetActive(false);
            canInteract = false;
        }

        if (canInteract && Input.GetKeyDown(KeyCode.E))
            ToggleInfoPanel();
    }

    private bool IsInRange(Transform player, Transform target, float range)
    {
        float distance = Vector2.Distance(player.position, target.position);
        return distance <= range;
    }

    public void ToggleInfoPanel()
    {
        isPanelOpen = !isPanelOpen;
        fullScreenPanel.SetActive(isPanelOpen);

        if (isPanelOpen)
        {
            interactionText.SetActive(false);
            InstantZoom(new Vector3(player.position.x, player.position.y, -10), zoomInSize);
            infoMessage = infoText.text;
        }
        else
        {
            CloseInfoPanel();
        }
    }

    public void CloseInfoPanel()
    {
        isPanelOpen = false;
        fullScreenPanel.SetActive(false);
        interactionText.SetActive(true);
        InstantZoom(originalPosition, originalSize);
    }

    private void InstantZoom(Vector3 targetPosition, float targetSize)
    {
        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetSize;
    }
}
