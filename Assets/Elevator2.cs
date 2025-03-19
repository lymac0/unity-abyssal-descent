using UnityEngine;

public class Elevator2Trigger : MonoBehaviour
{
    public Transform pointA; // Asans�r�n en alt noktas�
    public Transform pointB; // Asans�r�n en �st noktas�
    public float speed = 2f; // Asans�r�n h�z�

    private bool isMoving = false; // Asans�r hareket halinde mi?
    private bool isPlayerOnElevator = false; // Oyuncu asans�rde mi?

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
            // Oyuncu bindi�inde asans�r a�a�� inmeli
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, pointA.position) < 0.1f)
            {
                isMoving = false; // Asans�r durdu
            }
        }
        else
        {
            // Oyuncu indikten sonra asans�r yukar� ��kmal�
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, pointB.position) < 0.1f)
            {
                isMoving = false; // Asans�r durdu
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnElevator = true;
            isMoving = true; // Oyuncu bindi�inde asans�r a�a�� inmeye ba�las�n
            collision.transform.parent = transform; // Oyuncuyu asans�re ba�la
            Debug.Log("Oyuncu asans�re bindi, a�a�� inmeye ba�l�yor.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnElevator = false;
            isMoving = true; // Oyuncu inince yukar� ��kmaya ba�las�n
            collision.transform.parent = null; // Oyuncuyu asans�rden ay�r
            Debug.Log("Oyuncu asans�rden indi, yukar� ��kmaya ba�l�yor.");
        }
    }
}
