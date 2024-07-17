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
    // ���� ����
    // ��ġ ����
    public bool isBossStart = false;

    public GameObject BossObject;

    public List<BossPattern> patternList;
    public int nowIndex = 0;

    public BossState state;

    
    
    // ���� ����
    public Transform flyingPos;

    public Coroutine nowCoroutine;
    void Start()
    {
        patternList = patternList ?? new List<BossPattern>();
        ChangeState(BossState.InitDelay);
    }

    void ChangeState(BossState newState)
    {
        Debug.Log("���� ���� " + state + " >>> " + "�ٲ� ���� " + newState);
        state = newState;

        switch (state)
        {
            case BossState.InitDelay:
                nowCoroutine = StartCoroutine(InitDelay());
                break;
            case BossState.Flying:
                // Ÿ�� ����
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

            // ���� �ε��� ����
            nowIndex++;

            // ���� �ε����� ����Ʈ ���̸� �ʰ��ϸ� 0���� �ʱ�ȭ
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
        float distance = dir.magnitude; // ������ ��ǥ ��ġ ������ �Ÿ� ���

        if (distance < 0.1f) // �Ÿ� �� (0.1f�� �Ӱ谪����, �ʿ信 ���� ���� ����)
        {
            BossObject.transform.position = flyingPos.transform.position; // ������ ��Ȯ�� ��ǥ ��ġ�� ���߱�
            ChangeState(BossState.FlyRotate); // ���¸� FlyAttack���� �����Ͽ� ���� ����
        }
        else
        {
            //BossObject.transform.Translate(dir.normalized * moveSpeed * Time.deltaTime); // ��ǥ ��ġ�� �̵�
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
