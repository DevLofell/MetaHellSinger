using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackCollision : MonoBehaviour
{
    public Animator animator;
    public int swordDamage = 330;
    public int swordThirdDamage = 670;

   


    private void OnEnable()
    {
        StartCoroutine("AutoDisable");
    }
    private void OnTriggerEnter(Collider other)
    {
        //현재 애니메이터의 상태를 불러온다.
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //트리거 안에 들어온게 Enemy라면
        if (other.CompareTag("Enemy"))
        {
            int damage;
            //애니메이터가 3번째 콤보일때
            if (stateInfo.IsName("Great Sword Slash3"))
            {
                //세번째 공격 데미지가 들어간다.
                damage = swordThirdDamage;
            }
            else
            {
                //그냥 검 공격 데미지가 들어간다.
                damage = swordDamage;

            }
            other.GetComponent<EnemyFsmJiwon>().HitEnemy(damage);
        }
    }
    private IEnumerator AutoDisable()
    {
        //0.1초 후에 오브젝트가 사라진다.
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }

   
}
