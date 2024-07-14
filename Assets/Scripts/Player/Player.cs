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
    public float maxMP = 1000;

    //즉살 공격
    bool onOneShot = false;
    EnemyFsmJiwon nearestEnemy = null;
    float speTime = 0;

    //스컬 특수공격
    public LayerMask enemyLayer;
    public float stunRadius = 5.0f;
    public GameObject stunEffectFactory;
    public GameObject stunEffectPos;

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

    public void Fire()
    {
        //플레이어가 검 상태일 때
        if (playerAnimator.animator.GetInteger("weaponState") == 0)
        {
            //검 애니메이션을 실행
            playerAnimator.OnSwordAttack();

        }
        if (playerAnimator.animator.GetInteger("weaponState") == 1)
        {
            #region 조준선에 정면에 레이를 쏜다
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hitInfo = new RaycastHit();

            //스컬 공격을 생성
            GameObject fireBall = Instantiate(fireBallFactory);
            //스컬 공격 위치 설정
            fireBall.transform.position = fireBallPos.transform.position;
            //앞에 물건이 있을 때
            if (Physics.Raycast(ray, out hitInfo))
            {

                //파이어볼을 조준선으로 발사
                fireBallDir = hitInfo.point - fireBall.transform.position;
                fireBall.transform.forward = fireBallDir.normalized;

            }
            //조준선에 물건이 없을 때
            else
            {
                //그냥 파이어 볼을 앞으로
                fireBall.transform.forward = Camera.main.transform.forward;

            }
            //파이어볼을 2초 뒤 파괴한다.
            Destroy(fireBall, 2);
            #endregion

        }
    }

    public void PlayerFire()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OneShot();

        }

        if (playerAnimator.animator.GetInteger("weaponState") == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //칼질 애니메이션
                playerAnimator.OnSwordAttack();
                //칼질 이펙트
                //SlashAni();
                onOneShot = false;

            }
            if (Input.GetButtonDown("Fire2") && currSwordMP == maxMP)
            {
                playerAnimator.OnSpeSwordState();
                //스워드 스페셜 스킬 임시 엠피 0 만들기
                Invoke("SwordMP0", 5);
            }
        }

        if (playerAnimator.animator.GetInteger("weaponState") == 1)
        {
            if (Input.GetButtonDown("Fire1"))
            {

                #region �÷��̾� ���Ÿ�
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

            if (Input.GetButtonDown("Fire2") && currSkullMP == maxMP)
            {
                StunSkill();
                currSkullMP = 0;
            }
        }

        //칼질 특수 공격
        if (playerAnimator.animator.GetInteger("weaponState") == 2)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //칼질 이펙트
                playerAnimator.OnSpeSwordAttack();

            }
        }
        //칼상태로 변경
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
    }

    // ReSharper disable Unity.PerformanceAnalysis
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
                    stunEffect.transform.position = stunEffectPos.transform.position;

                }

            }
        }
    }
    //즉살스킬
    void OneShot()
    {
        print("oneShot 호출");
        LayerMask layerMask = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f, layerMask);
        nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider hitCollider in hitColliders)
        {
            EnemyFsmJiwon enemy = hitCollider.GetComponent<EnemyFsmJiwon>();
            if (enemy)
            {
                Vector3 viewportPoint = Camera.main.WorldToViewportPoint(enemy.transform.position);
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

    void SwordMP0()
    {
        currSwordMP = 0;
    }
}
