using System.Collections;
using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    public Animator laserAnimator; // Lazer animasyonunu oynatacak Animator
    public float fireInterval = 10f; // Lazerin kaç saniyede bir ateþleneceði

    void Start()
    {
        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser()
    {
        while (true) // Sonsuz döngü, oyun çalýþtýðý sürece lazer tetiklenecek
        {
            // Lazer animasyonunu tetikle
            //laserAnimator.SetTrigger("Fire"); //BUNU EKLEYECEÐÝM ÞUAN YAPAMADIM AMA GEREKLÝÝ OKEYYY
            laserAnimator.Play("laser");
            // Lazer ateþlendikten sonra fireInterval kadar bekle
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
