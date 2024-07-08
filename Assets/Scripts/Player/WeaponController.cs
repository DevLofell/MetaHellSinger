using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject sword;
    public bool isAttack = false;
    public float attackpCoolDown = 0.5f;
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(isAttack == false)
            {
                SwordAttack();

            }
        }
    }
    public void SwordAttack()
    {
        isAttack = true;
        Animator anim = sword.GetComponent<Animator>();
        anim.SetTrigger("Attack1");
        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackpCoolDown);
        isAttack = false;
    }
}
