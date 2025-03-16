using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private bool isAttacking;
    private int attackStep = 0;
    public Transform attackPos;
    public LayerMask WhatIsEnemies;
    public float attackRange;
    public int damage;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isAttacking)
        {
            if (attackStep > 2)
            {
                ResetAttack();
            }
            attackStep++;
            Attack();
        }
    }

    private void Attack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        anim.SetInteger("AttackStep", attackStep);
        DealDamage();
    }

    // Animation Event çağırır
    private void DealDamage()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, WhatIsEnemies);
        foreach (var enemy in enemiesToDamage)
        {
            enemy.GetComponent<Enemy1>().TakeDamage(damage);
        }
    }

    // Animasyonun sonunda çağrılır
    private void ResetAttack()
    {
        isAttacking = false;
        attackStep = 0; // Komboyu sıfırla
        anim.SetInteger("AttackStep", 0);
    }

    void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(attackPos.position, attackRange);
    }

}
