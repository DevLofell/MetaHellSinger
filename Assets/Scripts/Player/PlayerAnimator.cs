using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Rendering.LookDev;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private GameObject attackCollision;
    public Animator animator;

    //���� ���� Ȱ��ȭ�� ���� ����
    public GameObject skull;
    public GameObject sword;

    public CameraShake cameraShake;




    private void Awake()
    {
        


        sword.SetActive(false);
    }
    private void Update()
    {
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
        print("����");
    }

    public void OnSpeSwordAttack()
    {
        animator.SetTrigger("onSpeSwordAttack");
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
    public void OnSpeSwordState()
    {
        animator.SetInteger("weaponState", 2);
        print("����� �� ����");

        
    }
    public void OnAttackCollision(int attackNum) 
    {
        GetComponentInParent<Player>().SlashAni(attackNum);
        attackCollision.SetActive(true);
    }

    public void ChangeWeapon()
    {
        if (animator.GetInteger("weaponState") == 0)
        {
            //�˹��� �����϶� �ذ�� ������Ʈ�� Ȱ��ȭ�Ѵ�.
            sword.SetActive(true);
            //�ذ�� ������Ʈ�� ��Ȱ��ȭ�Ѵ�.
            skull.SetActive(false);
        }
        else if (animator.GetInteger("weaponState") == 1)
        {
            //�ذ񹫱� �����϶� �ذ� ������Ʈ�� Ȱ��ȭ�Ѵ�.
            skull.SetActive(true);
            //���� ������Ʈ�� ��Ȱ��ȭ �Ѵ�.
            sword.SetActive(false);
        }

    }
    public void GroggyAttack()
    {
        if (animator.GetInteger("weaponState") == 1)
        {
            //�ذ񹫱� �����϶� �ذ� ������Ʈ�� Ȱ��ȭ�Ѵ�.
            skull.SetActive(false);
            //���� ������Ʈ�� ��Ȱ��ȭ �Ѵ�.
            sword.SetActive(true);
        }
        animator.SetTrigger("GroggyAttack");

    }

    public void SlashCameraShake()
    {
        cameraShake.PlayShake(0.3f, 0.1f);
    }
}
