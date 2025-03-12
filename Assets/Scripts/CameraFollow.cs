using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f; // Kameranýn takip hýzý
    public Transform target; // Takip edilecek oyuncu
    public float yOffset = 2f; // Kamera yüksekliði ofseti
    public float minY = -2f; // Kameranýn inebileceði en alt sýnýr
    public float maxY = 4f; // Kameranýn çýkabileceði en üst sýnýr

    void LateUpdate()
    {
        if (target != null)
        {
            // Kamera pozisyonunu hesapla
            float targetX = target.position.x;
            float targetY = Mathf.Clamp(target.position.y + yOffset, minY, maxY); // Y sýnýrlarýný uygula

            Vector3 newPos = new Vector3(targetX, targetY, -10f); // Kameranýn yeni pozisyonu

            // Kamerayý yumuþak hareket ettir
            transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
        }
    }
}
