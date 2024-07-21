using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static EnemyGroggyState;


public class EnemyFsmJiwon : MonoBehaviour
{
    //���ǰ����� �ޱ� �߰� (����)
    float attackMPUP = 0;

    public float findDistance;
    public float attackDistance;
    public float moveDistance;
    public int attackPower;
    public int maxHp;
    public int hp;
    public float waitDamagedSec = 0.2f;
    public float attackDelay = 2f;
    public EnemyState mState;
    [FormerlySerializedAs("AudioClips")] public AudioClip[] audioClips;

    //�׷α� ����
    public EnemyGroggyState groggyState = NOT_GROGGY;
    public int groggyHp;
    public float groggyTimer = 3.0f;

    //�׷α�UI
    public GameObject groggyUI;
    public GameObject canvas;

    //���Ͻ�ų
    public float stunTimer;
    public float stunTime = 5.0f;

    //���� ���� ����
    public float stunDeadRadius = 15.0f;

    //������ UI
    public GameObject damageUI;

    //�������� �ߴ� ��ġ
    public Transform damagePos;
    public Transform groggyPos;

    //UI�� ��Ʈ������
    public RectTransform rtDamageUI;

    private float _currentSoundTime = 0f;
    private EnemyState prevEnemyState = EnemyState.Idle;
    private AudioSource _audioSource;
    private RectTransform _rectTransformGroggyUI;
    private GameObject _groggyUIObj;
    private LayerMask _enemyLayer;
    private NavMeshAgent _navMeshAgent;
    protected Transform _player;
    private Player _playerScript;
    private float _currentTime;
    private Vector3 _originPos;
    private Quaternion _originRot;
    private Animator _anim;
    private static readonly int IdleToMove = Animator.StringToHash("IdleToMove");
    private static readonly int MoveToIdle = Animator.StringToHash("MoveToIdle");
    private static readonly int MoveToAttackDelay = Animator.StringToHash("MoveToAttackDelay");
    private static readonly int StartAttack = Animator.StringToHash("StartAttack");
    private static readonly int AttackToMove = Animator.StringToHash("AttackToMove");
    private static readonly int Damaged1 = Animator.StringToHash("Damaged");
    private static readonly int Die1 = Animator.StringToHash("Die");
    private static readonly int OnStun = Animator.StringToHash("OnStun");
    private static readonly int OnGroggy = Animator.StringToHash("OnGroggy");

    public virtual void Start()
    {
        //MP�ޱ� (��)
        attackMPUP = Player.instance.maxMP / 10f;

        _audioSource = GetComponent<AudioSource>();
        groggyHp = maxHp / 10;
        hp = maxHp;
        mState = EnemyState.Idle;
        stunTimer = stunTime;
        _player = GameObject.FindWithTag("Player").transform;
        _playerScript = _player.GetComponent<Player>();
        _originPos = transform.position;
        _originRot = transform.rotation;
        _anim = GetComponentInChildren<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        canvas = GameObject.Find("Canvas");
        _enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        _audioSource.clip = audioClips[(int)mState];
        _audioSource.Play();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        _currentSoundTime += Time.deltaTime;
        
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
            case EnemyState.Groggy:
                Groggy();
                break;
            case EnemyState.Damaged:
                // Damaged();
                break;
            case EnemyState.Die:
                // Die();
                break;
        }
        
        // PlayByState();

        // prevEnemyState = mState;
    }

    private void Damaged(EnemyState prevState)
    {
        StartCoroutine(DamageProcess(prevState));
    }

    private IEnumerator DamageProcess(EnemyState prevState)
    {
        // �ǰ� ��� ��ŭ ��ٸ���
        yield return new WaitForSeconds(waitDamagedSec);

        if (prevState is EnemyState.Groggy or EnemyState.Stun)
        {
            mState = prevState;
        }
        else
        {
            mState = EnemyState.Idle;
        }

        print("���� ��ȯ: Damaged -> " + mState);
    }

    private void Return()
    {
       print("Return Distance : " + Vector3.Distance(transform.position, _originPos));
        if (Vector3.Distance(transform.position, _originPos) > 1f)
        {
            _navMeshAgent.SetDestination(_originPos);
            _navMeshAgent.stoppingDistance = 0;
            print("���ư��� ��");
        }
        else
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.ResetPath();

            transform.position = _originPos;
            transform.rotation = _originRot;

            hp = maxHp;
            mState = EnemyState.Idle;
            print("���� ��ȯ: Return -> Idle");

            _anim.SetTrigger(MoveToIdle);
        }
    }

    public void Die()
    {
        StopAllCoroutines();

        StartCoroutine(DieProcess());
    }

    private IEnumerator DieProcess()
    {
        mState = EnemyState.Die;
        _anim.SetBool(OnStun, false);
        _anim.SetBool(OnGroggy, false);
        _anim.SetTrigger(Die1);
        groggyState = WAS_GROGGY;
        print("���� ��� mState : " + mState);
        OnDieWhenStun();
        if (_groggyUIObj)
        {
            Destroy(_groggyUIObj);
        }

        yield return new WaitForSeconds(5f);
        print("!�Ҹ�");
        gameObject.SetActive(false);
    }

    protected virtual void Attack()
    {
        // �÷��̾ ���� ���� ��
        if (Vector3.Distance(transform.position, _player.position) < attackDistance)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > attackDelay)
            {
                print("����");
                _currentTime = 0;
                _anim.SetTrigger(StartAttack);
            }
        }
        else
        {
            mState = EnemyState.Move;
            print("���� ��ȯ: Attack -> Move");
            _currentTime = 0;

            _anim.SetTrigger(AttackToMove);
        }
    }

    private void Move()
    {
      // ���� ��ġ�� �ʱ� ��ġ���� �̵� ���� ������ �Ѿ�ٸ�
        var currentToOriginDistance = Vector3.Distance(transform.position, _originPos);
        if (currentToOriginDistance > moveDistance)
        {
            mState = EnemyState.Return;
            print("���� ��ȯ: Move -> Return");
        }
        else
        {
            print(gameObject.name + " Move Distance : " + currentToOriginDistance);
            var currentToPlayerDistance = Vector3.Distance(transform.position, _player.position);
            if (mState is EnemyState.Stun or EnemyState.Groggy) return;
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
                print("���� ��ȯ: Move -> Attack");

                _currentTime = 0;

                _anim.SetTrigger(MoveToAttackDelay);
            }
        }
    }

    private void Idle()
    {
        if (Vector3.Distance(transform.position, _player.position) < findDistance)
        {
            mState = EnemyState.Move;
            print("���� ��ȯ : Idle -> Move");
            _anim.SetTrigger(IdleToMove);
        }
    }

    public void HitEnemy(int hitPower)
    {
        if (mState is EnemyState.Damaged or EnemyState.Die)
        {
            return;
        }
        
        var prevState = mState;
        hp -= hitPower;
        print("�� ü�� hp : " + hp);
        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath();
        
        if (hp > 0)
        {
            mState = EnemyState.Damaged;
            print("���� ��ȯ: Any state -> Damaged");
            if (hp <= groggyHp && groggyState == NOT_GROGGY)
            {
                mState = EnemyState.Groggy;
                _anim.SetBool(OnStun, false);
                _anim.SetBool(OnGroggy, true);
                print("���� ��ȯ: Any State -> Groggy");
                Groggy();
            }
            else
            {
                OnDamageUI(hitPower);
                _anim.SetTrigger(Damaged1);
                Damaged(prevState);
                //���ǰ����� �ޱ� �߰� (����)
                _playerScript.UpdateMP(attackMPUP);
            }
        }
        else
        {
            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            capsuleCollider.enabled = false;
            //�������� üũ(��)
            StageCheck.instance.EnemyDead(gameObject);

            print("���� ��ȯ: Any state -> Die");
            Die();
        }
    }

    public void AttackAction()
    {
        _playerScript.UpdateHP(-attackPower);
        Debug.Log("PlayerHit");
    }

    private void Groggy()
    {
        if (groggyTimer < 0.0f)
        {
            groggyState = WAS_GROGGY;
            mState = EnemyState.Idle;
            _anim.SetBool(OnGroggy, false);
            Destroy(_groggyUIObj);
            return;
        }

        groggyTimer -= Time.deltaTime;
        _navMeshAgent.isStopped = true;

        // ���� �߾ӿ� E��� �۾��� ���
        OnGroggyUI();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnDamageUI(int damageValue)
    {
        GameObject damage = Instantiate(damageUI, canvas.transform);
        Text damageText = damage.GetComponent<Text>();
        damageText.text = Convert.ToString(damageValue);

        DamageSystem ds = damage.GetComponent<DamageSystem>();
        ds.DamageMove(damagePos);
        Destroy(damage, 2);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnGroggyUI()
    {
        if (_groggyUIObj == null)
        {
            _groggyUIObj = Instantiate(groggyUI, canvas.transform);
            _rectTransformGroggyUI = _groggyUIObj.GetComponent<RectTransform>();
        }

        _rectTransformGroggyUI.anchoredPosition = Camera.main.WorldToScreenPoint(groggyPos.transform.position);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnDieWhenStun()
    {
        print("OnDieWhenStun ȣ��");
        // �ֺ��� ������ Ž���ϰ�
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, stunDeadRadius, _enemyLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyFsmJiwon enemy = hitCollider.GetComponent<EnemyFsmJiwon>();
            if (!enemy) continue;
            // �ֺ� ������ ���� �������� Ȯ���ϰ�;
            if (enemy.mState == EnemyState.Stun)
            {
                //�������� üũ(��)
                StageCheck.instance.EnemyDead(enemy.gameObject);
                enemy.Die();

            }
        }
    }

    public void OnStunChanged()
    {
        if (mState != EnemyState.Die)
        {
            mState = EnemyState.Stun;
        }

        stunTimer = stunTime;
        _navMeshAgent.isStopped = true;
    }

    private void Stun()
    {
        if (stunTimer < 0.0f)
        {
            mState = EnemyState.Idle;
            _anim.SetBool(OnStun, false);
            return;
        }

        stunTimer -= Time.deltaTime;
        _anim.SetBool(OnStun, true);
    }

    // private void PlayByState()
    // {
    //     print("_currentSoundTime  :" + _currentSoundTime);
    //     if (mState is EnemyState.Groggy or EnemyState.Die) return;
    //     // if (prevEnemyState == mState) return;
    //     if (_currentSoundTime < attackDelay) return;
    //     
    //     print("preSTate : " + prevEnemyState + "/mState : " + mState);
    //     
    //     _audioSource.Stop();
    //     _audioSource.loop = true;
    //     _audioSource.clip = audioClips[(int)mState];
    //     _audioSource.Play();
    //     _currentSoundTime = 0f;
    // }
}