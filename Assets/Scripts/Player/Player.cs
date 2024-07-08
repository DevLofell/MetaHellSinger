using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    #region 플레이어 기본 움직임
    //카메라 회전 속도 값
    public float rotSpeed = 200f;

    //플레이어 마우스 회전
    float mx = 0;

    //플레이어 스피드
    public float moveSpeed = 7f;

    //플레이어 이동 방향
    public Vector3 dir;

    CharacterController cc;
    #endregion

    #region 점프 변수
    //점프
    //중력 변수
    public float gravity = -4f;
    //수직 속력 변수
    public float yVelocity = 0;
    //점프력 변수
    public float jumpPower = 1.7f;
    //점프상태 변수
    public bool isJumping = false;
    //현재 점프 수
    int nowJumpCount = 0;
    //최대 점프 수
    int maxJumpCount = 2;

    #endregion

    //원거리공격
    //파이어볼 프리팹
    public GameObject fireBallFactory;
    //파이어볼 생성 위치
    public GameObject fireBallPos;
    //파이어볼 스피드
    public float fireBallSpeed = 5.0f;
    //파이어볼 방향
    public Vector3 fireBallDir;



    //피격 파티클 시스템
    ParticleSystem ps;

    //애니메이터
    public Animator anim;
    private PlayerAnimator playerAnimator;

    
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();

        //ps = bulletEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerRotate();
        PlayerFire();
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

        //점프 기능
        //스페이스바 누르면 점프

        if (!cc.isGrounded)
        {
            //캐릭터 수직 속도에 중력 값을 적용
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
        //transform.position += dir * moveSpeed * Time.deltaTime;


        //애니메이션
        //무빙 애니메이션 실행
        playerAnimator.OnMovement(h, v);

    }

    public void Fire()
    {
        //무기 상태가 칼일 때
        if (playerAnimator.animator.GetInteger("weaponState") == 0)
        {
            ////칼질 애니메이션 실행
            playerAnimator.OnSwordAttack();

        }
        if (playerAnimator.animator.GetInteger("weaponState") == 1)
        {
            #region 플레이어 원거리
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hitInfo = new RaycastHit();

            //파이어볼을 생성한다
            GameObject fireBall = Instantiate(fireBallFactory);
            //파이어볼의 위치를 생성위치에 맞춘다.
            fireBall.transform.position = fireBallPos.transform.position;
            //레이를 쐈는데...
            //무언가와 부딪혔으면 부딪힌 곳으로
            if (Physics.Raycast(ray, out hitInfo))
            {

                //파이어볼의 방향을 화면 중앙(레이)으로 한다.
                fireBallDir = hitInfo.point - fireBall.transform.position;
                //앞으로 이동한다.
                fireBall.transform.forward = fireBallDir.normalized;

            }
            //부딪힌곳이 없으면 카메라가 바라보는 방향으로
            else
            {
                //파이어볼의 앞을 레이쪽으로 바꾼다.
                fireBall.transform.forward = Camera.main.transform.forward;

            }
            //2초 뒤에 파이어볼을 파괴한다.
            Destroy(fireBall, 2);
            #endregion

        }
    }

    public void PlayerFire()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            

        }
        //1번을 누르면
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //애니메이션 상태를 스워드로 바꾸기
            playerAnimator.OnSwordState();
        }

        //2번을 누르면
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //애니메이션 상태를 원거리로 바꾸기
            playerAnimator.OnFireState();
        }
    }

}
