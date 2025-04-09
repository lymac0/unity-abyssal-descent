using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private Animator laserAnimator;
    private Collider2D laserCollider;
    public float fireInterval = 3f;

    void Start()
    {
        laserAnimator = GetComponent<Animator>();
        laserCollider = GetComponent<Collider2D>();

        if (laserCollider != null)
        {
            laserCollider.enabled = false; // Baþlangýçta kapalý
        }

        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser()
    {
        while (true)
        {
            if (laserAnimator != null)
            {
                laserAnimator.SetTrigger("Fire");

                if (laserCollider != null)
                {
                    laserCollider.enabled = true; // Ateþleme anýnda Collider'ý aç
                }
            }

            yield return new WaitForSeconds(1f); // Ateþleme süresi

            if (laserCollider != null)
            {
                laserCollider.enabled = false; // Ateþleme bittikten sonra kapat
            }

            yield return new WaitForSeconds(fireInterval - 1f);
        }
    }
}
