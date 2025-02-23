using System.Collections;
using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    public Animator laserAnimator; // Lazer animasyonunu oynatacak Animator
    public float fireInterval = 2f; // Lazerin kaç saniyede bir ateþleneceði

    void Start()
    {
        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser()
    {
        while (true) // Sonsuz döngü, oyun çalýþtýðý sürece lazer tetiklenecek
        {
            // Lazer animasyonunu tetikle
            laserAnimator.SetTrigger("Fire");

            // Lazer ateþlendikten sonra fireInterval kadar bekle
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
