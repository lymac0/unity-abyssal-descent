using UnityEngine;
using System.Collections;

public class ElevatorTrigger : MonoBehaviour
{
    public Transform pointA; // Asans�r�n ba�lang�� noktas� (A�a��)
    public Transform pointB; // Asans�r�n biti� noktas� (Yukar�)
    public float speed = 0.5f; // Asans�r h�z� (Yava�lat�ld�)
    private bool goingUp = false;
    private bool goingDown = false;
    private bool isPlayerOnElevator = false; // Oyuncu asans�rde mi?
    private bool isMoving = false; // Asans�r �u anda hareket ediyor mu?

    void Update()
    {
        if (isMoving) return; // E�er asans�r zaten hareket ediyorsa yeni bir hareket ba�latma

        if (isPlayerOnElevator && goingUp)
        {
            StartCoroutine(MoveElevator(pointB.position)); // Yukar� ��k
        }
        else if (!isPlayerOnElevator && goingDown)
        {
            StartCoroutine(MoveElevator(pointA.position)); // A�a�� in
        }
    }

    IEnumerator MoveElevator(Vector3 target)
    {
        isMoving = true; // Hareket ba�lad���nda ba�ka hareket ba�lat�lmas�n

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
            yield return null; // Bir sonraki frame'i bekle
        }

        transform.position = target; // Konumu tam olarak hedefe ayarla
        isMoving = false; // Hareket tamamland�
    }

    // Oyuncu asans�re temas etti�inde �al���r
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
            goingUp = true; // Oyuncu bindi�inde yukar� ��kmaya haz�r

            // Asans�r yukar�daysa a�a�� inmeye ba�lamas�n
            if (Vector3.Distance(transform.position, pointB.position) < 0.1f)
            {
                goingDown = false;
            }
        }
    }

    // Oyuncu trigger'dan ��kt���nda �al���r
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnElevator = false;
            goingDown = true; // Oyuncu indi�inde a�a�� inmesi i�in aktif et
        }
    }
}
