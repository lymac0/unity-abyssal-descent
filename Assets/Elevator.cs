using UnityEngine;
using System.Collections;

public class ElevatorTrigger : MonoBehaviour
{
    public Transform pointA; // Asansörün baþlangýç noktasý (Aþaðý)
    public Transform pointB; // Asansörün bitiþ noktasý (Yukarý)
    public float speed = 0.5f; // Asansör hýzý (Yavaþlatýldý)
    private bool goingUp = false;
    private bool goingDown = false;
    private bool isPlayerOnElevator = false; // Oyuncu asansörde mi?
    private bool isMoving = false; // Asansör þu anda hareket ediyor mu?

    void Update()
    {
        if (isMoving) return; // Eðer asansör zaten hareket ediyorsa yeni bir hareket baþlatma

        if (isPlayerOnElevator && goingUp)
        {
            StartCoroutine(MoveElevator(pointB.position)); // Yukarý çýk
        }
        else if (!isPlayerOnElevator && goingDown)
        {
            StartCoroutine(MoveElevator(pointA.position)); // Aþaðý in
        }
    }

    IEnumerator MoveElevator(Vector3 target)
    {
        isMoving = true; // Hareket baþladýðýnda baþka hareket baþlatýlmasýn

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
            yield return null; // Bir sonraki frame'i bekle
        }

        transform.position = target; // Konumu tam olarak hedefe ayarla
        isMoving = false; // Hareket tamamlandý
    }

    // Oyuncu asansöre temas ettiðinde çalýþýr
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.parent = transform; // Oyuncuyu asansöre baðla
        }
    }

    // Oyuncu asansörden ayrýldýðýnda çalýþýr
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.parent = null; // Oyuncuyu asansörden ayýr
        }
    }

    // Oyuncu asansöre trigger ile girdiðinde çalýþýr
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnElevator = true;
            goingUp = true; // Oyuncu bindiðinde yukarý çýkmaya hazýr

            // Asansör yukarýdaysa aþaðý inmeye baþlamasýn
            if (Vector3.Distance(transform.position, pointB.position) < 0.1f)
            {
                goingDown = false;
            }
        }
    }

    // Oyuncu trigger'dan çýktýðýnda çalýþýr
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnElevator = false;
            goingDown = true; // Oyuncu indiðinde aþaðý inmesi için aktif et
        }
    }
}
