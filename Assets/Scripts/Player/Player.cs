using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public class Player : MonoBehaviour
{
    public static Player instance;

    //플레이어 체력
    public float currHP = 0;
    public float maxHP = 1000;

    //플레이어 마나
    public float currSkullMP = 0;
    public float currSwordMP = 0;
    public float maxMP = 1f;

    //즉살 공격
    bool onOneShot = false;
    EnemyFsmJiwon nearestEnemy = null;
    float speTime = 0;
    public float oneShotDistance = 50;

    //스컬 특수공격
    public LayerMask enemyLayer;
    public float stunRadius = 5.0f;
    public GameObject stunEffectFactory;
    public GameObject stunEffectPos;
    public float swordSKillTime = 5.0f;


    #region 플레이어 기본 움직임
    //플레이어 회전값
    public float rotSpeed = 200f;

    //플레이어,,
    float mx = 0;

    //플레이어 속도
    public float moveSpeed = 7f;

    //방향
    public Vector3 dir;

    CharacterController cc;
    #endregion

    #region 플레이어 점프
    //중력값
    public float gravity = -4f;
    //플레이어 중력 값
    public float yVelocity = 0;
    //점프높이
    public float jumpPower = 1.7f;
    //점프 상태
    public bool isJumping = false;
    //점프 횟수
    int nowJumpCount = 0;
    //최대 점프 횟수
    int maxJumpCount = 2;

    #endregion

    //스컬 공격
    //스컬 공격 프리팹
    public GameObject fireBallFactory;
    //스컬 공격 위치
    public GameObject fireBallPos;
    //스컬 공격 방향
    public Vector3 fireBallDir;

    //플레이어 애니메이터
    private PlayerAnimator playerAnimator;
    //칼질 애니메이션
    public List<GameObject> slashEffectList;
    //칼질 애니메이션 위치
    public GameObject slashPos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
        currHP = maxHP;
    }

    void Update()
    {
        if (onOneShot)
        {

            speTime += Time.deltaTime;
            //적과 플레이어의 거리
            float distances = Vector3.Distance(transform.position, nearestEnemy.transform.position);
            if (distances > 2.5f)
            {
                Vector3 spedir = nearestEnemy.transform.position - transform.position;
                spedir.Normalize();
                transform.Translate(spedir * 100 * Time.deltaTime, Space.World);
                print("원샷 함수 실행3");


            }
            else
            {

            }

            if (speTime > 1)
            {
                onOneShot = false;
                speTime = 0;
                nearestEnemy = null;

            }



        }
        else
        {
            PlayerMove();
            PlayerRotate();
            PlayerFire();

            //스페셜 검 상태 게이지 낮추기
            if (playerAnimator.animator.GetInteger("weaponState") == 2)
            {
                if (currSwordMP > 0)
                {
                    currSwordMP -= (Time.deltaTime / swordSKillTime);
                }
                else if(currSwordMP <= 0)
                {
                    currSwordMP = 0;
                    playerAnimator.OnSwordState();
                }

            }
        }
    }

    void PlayerRotate()
    {

        float mouseX = Input.GetAxis("Mouse X");

        mx += mouseX * rotSpeed * Time.deltaTime;


        transform.eulerAngles = new Vector3(0, mx, 0);
    }

    void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        dir = new Vector3(h, 0, v);

        float scalar = dir.magnitude;


        if (scalar > 1)
        {
            scalar = 1f;
        }

        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        dir = dir.normalized * scalar;


        if (!cc.isGrounded)
        {

            yVelocity += gravity * Time.deltaTime;

        }
        else
        {
            yVelocity = 0;
        }




        if (cc.collisionFlags == CollisionFlags.Below && isJumping)
        {

            isJumping = false;
            yVelocity = 0;
            nowJumpCount = 0;
        }



        if (Input.GetButtonDown("Jump") && nowJumpCount < maxJumpCount)
        {
            yVelocity = jumpPower;
            isJumping = true;
            nowJumpCount++;
        }


        dir.y = yVelocity;


        cc.Move(dir * moveSpeed * Time.deltaTime);

        playerAnimator.OnMovement(h, v);

    }


    public void PlayerFire()
    {
        #region 마우스 왼쪽 버튼 - 무기 별 기본 공격
        if (Input.GetButtonDown("Fire1"))
        {
            if (playerAnimator.animator.GetInteger("weaponState") == 0)
            {
                //칼질 애니메이션
                playerAnimator.OnSwordAttack();
                //칼질 이펙트
                //SlashAni();
                onOneShot = false;
            }
            if (playerAnimator.animator.GetInteger("weaponState") == 1)
            {
                #region 스컬 공격
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

                RaycastHit hitInfo = new RaycastHit();

                GameObject fireBall = Instantiate(fireBallFactory);
                fireBall.transform.position = fireBallPos.transform.position;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    fireBallDir = hitInfo.point - fireBall.transform.position;
                    fireBall.transform.forward = fireBallDir.normalized;
                }
                else
                {
                    fireBall.transform.forward = Camera.main.transform.forward;
                }
                Destroy(fireBall, 2);
                #endregion
            }
            //칼 공격 스킬----- 박자 두배로 하기
            if (playerAnimator.animator.GetInteger("weaponState") == 2)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    //칼질 이펙트
                    playerAnimator.OnSpeSwordAttack();
                }
            }

        }
        #endregion
        #region 마우스 오른쪽 - 특수 스킬
        //오른쪽 마우스를 클릭하면
        if (Input.GetButtonDown("Fire2"))
        {
            if (playerAnimator.animator.GetInteger("weaponState") == 0 && currSwordMP == maxMP)
            {
                playerAnimator.OnSpeSwordState();
            }

            if (playerAnimator.animator.GetInteger("weaponState") == 1 && currSkullMP == maxMP)
            {
                StunSkill();
                currSkullMP = 0;
            }
        }
        #endregion
        #region 1번 2번 - 무기변경
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //칼 애니메이션
            playerAnimator.OnSwordState();
        }

        //해골 상태 변경
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //해골 애니메이션
            playerAnimator.OnFireState();
        }
        #endregion
        #region E버튼 - 그로기 즉살 스킬
        //즉살 스킬
        if (Input.GetKeyDown(KeyCode.E))
        {
            OneShot();
        }
        #endregion


    }

    void StunSkill()
    {
        print("StunSkill 호출");
        LayerMask enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5, enemyLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyFsmJiwon enemy = hitCollider.GetComponent<EnemyFsmJiwon>();

            if (enemy != null)
            {
                Vector3 viewportPoint = Camera.main.WorldToViewportPoint(enemy.transform.position);
                if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
                {
                    print("스턴발생");
                    enemy.OnStunChanged();
                    GameObject stunEffect = Instantiate(stunEffectFactory);
                    stunEffect.transform.position = enemy.transform.position;

                }

            }
        }
    }
    //즉살스킬
    void OneShot()
    {
        print("oneShot 호출");
        LayerMask layerMask = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, oneShotDistance, layerMask);
        nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider hitCollider in hitColliders)
        {
            EnemyFsmJiwon enemy = hitCollider.GetComponent<EnemyFsmJiwon>();
            if (enemy)
            {
                Vector3 viewportPoint = Camera.main.WorldToViewportPoint(enemy.transform.position + Vector3.up);
                if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
                {
                    if (enemy.mState == EnemyState.Groggy)
                    {
                        float distance = Vector3.Distance(transform.position, enemy.transform.position);
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestEnemy = enemy;
                            playerAnimator.GroggyAttack();
                            print("원샷 함수 실행");
                        }

                    }
                }
            }
        }
        if (nearestEnemy)
        {
            onOneShot = true;
            print("원샷 함수 실행2");

            nearestEnemy.Die();
        }
    }
    public void SlashAni(int attackNum)
    {
        GameObject effectex = Instantiate(slashEffectList[attackNum - 1]);
        effectex.transform.position = slashPos.transform.position;
        effectex.transform.forward = slashPos.transform.forward;
        Destroy(effectex, 0.3f);

    }
    public void UpdateHP(float value)
    {
        // 현재 HP를 value 더하자.
        currHP += value;

        // 현재 HP가 0이면
        if (currHP <= 0)
        {
            // 파괴하자
            Destroy(gameObject);
        }
    }
    public void UpdateMP(float value)
    {
        if (playerAnimator.animator.GetInteger("weaponState") == 0)
        {
            currSwordMP += value;
        }
        else if (playerAnimator.animator.GetInteger("weaponState") == 1)
        {
            currSkullMP += value;
        }
        else
        {
            value = 0;
        }

        if (currSwordMP >= maxMP)
        {
            currSwordMP = maxMP;
        }
        if (currSkullMP >= maxMP)
        {
            currSkullMP = maxMP;
        }
    }

   
}
