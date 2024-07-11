using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private GameObject attackCollision;
    public Animator animator;

    //무기 상태 활성화를 위한 변수
    public GameObject skull;
    public GameObject sword;


    private void Awake()
    {
        


        sword.SetActive(false);
    }

    //칼을든 무빙 애니메이션 구현(기본)
    public void OnMovement(float horizontal, float vertical)
    {
        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);
    }
    //해골을 든 무빙 애니메이션 구현
    public void OnSwordAttack()
    {
        animator.SetTrigger("onSwordAttack");
        print("공격");
    }

    public void OnSpeSwordAttack()
    {
        animator.SetTrigger("onSpeSwordAttack");
    }

    //원거리 공격으로 전환
    public void OnFireState()
    {
        animator.SetInteger("weaponState", 1);
        //해골무기 상태일때 해골 오브젝트를 활성화한다.
        skull.SetActive(true);
        //검을 오브젝트를 비활성화 한다.
        sword.SetActive(false);
    }

    //근거리 공격으로 전환
    public void OnSwordState()
    {
        animator.SetInteger("weaponState", 0);
        //검무기 상태일때 해골검 오브젝트를 활성화한다.
        sword.SetActive(true);
        //해골검 오브젝트를 비활성화한다.
        skull.SetActive(false);

    }
    public void OnSpeSwordState()
    {
        animator.SetInteger("weaponState", 2);
        print("스페셜 검 상태");
        Invoke("OnSwordState", 5f);
    }
    public void OnAttackCollision() 
    {
        attackCollision.SetActive(true);
    }

    public void ChangeWeapon()
    {
        if (animator.GetInteger("weaponState") == 0)
        {
            //검무기 상태일때 해골검 오브젝트를 활성화한다.
            sword.SetActive(true);
            //해골검 오브젝트를 비활성화한다.
            skull.SetActive(false);
        }
        else if (animator.GetInteger("weaponState") == 1)
        {
            //해골무기 상태일때 해골 오브젝트를 활성화한다.
            skull.SetActive(true);
            //검을 오브젝트를 비활성화 한다.
            sword.SetActive(false);
        }

    }
    public void GroggyAttack()
    {
        if (animator.GetInteger("weaponState") == 1)
        {
            //해골무기 상태일때 해골 오브젝트를 활성화한다.
            skull.SetActive(false);
            //검을 오브젝트를 비활성화 한다.
            sword.SetActive(true);
        }
        animator.SetTrigger("GroggyAttack");

    }
}
