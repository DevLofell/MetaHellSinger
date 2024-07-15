using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum BossState
{
    InitDelay,
    Flying,
    FlyDelay,
    FlyRotate,
    FlyAttack,
    AttackDelay,
    Die
}

public class BossPatternManager : MonoSingleton<BossPatternManager>
{
    // 패턴 관리
    // 위치 관리
    public bool isBossStart = false;

    public GameObject BossObject;

    public List<BossPattern> patternList;
    public int nowIndex = 0;

    public BossState state;

    
    
    // 날기 관련
    public Transform flyingPos;

    public Coroutine nowCoroutine;
    void Start()
    {
        patternList = patternList ?? new List<BossPattern>();
        ChangeState(BossState.InitDelay);
    }

    void ChangeState(BossState newState)
    {
        Debug.Log("현재 상태 " + state + " >>> " + "바꿀 상태 " + newState);
        state = newState;

        switch (state)
        {
            case BossState.InitDelay:
                nowCoroutine = StartCoroutine(InitDelay());
                break;
            case BossState.Flying:
                // 타겟 지정
                flyingPos = patternList[nowIndex].BossRig;
                break;
            case BossState.FlyDelay:
                nowCoroutine = StartCoroutine(FlyDelay());  
                break;
            case BossState.FlyRotate:
                flyingPos = patternList[nowIndex].BossRig;
                break;
            case BossState.FlyAttack:
                patternList[nowIndex].gameObject.SetActive(true);
                break;
            case BossState.AttackDelay:
                nowCoroutine = StartCoroutine(AttackDelay());
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (patternList.Count == 0) return;

        switch (state)
        {
            case BossState.Flying:
                Flying();
                break;
            case BossState.FlyRotate:
                FlyRotate();
                break;
            case BossState.FlyAttack:
                FlyAttack();
                break;
            default:
                break;
        }
    }

    IEnumerator InitDelay()
    {
        yield return new WaitUntil(() => (isBossStart));
        ChangeState(BossState.FlyAttack);
    }

    IEnumerator FlyDelay()
    {
        yield return new WaitForSeconds(3f);
        ChangeState(BossState.FlyAttack);
    }
    void FlyAttack()
    {
        if (patternList[nowIndex].isPatternOver)
        {
            patternList[nowIndex].gameObject.SetActive(false);

            // 패턴 인덱스 증가
            nowIndex++;

            // 패턴 인덱스가 리스트 길이를 초과하면 0으로 초기화
            if (nowIndex >= patternList.Count)
            {
                nowIndex = 0;
            }

            ChangeState(BossState.AttackDelay);
        }
    }

    float moveSpeed = 10f;

    void Flying()
    {
        Vector3 dir = flyingPos.transform.position - BossObject.transform.position;
        float distance = dir.magnitude; // 보스와 목표 위치 사이의 거리 계산

        if (distance < 0.1f) // 거리 비교 (0.1f는 임계값으로, 필요에 따라 조정 가능)
        {
            BossObject.transform.position = flyingPos.transform.position; // 보스를 정확히 목표 위치에 맞추기
            ChangeState(BossState.FlyRotate); // 상태를 FlyAttack으로 변경하여 공격 시작
        }
        else
        {
            //BossObject.transform.Translate(dir.normalized * moveSpeed * Time.deltaTime); // 목표 위치로 이동
            BossObject.transform.position += dir.normalized * moveSpeed * Time.deltaTime;
        }
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(3f);
        ChangeState(BossState.Flying);
        
    }

    float nowRotateTime = 0;
    void FlyRotate()
    {
        //BossObject.transform.forward = Vector3.Lerp(BossObject.transform.forward, flyingPos.forward, 10 * Time.deltaTime);

        BossObject.transform.rotation = Quaternion.Lerp(BossObject.transform.rotation, flyingPos.rotation, 10 * Time.deltaTime);

        if(Vector3.Angle(BossObject.transform.forward, flyingPos.transform.forward) < 5)
        {
            ChangeState(BossState.FlyDelay);
        }

    }

}
