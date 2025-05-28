using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public Transform target;
    public float yOffset = 2f;
    public float minY = -2f;
    public float maxY = 4f;

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
        if (newPlayer != null)
            target = newPlayer.transform;
    }

    void Start()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("📷 CameraFollow: Player bulunamadı!");
            }
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            float targetX = target.position.x;
            float targetY = Mathf.Clamp(target.position.y + yOffset, minY, maxY);
            Vector3 newPos = new Vector3(targetX, targetY, -10f);

            float camHalfHeight = Camera.main.orthographicSize;
            float camHalfWidth = camHalfHeight * Camera.main.aspect;

            float minYLimit = -2.8f + camHalfHeight;
            float maxYLimit = 10f - camHalfHeight;

            newPos.y = Mathf.Clamp(newPos.y, minYLimit, maxYLimit);

            transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
        }
    }
}
