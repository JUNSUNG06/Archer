using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private enum BossState
    {
        Idle = 0,
        Move = 1, 
        Attack = 2,
        spawn = 3,
        Summon = 4,
        Dash = 5
    }

    [SerializeField] private BossState state = BossState.spawn;

    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float dashDistance = 9f;
    [SerializeField] private float delayCycle = 2;
    [SerializeField] private float dashLength = 9f;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private StageData stageData;

    [SerializeField] private bool isDash = false;

    private int randValue;
    private Vector2 targetPos;
    private Vector2 dashDir;
    private Vector2 dashVector;

    Animator animator;
    BossHp bossHp;
    Transform playerTrm;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bossHp = GetComponent<BossHp>();
        playerTrm = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        Vector2 limitPos = transform.position;
        limitPos.x = Mathf.Clamp(limitPos.x, stageData.MinX, stageData.MaxX);
        transform.position = limitPos;

        switch (state)
        {
            case BossState.Idle:
                LookPlayer();
                animator.SetTrigger("isIdle");
                break;
            case BossState.Move: 
                Move();
                break;
            case BossState.Attack:
                animator.SetTrigger("NormalAttack");
                break;
            case BossState.Summon:
                animator.SetTrigger("Summon");
                break;
            case BossState.Dash:
                Dash();
                break;
        }

        Die();
    }

    private IEnumerator Idle()
    {
        Debug.Log("Idle");
        animator.SetTrigger("isIdle");
        delayCycle = UnityEngine.Random.Range(0, 2);
        
        yield return new WaitForSeconds(delayCycle);

        if(Vector2.Distance(playerTrm.position, transform.position) < dashDistance)
        {
            switch (randValue)
            {
                case 0:
                    state = BossState.Summon;
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                    state = BossState.Move;
                    break;
            }
        }
        else
        {
            switch (randValue)
            {
                case 0:
                case 1:
                    state = BossState.Summon;
                    break;
                case 2:
                case 3:
                case 4:
                    animator.SetTrigger("isDash");
                    break;
            }
        }
    }

    private void Move()
    {
        Debug.Log("move");

        if (Vector2.Distance(targetPos, transform.position) > attackDistance)
        {
            transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * 2);
        }
        else { state = BossState.Attack; }
    }

    private void Summon()
    {
        Debug.Log("summon");
        PoolManager.Instance.Pop("SummonObject", transform.localPosition, Quaternion.identity);
    }

    private void Attack()
    {
        Debug.Log("attack");
        Vector2 attackPos = new Vector2(transform.position.x - 0.7f, transform.position.y - 0.7f);
        float radius = 1f;

        Collider2D col = Physics2D.OverlapCircle(attackPos, radius, playerLayer);

        if (col != null)
        {
            Debug.Log("hit");
            StartCoroutine(col.GetComponent<PlayerController>().OnDamage(15));
        }
    }

    private void StartDash()
    {
        state = BossState.Dash;
        dashDir = (playerTrm.position - transform.position).normalized;
        dashVector = new Vector2(transform.position.x + dashDir.x * dashLength, transform.position.y);
    }

    private void Dash()
    {
        isDash = true;
        Vector2 attackPos = new Vector2(transform.position.x - 0.7f, transform.position.y - 0.7f);
        float attackRange = 1.3f;

        transform.position = Vector2.Lerp(transform.position, dashVector, Time.deltaTime * 20f);

        Collider2D col = Physics2D.OverlapCircle(attackPos, attackRange, playerLayer);

        if (col != null)
        {
            Debug.Log("hit");
            StartCoroutine(col.GetComponent<PlayerController>().OnDamage(15));
        }
    }

    private void Die()
    {


        if (bossHp.Hp <= 0)
        {
            animator.SetTrigger("OnDie");
            Destroy(gameObject, 2f);
        }
    }

    private void SetIdle()
    {
        Debug.Log("setIdle");
        state = BossState.Idle;

        isDash = false;
        randValue = UnityEngine.Random.Range(1, 5);
        targetPos = new Vector2(playerTrm.position.x, transform.position.y);

        StopCoroutine("Idle");
        StartCoroutine("Idle");
    }

    private void LookPlayer()
    {
        if(playerTrm.position.x - transform.position.x < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if(playerTrm.position.x - transform.position.x > 0) 
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
}