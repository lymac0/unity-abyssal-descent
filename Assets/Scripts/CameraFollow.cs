using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f; // Kameran�n takip h�z�
    public Transform target; // Takip edilecek oyuncu
    public float yOffset = 2f; // Kamera y�ksekli�i ofseti
    public float minY = -2f; // Kameran�n inebilece�i en alt s�n�r
    public float maxY = 4f; // Kameran�n ��kabilece�i en �st s�n�r

    void LateUpdate()
    {
        if (target != null)
        {
            // Kamera pozisyonunu hesapla
            float targetX = target.position.x;
            float targetY = Mathf.Clamp(target.position.y + yOffset, minY, maxY); // Y s�n�rlar�n� uygula

            Vector3 newPos = new Vector3(targetX, targetY, -10f); // Kameran�n yeni pozisyonu

            // Kameray� yumu�ak hareket ettir
            transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
        }
    }
}
