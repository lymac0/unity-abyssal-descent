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

        if (laserAnimator == null)
        {
            Debug.LogWarning("⚠️ Laser: Animator bileşeni eksik!");
        }

        if (laserCollider == null)
        {
            Debug.LogWarning("⚠️ Laser: Collider2D bileşeni eksik!");
        }
        else
        {
            laserCollider.enabled = false;
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
            }

            if (laserCollider != null)
            {
                laserCollider.enabled = true;
            }

            yield return new WaitForSeconds(1f); // Ateşleme süresi

            if (laserCollider != null)
            {
                laserCollider.enabled = false;
            }

            yield return new WaitForSeconds(fireInterval - 1f);
        }
    }
}
