using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    public EnemyState m_State;

    // Start is called before the first frame update
    void Start()
    {
        m_State = EnemyState.Idle;   
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
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
            default:

                break;
        }
    }

    void Idle()
    {

    }
    void Move()
    {

    }
    void Attack()
    {

    }

    void Return()
    {

    }

    void Damaged()
    {

    }
    
    void Die()
    {

    }

}
