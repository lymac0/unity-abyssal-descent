using UnityEngine;

public class Elevator2Trigger : MonoBehaviour
{
    public Transform pointA; // Asansörün en alt noktasý
    public Transform pointB; // Asansörün en üst noktasý
    public float speed = 2f; // Asansörün hýzý

    private bool isMoving = false; // Asansör hareket halinde mi?
    private bool isPlayerOnElevator = false; // Oyuncu asansörde mi?

    void Update()
    {
        if (isMoving)
        {
            MoveElevator();
        }
    }

    void MoveElevator()
    {
        if (isPlayerOnElevator)
        {
            // Oyuncu bindiðinde asansör aþaðý inmeli
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, pointA.position) < 0.1f)
            {
                isMoving = false; // Asansör durdu
            }
        }
        else
        {
            // Oyuncu indikten sonra asansör yukarý çýkmalý
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, pointB.position) < 0.1f)
            {
                isMoving = false; // Asansör durdu
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnElevator = true;
            isMoving = true; // Oyuncu bindiðinde asansör aþaðý inmeye baþlasýn
            collision.transform.parent = transform; // Oyuncuyu asansöre baðla
            Debug.Log("Oyuncu asansöre bindi, aþaðý inmeye baþlýyor.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnElevator = false;
            isMoving = true; // Oyuncu inince yukarý çýkmaya baþlasýn
            collision.transform.parent = null; // Oyuncuyu asansörden ayýr
            Debug.Log("Oyuncu asansörden indi, yukarý çýkmaya baþlýyor.");
        }
    }
}
