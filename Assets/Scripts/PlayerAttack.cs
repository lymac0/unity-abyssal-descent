using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    private Animator anim;
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
        StartCoroutine(Attack());
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPos.position, attackRange);
    }
    private IEnumerator Attack()
    {
        if (timeBtwAttack <= 0)
        {
            // then you can attack
            if (Input.GetKey(KeyCode.LeftControl))
            {
                anim.SetBool("isAttacking", true);
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, WhatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy1>().TakeDamage(damage);
                }
                yield return new WaitForSeconds(0.550f);
                anim.SetBool("isAttacking", false);
                Debug.Log(enemiesToDamage.Length);
                print("AMK");
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }
}
