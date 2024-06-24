using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //카메라 회전 속도 값
    public float rotSpeed = 200f;

    //플레이어 스피드
    public float moveSpeed = 7f;

    //플레이어 이동 방향
    public Vector3 dir;

    //점프
    //중력 변수
    public float gravity = -10f;
    //수직 속력 변수
    public float yVelocity = 0;
    //점프력 변수
    public float jumpPower = 10f;
    //점프상태 변수
    public bool isJumping = false;

    int nowJumpCount = 0;
    int maxJumpCount = 2;

    CharacterController cc;

    //플레이어 마우스 회전
    float mx = 0;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerRotate();
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
        }


        dir.y = yVelocity;


        cc.Move(dir * moveSpeed * Time.deltaTime);
        //transform.position += dir * moveSpeed * Time.deltaTime;
    }



}
