using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f; // Kameran�n takip h�z�
    public Transform target; // Takip edilecek oyuncu
    public float yOffset = 2f; // Kamera y�ksekli�i ofseti
    public float minY = -2f; // Kameran�n inebilece�i en alt s�n�r
    public float maxY = 4f; // Kameran�n ��kabilece�i en �st s�n�r
    /*
    void LateUpdate()
    {
        if (target != null)
        {
            // Kamera pozisyonunu hesapla
            float targetX = target.position.x;
            float cameraHeight = Camera.main.orthographicSize;
            float targetY = Mathf.Clamp(target.position.y + yOffset, minY - cameraHeight, maxY + cameraHeight);


            Vector3 newPos = new Vector3(targetX, targetY, -10f); // Kameran�n yeni pozisyonu

            // Kameray� yumu�ak hareket ettir
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

            // Harita s�n�rlar� dahilinde kamera hareketini s�n�rla
            float camHalfHeight = Camera.main.orthographicSize;
            float camHalfWidth = camHalfHeight * Camera.main.aspect;

            //float minXLimit = 0f + camHalfWidth; // Sol s�n�r (Tilemap'in sol s�n�r�na g�re ayarla)
            //float maxXLimit = 100f - camHalfWidth; // Sa� s�n�r (Tilemap'in geni�li�ine g�re ayarla)
            float minYLimit = -2.8f + camHalfHeight; // Alt s�n�r
            float maxYLimit = 10f - camHalfHeight; // �st s�n�r

            //newPos.x = Mathf.Clamp(newPos.x, minXLimit, maxXLimit);
            newPos.y = Mathf.Clamp(newPos.y, minYLimit, maxYLimit);

            // Kameray� yumu�ak hareket ettir
            transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
        }
    }

}
