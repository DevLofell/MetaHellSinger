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
        //���� �ִϸ������� ���¸� �ҷ��´�.
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //Ʈ���� �ȿ� ���°� Enemy���
        if (other.CompareTag("Enemy"))
        {
            //�ִϸ����Ͱ� 3��° �޺��϶�
            if (stateInfo.IsName("Great Sword Slash3"))
            {
                //����° ���� �������� ����.
                other.GetComponent<EnemyController_GH>().TakeDamage(swordThirdDamage);
                EnemyController_GH enemeyCon = other.GetComponent<EnemyController_GH>();
                enemeyCon.OnDamageUI(swordThirdDamage);
            }
            else

            {
                //�׳� �� ���� �������� ����.
                other.GetComponent<EnemyController_GH>().TakeDamage(swordDamage);
                EnemyController_GH enemeyCon = other.GetComponent<EnemyController_GH>();
                enemeyCon.OnDamageUI(swordDamage);

            }
        }
    }
    private IEnumerator AutoDisable()
    {
        //0.1�� �Ŀ� ������Ʈ�� �������.
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }

   
}
