using System.Collections;
using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    public Animator laserAnimator; // Lazer animasyonunu oynatacak Animator
    public float fireInterval = 2f; // Lazerin ka� saniyede bir ate�lenece�i

    void Start()
    {
        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser()
    {
        while (true) // Sonsuz d�ng�, oyun �al��t��� s�rece lazer tetiklenecek
        {
            // Lazer animasyonunu tetikle
            laserAnimator.SetTrigger("Fire");

            // Lazer ate�lendikten sonra fireInterval kadar bekle
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
