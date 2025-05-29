using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public Transform respawnPoint;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float respawnTime;

    private float respawnTimeStart;

    private bool respawn;

    private CameraFollow cam;


    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
    }

    void Update()
    {
        CheckRespawn();
    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    private void CheckRespawn()
    {
        if (Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, respawnPoint);
            cam.target = playerTemp.transform;

            respawn = false;
        }
    }

}
