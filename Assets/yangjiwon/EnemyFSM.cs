using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyFSM : MonoBehaviour
{
    private EnemyState m_State;


    private Transform player;

    private CharacterController _characterController;

    public float findDistance = 8f;
    public float moveSpeed = 5f;
    public float attackDistance = 2f;
    
    void Start()
    {
        m_State = EnemyState.Idle;
        player = GameObject.Find("Player").transform;
        _characterController = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Return:
                Attack();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    private void Die()
    {
        throw new System.NotImplementedException();
    }

    private void Damaged()
    {
        throw new System.NotImplementedException();
    }

    private void Attack() => throw new System.NotImplementedException();

    private void Move()
    {
        if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            var dir = (player.position - transform.position).normalized;

            _characterController.Move(dir * (moveSpeed * Time.deltaTime));
        }
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환: Move -> Attack");
        }
    }

    private void Idle()
    {
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환 : Idle -> Move");
        }
    } 
}
