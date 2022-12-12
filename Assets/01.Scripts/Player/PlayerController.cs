using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region
    [Header("움직임")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpStamina;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashStamina;
    [SerializeField] private GameObject groundChecker;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private StageData stageData;

    private Vector2 moveVec;
    private Vector2 dashVec;

    public bool lookLeft;

    private float h;
    #endregion

    #region
    [Header("움직임 bool값")]
    [SerializeField] private bool isDash = false;
    [SerializeField] private bool isAttack = false;
    [SerializeField] private bool isJump = false;
    [SerializeField] private bool isActivity = false;
    #endregion

    #region
    [Header("공격")]
    [SerializeField] private float attackPower = 0;
    [SerializeField] private float maxAttackPower;
    [SerializeField] private GameObject arrowPref;
    [SerializeField] private Transform fireTrm;
    #endregion

    [SerializeField] private float InvincibilityTime = 2f; 

    Rigidbody2D rb;
    SpriteRenderer sp;
    Animator animator;
    PlayerStats stats;
    Transform bossTrm;

    public float Power => attackPower;
    public float MaxPower => maxAttackPower;
    public bool IsActivity => isActivity;

    public Pause pause;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();
        bossTrm = GameObject.Find("Boss").GetComponent<Transform>();
    }

    private void Update()
    {
        if(!pause.isOpen)
        {
            Move();
            Jump();
            Attack();
            Dash();
            CheckGround();
            CheckActivity();
            Rotate();
            if (stats.Hp <= 0) { Die(); }
        }       
    }
    
    private void Move()
    {
        if(!isDash && !isAttack)
        {
            Animation();
            h = Input.GetAxisRaw("Horizontal");
            moveVec = new Vector2(h * moveSpeed, rb.velocity.y);
       
            rb.velocity = moveVec;
        }
        else { animator.SetBool("isMove", false); }

        Vector2 limitPos = transform.position;
        limitPos.x = Mathf.Clamp(limitPos.x, stageData.MinX, stageData.MaxX);
        transform.position = limitPos;
    }

    private void Jump()
    {
        if(Input.GetButtonDown("Jump") && CheckGround() && !isDash && !isAttack && stats.CanActivity)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            stats.Stamina -= jumpStamina;
        }
    }

    private void Attack()
    {
        if(!isDash && stats.CanActivity)
        {
            if(Input.GetMouseButtonDown(0))
            {
                isAttack = true;
                animator.SetBool("isFire", true);
                UIManager.Instance.ApperUI("PlayerPowerSlider");
            }

            if(Input.GetMouseButton(0)) 
            { 
                rb.velocity = new Vector2(0, rb.velocity.y);
                attackPower += Time.deltaTime * 40; 

                if(attackPower >= maxAttackPower) { attackPower = maxAttackPower; }
            }

            if(Input.GetMouseButtonUp(0) && isAttack)
            {
                PoolableMono arrow = PoolManager.Instance.Pop("Arrow", fireTrm.position, fireTrm.rotation);
                Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
                arrowRb.AddForce(arrow.transform.right * attackPower, ForceMode2D.Impulse);

                isAttack = false;
                animator.SetBool("isFire", false);
                UIManager.Instance.DisapperUI("PlayerPowerSlider");

                PlayerPrefs.SetFloat("AttackPower", attackPower);
                stats.Stamina -= attackPower / 2;
                Debug.Log(attackPower);

                attackPower = 0;
            }
        }
        
    }

    private void Dash()
    {
        if(Input.GetButtonDown("Dash") && !isDash && !isAttack && CheckGround() && stats.CanActivity)
        {   
            StartCoroutine(DoDash());
        }
    }

    private IEnumerator DoDash()
    {
        isDash = true;
        animator.SetBool("isDash", true);
        moveSpeed *= 4;
        gameObject.layer = LayerMask.NameToLayer("Invincibility");

        dashVec = new Vector2(rb.velocity.normalized.x * moveSpeed, 0);
        rb.velocity = dashVec;

        stats.Stamina -= dashStamina;

        yield return new WaitForSeconds(dashTime);

        isDash = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
        animator.SetBool("isDash", false);
        moveSpeed *= 0.25f;
    }

    public IEnumerator OnDamage(float damage)
    {
        Debug.Log(1);
        Vector2 dir = (transform.position - bossTrm.transform.position).normalized;

        stats.Hp -= damage;
        gameObject.layer = LayerMask.NameToLayer("Invincibility");
        sp.color = Color.red;    

        yield return new WaitForSeconds(InvincibilityTime);

        Debug.Log(2);
        gameObject.layer = LayerMask.NameToLayer("Player");
        sp.color = Color.white;
    }

    private void Die()
    {
            sp.color = Color.white;
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            Destroy(gameObject.GetComponent<BoxCollider2D>());
            Destroy(gameObject.GetComponent<PlayerController>());
            Destroy(gameObject.GetComponent<PlayerStats>());
            animator.SetTrigger("isDie");
    }

    private bool CheckGround()
    {
        Vector2 pos = groundChecker.transform.position;
        float radios = 0.15f;

        Collider2D col = Physics2D.OverlapCircle(pos, radios, groundLayer);
        isJump = !col;

        return col;
    }

    private void CheckActivity()
    {
        if(isJump || isAttack || isDash) { isActivity = true; }
        else { isActivity = false; }   
    }

    private void Rotate()
    {
        if (h > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            lookLeft = false;
        }
        else if (h < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            lookLeft = true;
        }

        if(isAttack)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            if(mousePos.x >= 0) 
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                lookLeft = false;
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                lookLeft = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("summonObj"))
        {
            StartCoroutine(OnDamage(10));
        }
    }

    private void Animation()
    {
        if(Input.GetButton("Horizontal")) { animator.SetBool("isMove", true); }
        if(Input.GetButtonUp("Horizontal") || h == 0) { animator.SetBool("isMove", false); }
    }
}
