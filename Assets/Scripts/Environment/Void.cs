using UnityEngine;
using UnityEngine.UIElements;

public class Void : MonoBehaviour
{
    public LayerMask whatIsEnemy;
    public AttackDetails attackDetails;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Oyuncu layer'indeyse
        if (((1 << collision.gameObject.layer) & whatIsEnemy) != 0)
        {
            attackDetails.damageAmount = 9999; // Anýnda öldürecek kadar yüksek
            attackDetails.position = transform.position;

            if (collision.CompareTag("Player"))
            {
                collision.transform.SendMessage("Damage", attackDetails);
            }
            else if(collision.CompareTag("Enemy"))
            {
                collision.transform.parent.SendMessage("Damage", attackDetails);
            }
        }
    }
}
