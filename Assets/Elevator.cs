using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    public Transform pointA; // Asansörün baþlangýç noktasý
    public Transform pointB; // Asansörün bitiþ noktasý
    public float speed = 2f; // Asansör hýzý
    private bool goingUp = false; // Baþlangýçta hareketsiz
    private bool goingDown = false; // Baþlangýçta hareketsiz
    private bool isPlayerOnElevator = false; // Oyuncu asansörde mi?

    void Update()
    {
        // Oyuncu asansördeyken yukarý çýk
        if (isPlayerOnElevator && goingUp)
        {
            MoveElevatorUp();
        }

        // Oyuncu asansörden indikten sonra aþaðý in
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

    // Oyuncu asansörle çarpýþtýðýnda çalýþýr
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

            // Asansör yukarýda ise aþaðý inmeye baþlasýn
            if (Vector3.Distance(transform.position, pointB.position) < 0.1f)
            {
                goingDown = false;
            }
            else
            {
                goingUp = true; // Oyuncu bindiðinde asansörü çalýþtýr
            }
        }
    }

    // Oyuncu trigger'dan çýktýðýnda çalýþýr
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnElevator = false;

            // Oyuncu inince aþaðý inmeye baþlasýn
            if (Vector3.Distance(transform.position, pointA.position) > 0.1f)
            {
                goingDown = true;
            }
        }
    }
}
