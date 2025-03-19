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
            laserCollider.enabled = false; // Ba�lang��ta kapal�
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
                    laserCollider.enabled = true; // Ate�leme an�nda Collider'� a�
                }
            }

            yield return new WaitForSeconds(1f); // Ate�leme s�resi

            if (laserCollider != null)
            {
                laserCollider.enabled = false; // Ate�leme bittikten sonra kapat
            }

            yield return new WaitForSeconds(fireInterval - 1f);
        }
    }
}
