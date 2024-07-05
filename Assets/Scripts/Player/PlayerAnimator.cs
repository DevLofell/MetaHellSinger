using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private GameObject attackCollision;
    public Animator animator;

    //���� ���� Ȱ��ȭ�� ���� ����
    GameObject skull;
    GameObject sword;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        //�ذ񹫱⸦ ã�´�
        skull = GameObject.Find("Skull");
        //�ذ񹫱⸦ ����.

        //���� ã�´�.
        sword = GameObject.Find("Sword");
        sword.SetActive(false);
    }

    //Į���� ���� �ִϸ��̼� ����(�⺻)
    public void OnMovement(float horizontal, float vertical)
    {
        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);
    }
    //�ذ��� �� ���� �ִϸ��̼� ����
    public void OnSwordAttack()
    {
        animator.SetTrigger("onSwordAttack");
    }

    //���Ÿ� �������� ��ȯ
    public void OnFireState()
    {
        animator.SetInteger("weaponState", 1);
        //�ذ񹫱� �����϶� �ذ� ������Ʈ�� Ȱ��ȭ�Ѵ�.
        skull.SetActive(true);
        //���� ������Ʈ�� ��Ȱ��ȭ �Ѵ�.
        sword.SetActive(false);


    }
    //�ٰŸ� �������� ��ȯ
    public void OnSwordState()
    {
        animator.SetInteger("weaponState", 0);
        //�˹��� �����϶� �ذ�� ������Ʈ�� Ȱ��ȭ�Ѵ�.
        sword.SetActive(true);
        //�ذ�� ������Ʈ�� ��Ȱ��ȭ�Ѵ�.
        skull.SetActive(false);

    }
    public void OnAttackCollision() 
    {
        attackCollision.SetActive(true);
    }
}
