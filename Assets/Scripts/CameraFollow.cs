using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f; // Kameranýn takip hýzý
    public Transform target; // Takip edilecek oyuncu
    public float yOffset = 2f; // Kamera yüksekliði ofseti
    public float minY = -2f; // Kameranýn inebileceði en alt sýnýr
    public float maxY = 4f; // Kameranýn çýkabileceði en üst sýnýr
    /*
    void LateUpdate()
    {
        if (target != null)
        {
            // Kamera pozisyonunu hesapla
            float targetX = target.position.x;
            float cameraHeight = Camera.main.orthographicSize;
            float targetY = Mathf.Clamp(target.position.y + yOffset, minY - cameraHeight, maxY + cameraHeight);


            Vector3 newPos = new Vector3(targetX, targetY, -10f); // Kameranýn yeni pozisyonu

            // Kamerayý yumuþak hareket ettir
            transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
        }
    }*/
    void LateUpdate()
    {
        if (target != null)
        {
            // Kamera pozisyonunu hesapla
            float targetX = target.position.x;
            float targetY = Mathf.Clamp(target.position.y + yOffset, minY, maxY);
            Vector3 newPos = new Vector3(targetX, targetY, -10f);

            // Harita sýnýrlarý dahilinde kamera hareketini sýnýrla
            float camHalfHeight = Camera.main.orthographicSize;
            float camHalfWidth = camHalfHeight * Camera.main.aspect;

            //float minXLimit = 0f + camHalfWidth; // Sol sýnýr (Tilemap'in sol sýnýrýna göre ayarla)
            //float maxXLimit = 100f - camHalfWidth; // Sað sýnýr (Tilemap'in geniþliðine göre ayarla)
            float minYLimit = -2.8f + camHalfHeight; // Alt sýnýr
            float maxYLimit = 10f - camHalfHeight; // Üst sýnýr

            //newPos.x = Mathf.Clamp(newPos.x, minXLimit, maxXLimit);
            newPos.y = Mathf.Clamp(newPos.y, minYLimit, maxYLimit);

            // Kamerayý yumuþak hareket ettir
            transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
        }
    }

}
