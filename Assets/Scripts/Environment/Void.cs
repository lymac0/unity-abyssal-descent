using UnityEngine;
using UnityEngine.UIElements;

public class Void : MonoBehaviour
{
    public LayerMask whatIsPlayer;
    public AttackDetails attackDetails;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Oyuncu layer'indeyse
        if (((1 << collision.gameObject.layer) & whatIsPlayer) != 0)
        {
            attackDetails.damageAmount = 9999; // Anýnda öldürecek kadar yüksek
            attackDetails.position = transform.position;

            collision.transform.SendMessage("Damage", attackDetails);
        }
    }
}
