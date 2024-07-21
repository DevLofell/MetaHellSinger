using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Experimental.GraphView.GraphView;
using Random = UnityEngine.Random;


public enum BossMainState
{
    Sleep,
    Scream,
    Idle,
    ChangeFlyState,
    StartMoveRotate,
    Move,
    EndMoveRotate,
    AttackDelay,
    PatternAttack,
    CloseMove,
    CloseAttack,
    Hurt,
    Die
}
public enum BossFlyState
{
    Walk = 0,
    Run = 1,
    Fly = 2
}



public class BossFSM : MonoBehaviour
{
    
    public List<BossPattern> bossPatterns;
    public int selectPattern = 0;

    public Transform bossMovePos;


    public Transform player;
    public Transform boss;

    public bool isStartBoss = false;

    public bool isDie = false;

    bool isAnimationFinished = false;

    public Animator animator;

    public Coroutine nowCoroutine;

    public GameObject bossSliderObect;
    public Image bossHPSlider;
    public int maxHp = 3000;
    public int hp;

    //������ UI
    public GameObject damageUI;

    //UI�� ��Ʈ������
    public RectTransform rtDamageUI;

    private Player _playerScript;


    //�̵��ϰ� �����ϰ� ���� ���� ���� ����
    //�ʱ� ������ ���� trueó��

    public float moveRange
    {
        get
        {
            switch(flyState)
            {
                case BossFlyState.Walk:
                    return walkRange;
                case BossFlyState.Run:  
                    return runRange;
                case BossFlyState.Fly:
                    return flyRange;
                default:
                    return walkRange;
            }
        }
    }
    public float walkRange = 4f;
    public float runRange = 4f;
    public float flyRange = 4f;

    public float moveSpeed
    {
        get
        {
            switch (flyState)
            {
                case BossFlyState.Walk:
                    return walkSpeed;
                case BossFlyState.Run:
                    return runSpeed;
                case BossFlyState.Fly:
                    return flySpeed;
                default:
                    return walkSpeed;
            }
        }
    }
    public float walkSpeed = 5f;
    public float runSpeed = 5f;
    public float flySpeed = 5f;

    public float attackRange
    {
        get
        {
            switch (flyState)
            {
                case BossFlyState.Walk:
                    return walkAttackRange;
                case BossFlyState.Run:
                    return runAttackRange;
                case BossFlyState.Fly:
                    return flyAttackRange;
                default:
                    return walkAttackRange;
            }
        }
    }
    public float walkAttackRange = 3f;
    public float runAttackRange = 5f;
    public float flyAttackRange = 7f;

    float currIdleTime = 0;
    public float IdleTotalTime = 3f;

    float currDelayTime = 0f;
    public float attackDelayTime = 1f;

    float currAttackTime = 0f;
    public float attackTime = 3f;

    public int attackValue = 1;

    public Vector3 closeAttackPos;

    BossMainState mainstate = BossMainState.Idle;
    BossFlyState flyState = BossFlyState.Fly;


    IEnumerator Start()
    {
       
        hp = maxHp;
        player = player ?? GameObject.FindWithTag("Player").transform;
        _playerScript = player.GetComponent<Player>();

        bossPatterns = bossPatterns ?? new List<BossPattern>();
        animator = boss.GetComponent<Animator>();
        yield return new WaitUntil(()=>(isStartBoss));
        bossSliderObect.SetActive(true);




    }
    public void ChangeMainState(BossMainState state)
    {
        Debug.Log("���� ���� ���� ����: " + mainstate + ">>>" + state);
        this.mainstate = state;
        switch(state)
        {
            case BossMainState.StartMoveRotate:
                bossMovePos = bossPatterns[selectPattern].BossRig;
                break;
            case BossMainState.ChangeFlyState:
                int randValue;
                do
                {
                     randValue = Random.Range(0, 3);
                }
                while (randValue == (int)flyState);
                animator.SetInteger("FlyState", randValue);
                nowCoroutine = StartCoroutine(ChangeFlyState((BossFlyState)randValue));
                    break;
            case BossMainState.Move:
                animator.SetTrigger("IdleToMove");
                break;
            case BossMainState.EndMoveRotate:
                animator.SetTrigger("MoveToIdle");
                break;
            case BossMainState.AttackDelay:
                //attackValue = Random.Range(1, 1);
                attackValue = 1;
                animator.SetTrigger("IdleToAttackDelay");
                break;
            case BossMainState.PatternAttack:
                animator.SetTrigger("StartAttack");
                bossPatterns[selectPattern].gameObject.SetActive(true);
                break;
            case BossMainState.CloseAttack:
                closeAttackPos = player.transform.position;
                animator.SetTrigger("StartAttack");
                break;
            case BossMainState.Die:
                animator.SetTrigger("Die");
                break;




        }

    }

    public IEnumerator ChangeFlyState(BossFlyState state)
    {
        Debug.Log("���� ���� ���� ����: " + flyState + ">>>" + state);
        
        
        if (flyState == BossFlyState.Fly ||
            state == BossFlyState.Fly)
        {
            isAnimationFinished = false;
        }
        else
        {
            isAnimationFinished = true;
        }
        yield return new WaitUntil(() => (isAnimationFinished));
        
        isAnimationFinished = false;
        ChangeMainState(BossMainState.StartMoveRotate);
        this.flyState = state;
        
        
        
    }

    private void Update()
    {
        //���� ������ ó�� ������
        if (!isStartBoss) return;
        
        UpdateMainState();
    }

    private void UpdateMainState()
    {
        switch (mainstate)
        {
            case BossMainState.Idle:
                Idle();
                break;
            case BossMainState.ChangeFlyState:
                UpdateFlyState();
                break;
            case BossMainState.Sleep:
                Sleep();
                break;
            case BossMainState.Scream:
                Scream();
                break;
            case BossMainState.StartMoveRotate:
                StartMoveRotate();
                break;
            case BossMainState.Move:
                Move();
                break;
            case BossMainState.EndMoveRotate:
                EndMoveRotate();
                break;
            case BossMainState.CloseMove:
                CloseMove();
                break; 
            case BossMainState.AttackDelay:
                AttackDelay();
                break;
            case BossMainState.PatternAttack:
                PatternAttack();
                break;
            case BossMainState.CloseAttack:
                CloseAttack();
                break;
            case BossMainState.Hurt:
                Hurt();
                break;
            case BossMainState.Die:
                Die();
                break;


        }
    }

    

    private void Die()
    {
        
    }

    private void Hurt()
    {
        throw new NotImplementedException();
    }

    private void StartMoveRotate()
    {

        // Ÿ�� ��ġ�� ���ϴ� ���� ���� ���
        Vector3 directionToTarget = bossMovePos.position - boss.transform.position;
        directionToTarget.y = 0; // y�� ȸ���� ����

        // ��ǥ ȸ���� ���
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        // ���� ȸ���� ��ǥ ȸ������ �ε巴�� ����
        boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, targetRotation, 10 * Time.deltaTime);

        // ���� ȸ���� ��ǥ ȸ�� ���� ���� ��
        if (Quaternion.Angle(boss.transform.rotation, targetRotation) < 5)
        {
            ChangeMainState(BossMainState.Move);
        }
    }
    /// <summary>
    /// �÷��̾ �ƴ� �������� �̵��ϴ� �Լ�
    /// </summary>
    private void Move()
    {
        
        Vector3 dir = bossPatterns[selectPattern].transform.position - boss.transform.position;
        this.transform.position += dir.normalized * moveSpeed * Time.deltaTime;
        if(Vector3.Distance(boss.transform.position, bossPatterns[selectPattern].transform.position) <  0.1f)
        {
            boss.transform.position = bossPatterns[selectPattern].transform.position;
            ChangeMainState(BossMainState.EndMoveRotate);
        }
    }
    private void EndMoveRotate()
    {
        // �÷��̾ ���ϴ� ���� ���� ���
        Vector3 directionToPlayer = player.position - boss.transform.position;
        directionToPlayer.y = 0; // y�� ȸ���� ����

        // ��ǥ ȸ���� ���
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // ���� ȸ���� ��ǥ ȸ������ �ε巴�� ����
        boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, targetRotation, 10 * Time.deltaTime);

        // ���� ȸ���� ��ǥ ȸ�� ���� ���� ��
        if (Quaternion.Angle(boss.transform.rotation, targetRotation) < 5)
        {
            ChangeMainState(BossMainState.AttackDelay); // ���� ��� ���·� ��ȯ
        }
    }

 
    private void Scream()
    {
        throw new NotImplementedException();
    }

    private void Sleep()
    {
        throw new NotImplementedException();
    }

    
    private void Idle()
    {
        //���ð� �ֱ�
        currIdleTime += Time.deltaTime;
        if(currIdleTime > IdleTotalTime)
        {

           ChangeMainState(BossMainState.ChangeFlyState);
            currIdleTime = 0;
        }
        
    }


    private void AttackDelay()
    {
        currDelayTime += Time.deltaTime;
        if(currDelayTime > attackDelayTime)
        {
            
            currDelayTime = 0;
            animator.SetInteger("AttackState", attackValue);
            switch(attackValue)
            {
                case 0:
                    ChangeMainState(BossMainState.CloseAttack);
                    break;

                case 1:
                    ChangeMainState(BossMainState.PatternAttack);
                    break;
                default:
                    ChangeMainState(BossMainState.CloseAttack);
                    break;
            }
            return;
        }
    }
    private void CloseMove()
    {
        animator.SetTrigger("IdleToMove");
        Vector3 dir = player.transform.position - boss.transform.position;
        float distance = dir.magnitude; // ������ ��ǥ ��ġ ������ �Ÿ� ���

        if (distance < attackRange) 
        {
            ChangeMainState(BossMainState.CloseAttack); // ���¸� FlyAttack���� �����Ͽ� ���� ����
        }
        else
        {
           
            boss.transform.position += dir.normalized * moveSpeed * Time.deltaTime;
        }
    }

    private void CloseAttack()
    {
        //�ִϸ��̼� Ʈ����
        //�ݶ��̴� 
        switch(flyState)
        {
            case BossFlyState.Walk:

                ChangeMainState(BossMainState.Idle);
                //�ٷ� �տ��� �����ϱ⿡ �ִϸ��̼Ǹ� ����
                break;
            case BossFlyState.Run:
                ChangeMainState(BossMainState.Idle);
                //�ٷ� �տ��� �����ϱ⿡ �ִϸ��̼Ǹ� ����
                break;
            case BossFlyState.Fly:
                //���ư��鼭 �Ʒ��� ���� �������̱⿡ �̵�ó��
                Vector3 dir = closeAttackPos - boss.transform.position;
                float distance = dir.magnitude; // ������ ��ǥ ��ġ ������ �Ÿ� ���
                currAttackTime += Time.deltaTime;
                if (currAttackTime > attackTime)
                {
                   
                    ChangeMainState(BossMainState.Idle); // ���¸� FlyAttack���� �����Ͽ� ���� ����
                }
                else
                {

                    boss.transform.position += dir.normalized * moveSpeed * Time.deltaTime;
                }

                break;

        }
    }

    private void PatternAttack()
    {
        if (bossPatterns[selectPattern].isPatternOver)
        {
            bossPatterns[selectPattern].gameObject.SetActive(false);

            // ���� �ε��� ����
            selectPattern++;

            // ���� �ε����� ����Ʈ ���̸� �ʰ��ϸ� 0���� �ʱ�ȭ
            if (selectPattern >= bossPatterns.Count)
            {
                selectPattern = 0;
            }

            ChangeMainState(BossMainState.Idle); 
        }
    }

    private void UpdateFlyState()
    {
        
    }
    public void HitBoss(int hitPower)
    {
        if (mainstate == BossMainState.Die) return;
        Debug.Log("���� ���� hp :" + hp);
        Debug.Log("���� ����� : " + hitPower);
        hp -= hitPower;
        bossHPSlider.fillAmount = ((float)hp / (float)maxHp);

        if (hp <= 0)
        {
            hp = 0;
            ChangeMainState(BossMainState.Die);
            StartCoroutine(GameClear());
        }
        

    }

    public IEnumerator GameClear()
    {
        yield return new WaitForSeconds(4f);
        SceneSystem.instance.GameClear();
    }



    public void Landing() => isAnimationFinished = true;


    public void Flying() => isAnimationFinished = true;

}






