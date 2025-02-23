using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    public Transform pointA; // Asans�r�n ba�lang�� noktas�
    public Transform pointB; // Asans�r�n biti� noktas�
    public float speed = 2f; // Asans�r h�z�
    private bool goingUp = false; // Ba�lang��ta hareketsiz
    private bool goingDown = false; // Ba�lang��ta hareketsiz
    private bool isPlayerOnElevator = false; // Oyuncu asans�rde mi?

    void Update()
    {
        // Oyuncu asans�rdeyken yukar� ��k
        if (isPlayerOnElevator && goingUp)
        {
            MoveElevatorUp();
        }

        // Oyuncu asans�rden indikten sonra a�a�� in
        if (!isPlayerOnElevator && goingDown)
        {
            MoveElevatorDown();
        }
    }

    void MoveElevatorUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, pointB.position) < 0.1f)
        {
            goingUp = false;
        }
    }

    void MoveElevatorDown()
    {
        transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, pointA.position) < 0.1f)
        {
            goingDown = false;
        }
    }

    // Oyuncu asans�rle �arp��t���nda �al���r
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.parent = transform; // Oyuncuyu asans�re ba�la
        }
    }

    // Oyuncu asans�rden ayr�ld���nda �al���r
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.parent = null; // Oyuncuyu asans�rden ay�r
        }
    }

    // Oyuncu asans�re trigger ile girdi�inde �al���r
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnElevator = true;

            // Asans�r yukar�da ise a�a�� inmeye ba�las�n
            if (Vector3.Distance(transform.position, pointB.position) < 0.1f)
            {
                goingDown = false;
            }
            else
            {
                goingUp = true; // Oyuncu bindi�inde asans�r� �al��t�r
            }
        }
    }

    // Oyuncu trigger'dan ��kt���nda �al���r
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnElevator = false;

            // Oyuncu inince a�a�� inmeye ba�las�n
            if (Vector3.Distance(transform.position, pointA.position) > 0.1f)
            {
                goingDown = true;
            }
        }
    }
}
