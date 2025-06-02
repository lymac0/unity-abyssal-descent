using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private Animator laserAnimator;
    private Collider2D laserCollider;

    public float fireInterval = 3f;
    public Vector2 attackBoxSize = new Vector2(0.1f, 1.57f); // İnce uzun lazer
    public Vector2 attackBoxOffset = new Vector2(0f, -0.29f); // Kutunun merkezi lazer pozisyonuna göre kaydırılabilir
    public LayerMask whatIsPlayer;
    public AttackDetails attackDetails;

    void Start()
    {
        laserAnimator = GetComponent<Animator>();
        laserCollider = GetComponent<Collider2D>();

        if (laserAnimator == null)
            Debug.LogWarning("⚠️ Laser: Animator bileşeni eksik!");

        if (laserCollider == null)
            Debug.LogWarning("⚠️ Laser: Collider2D bileşeni eksik!");
        else
            laserCollider.enabled = false;

        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser()
    {
        while (true)
        {
            if (laserAnimator != null)
                laserAnimator.SetTrigger("Fire");

            if (laserCollider != null)
                laserCollider.enabled = true;

            DealDamage();

            yield return new WaitForSeconds(1f);

            if (laserCollider != null)
                laserCollider.enabled = false;

            yield return new WaitForSeconds(fireInterval - 1f);
        }
    }

    private void DealDamage()
    {
        Vector2 boxCenter = (Vector2)transform.position + (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * attackBoxOffset);
        float angle = transform.eulerAngles.z;

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, attackBoxSize, angle, whatIsPlayer);

        attackDetails.damageAmount = 50;

        foreach (Collider2D hit in hits)
        {
            hit.transform.SendMessage("Damage", attackDetails, SendMessageOptions.DontRequireReceiver);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 boxCenter = (Vector2)transform.position + (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * attackBoxOffset);
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, Quaternion.Euler(0, 0, transform.eulerAngles.z), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, attackBoxSize);
    }

}
