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
    public float gravity = -10f;
    //수직 속력 변수
    public float yVelocity = 0;
    //점프력 변수
    public float jumpPower = 5f;
    //점프상태 변수
    public bool isJumping = false;

    int nowJumpCount = 0;
    int maxJumpCount = 2;

    #endregion

    //원거리공격
    //피격 이펙트 오브젝트
    public GameObject bulletEffect;

    //피격 파티클 시스템
    ParticleSystem ps;

    //애니메이터
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

        ps = bulletEffect.GetComponent<ParticleSystem>();
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

        //캐릭터 수직 속도에 중력 값을 적용
        yVelocity += gravity * Time.deltaTime;




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
            //애니메이션
        }


        dir.y = yVelocity;


        cc.Move(dir * moveSpeed * Time.deltaTime);
        //transform.position += dir * moveSpeed * Time.deltaTime;


        //애니메이션
    }

    void PlayerFire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //레이를 생성한 후 발사될 위치와 진행 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            //레이가 부딪힌 대상의 정보를 저장할 변수를 생성한다.
            RaycastHit hitInfo = new RaycastHit();

            //레이를 발사한 후 만일 부딪힌 물체가 있으면 피격 이펙트를 표시한다.
            if(Physics.Raycast(ray, out hitInfo))
            {
                //피격 이펙트의 위치를 레이가 부딪힌 지점으로 이동
                bulletEffect.transform.position = hitInfo.point;

                //피격 이펙트의 forward 방향을 레이가 부딪힌 지점의 법선 벡터와 일치시킨다.
                bulletEffect.transform.forward = hitInfo.normal;

                //피격 이펙트를 플레이
                ps.Play();
            }
        }
    }

}
