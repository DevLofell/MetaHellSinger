using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using static EnemyGroggyState;


public class EnemyFsmJiwon : MonoBehaviour
{
    public float findDistance = 8f;
    public float moveSpeed = 5f;
    public float attackDistance = 2f;
    public float moveDistance = 20f;
    public int attackPower = 3;
    public int maxHp = 100;
    public int hp;
    public float waitDamagedSec = 1.0f;
    public float attackDelay = 2f;
    public EnemyState mState;

    //그로기 상태
    public EnemyGroggyState groggyState = NOT_GROGGY;
    public int groggyHp;
    public float groggyTimer = 3.0f;

    //그로기UI
    public GameObject groggyUI;
    public GameObject canvas;

    //스턴스킬
    public float stunTimer;
    public float stunTime = 5.0f;

    //스턴 연계 죽음
    public float stunDeadRadius = 5.0f;

    //데미지 UI
    public GameObject damageUI;

    //데미지가 뜨는 위치
    public Transform damagePos;

    //데미지 UI의 렉트포지션
    public RectTransform rtDamageUI;


    private LayerMask _enemyLayer;
    private NavMeshAgent _navMeshAgent;
    private Transform _player;
    private PlayerMove _playerMove;
    private float _currentTime;
    private Vector3 _originPos;
    private Quaternion _originRot;
    private Animator _anim;
    private CharacterController _characterController;
    private static readonly int IdleToMove = Animator.StringToHash("IdleToMove");
    private static readonly int MoveToIdle = Animator.StringToHash("MoveToIdle");
    private static readonly int MoveToAttackDelay = Animator.StringToHash("MoveToAttackDelay");
    private static readonly int StartAttack = Animator.StringToHash("StartAttack");
    private static readonly int AttackToMove = Animator.StringToHash("AttackToMove");
    private static readonly int Damaged1 = Animator.StringToHash("Damaged");
    private static readonly int Die1 = Animator.StringToHash("Die");
    private static readonly int OnStun = Animator.StringToHash("OnStun");
    private static readonly int OnGroggy = Animator.StringToHash("OnGroggy");

    void Start()
    {
        groggyHp = maxHp / 10;
        hp = maxHp;
        mState = EnemyState.Idle;
        stunTimer = stunTime;
        // TODO: 플레이어로 변경
        _player = GameObject.Find("Player").transform;
        _playerMove = _player.GetComponent<PlayerMove>();
        _characterController = GetComponent<CharacterController>();
        _originPos = transform.position;
        _originRot = transform.rotation;
        _anim = GetComponentInChildren<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        canvas = GameObject.Find("Canvas");
        _enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    // Update is called once per frame
    void Update()
    {

        switch (mState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Stun:
                Stun();
                break;
            case EnemyState.Damaged:
                // Damaged();
                break;
            case EnemyState.Die:
                // Die();
                break;
        }
    }
    
    private void Damaged()
    {
        StartCoroutine(DamageProcess());
    }

    private IEnumerator DamageProcess()
    {
        // 피격 모션 만큼 기다리기
        yield return new WaitForSeconds(waitDamagedSec);

        // 현재 상태를 이동 상태로 전환한다.
        mState = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }

    private void Return()
    {
        if (Vector3.Distance(transform.position, _originPos) > 0.1f)
        {
            _navMeshAgent.SetDestination(_originPos);
            _navMeshAgent.stoppingDistance = 0;
        }
        else
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.ResetPath();

            transform.position = _originPos;
            transform.rotation = _originRot;

            hp = maxHp;
            mState = EnemyState.Idle;
            print("상태 전환: Return -> Idle");

            _anim.SetTrigger(MoveToIdle);
        }
    }

    public void Die()
    {
        StopAllCoroutines();

        StartCoroutine(DieProcess());
    }

    // TODO: 그로기 상태 시 죽여야 함
    private IEnumerator DieProcess()
    {
        print("상태 전환 : 죽음");
        _characterController.enabled = false;
        groggyState = WAS_GROGGY;
        if (mState == EnemyState.Stun)
        {
            OnDieWhenStun();
        }
        mState = EnemyState.Die;
        yield return new WaitForSeconds(5f);
        print("!소멸");
        Destroy(gameObject);
    }

    private void Attack()
    {
        // 플레이어가 공격 범위 내
        if (Vector3.Distance(transform.position, _player.position) < attackDistance)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > attackDelay)
            {
                print("공격");
                _currentTime = 0;
                _anim.SetTrigger(StartAttack);
            }
        }
        else
        {
            mState = EnemyState.Move;
            print("상태 전환: Attack -> Move");
            _currentTime = 0;

            _anim.SetTrigger(AttackToMove);
        }
    }

    private void Move()
    {
        // 현재 위치가 초기 위치에서 이동 가능 범위를 넘어선다면
        var currentToOriginDistance = Vector3.Distance(transform.position, _originPos);
        if (currentToOriginDistance > moveDistance)
        {
            mState = EnemyState.Return;
            print("상태 전환: Move -> Return");
        }
        else
        {
            var currentToPlayerDistance = Vector3.Distance(transform.position, _player.position);
            if (currentToPlayerDistance > attackDistance)
            {
                _navMeshAgent.isStopped = true;
                _navMeshAgent.ResetPath();
                _navMeshAgent.stoppingDistance = attackDistance;
                _navMeshAgent.destination = _player.position;
            }
            else
            {
                mState = EnemyState.Attack;
                print("상태 전환: Move -> Attack");

                _currentTime = attackDelay;

                _anim.SetTrigger(MoveToAttackDelay);
            }
        }
    }

    private void Idle()
    {
        if (Vector3.Distance(transform.position, _player.position) < findDistance)
        {
            mState = EnemyState.Move;
            print("상태 전환 : Idle -> Move");

            _anim.SetTrigger(IdleToMove);
        }
    }

    public void HitEnemy(int hitPower)
    {
        if (mState is EnemyState.Damaged or EnemyState.Die or EnemyState.Return)
        {
            return;
        }

        hp -= hitPower;
        print("적 체력 hp : " + hp);
        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath();

        if (hp > 0)
        {
            mState = EnemyState.Damaged;
            print("상태 전환: Any state -> Damaged");
            if (hp <= groggyHp && groggyState == NOT_GROGGY)
            {
                print("상태 전환: 그로기");
                mState = EnemyState.Groggy;
                _anim.SetTrigger(OnGroggy);
                print("상태 전환: Any State -> Groggy");
                Groggy();
            }
            else
            {
                _anim.SetTrigger(Damaged1);
                Damaged();
            }
        }
        else
        {
            mState = EnemyState.Die;
            print("상태 전환: Any state -> Die");
            _anim.SetTrigger(Die1);
            Die();
        }
    }

    public void AttackAction()
    {
        // TODO: 플레이어 피격 함수로 변경
        _playerMove.DamageAction(attackPower);
    }

    private void Groggy()
    {
        if (groggyTimer < 0.0f)
        {
            groggyState = WAS_GROGGY;
            mState = EnemyState.Idle;
            return;
        }
        groggyTimer -= Time.deltaTime;
        Instantiate(groggyUI, canvas.transform);

        // 적의 중앙에 E라는 글씨가 뜬다
    }

    private void OnDieWhenStun()
    {
        // 주변의 적들을 탐색하고
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, stunDeadRadius, _enemyLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyFsmJiwon enemy = hitCollider.GetComponent<EnemyFsmJiwon>();
            if (!enemy) continue;
            // 주변 적들이 스턴 상태인지 확인하고
            if (enemy.mState == EnemyState.Stun)
            {
                enemy.Die();
            }
        }
    }

    public void OnStunChanged()
    {
        mState = EnemyState.Stun;
        stunTimer = stunTime;
    }

    private void Stun()
    {
        stunTimer -= Time.deltaTime;
        _anim.SetTrigger(OnStun);
        if (stunTimer < 0.0f)
        {
            mState = EnemyState.Idle;
        }
    }
}