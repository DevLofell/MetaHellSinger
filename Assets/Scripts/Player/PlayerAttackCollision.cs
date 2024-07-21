using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackCollision : MonoBehaviour
{
    public Animator animator;
    public int swordDamage = 330;
    public int swordThirdDamage = 670;
    int damage;
   


    private void OnEnable()
    {
        StartCoroutine("AutoDisable");
    }
    private void OnTriggerEnter(Collider other)
    {
        //���� �ִϸ������� ���¸� �ҷ��´�.
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //Ʈ���� �ȿ� ���°� Enemy���

        if (other.CompareTag("Enemy"))
        {
            //�ִϸ����Ͱ� 3��° �޺��϶�
            if (stateInfo.IsName("Great Sword Slash3"))
            {
                print("�� ����° ��");
                //����° ���� �������� ����.
                damage = swordThirdDamage;
            }
            else
            {
                print("�� �⺻ ��");

                //�׳� �� ���� �������� ����.
                damage = swordDamage;

            }
            
            other.GetComponent<EnemyFsmJiwon>().HitEnemy(damage);

        }
        else if(other.CompareTag("Boss"))
        {
            Player.instance.UpdateMP(0.1f);
            //�ִϸ����Ͱ� 3��° �޺��϶�
            if (stateInfo.IsName("Great Sword Slash3"))
            {
                //����° ���� �������� ����.
                damage = swordThirdDamage;
            }
            else
            {
                //�׳� �� ���� �������� ����.
                damage = swordDamage;

            }

            other.GetComponent<BossFSM>().HitBoss(damage);
        }
    }
    private IEnumerator AutoDisable()
    {
        //0.1�� �Ŀ� ������Ʈ�� �������.
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }

   
}
