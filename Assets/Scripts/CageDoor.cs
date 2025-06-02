using UnityEngine;

public class CageDoor : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    private float currentHealth;

    public SlaveInteraction slaveScript;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(AttackDetails attackDetails)
    {
        currentHealth -= attackDetails.damageAmount;

        if(currentHealth <= 0)
        {
            GameObject slave = GameObject.FindWithTag("Slave");
            if (slave != null)
                slaveScript.BreakCage();
        }
    }
}
