using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : PoolableMono
{
    Rigidbody2D rb;
    PlayerController pc;
    BossHp bossHp;

    private float lifeTime = 1f;
    private float damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        transform.right = rb.velocity;   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            damage = PlayerPrefs.GetFloat("AttackPower", 0);
            bossHp = collision.GetComponentInParent<BossHp>();
            Debug.Log("hit");

            if (collision.gameObject.CompareTag("BossHead"))
            {
                bossHp.Damaged(damage);
            }
            else if(collision.gameObject.CompareTag("BossBody"))
            {
                bossHp.Damaged(damage / 2);
            }
        }

        PoolManager.Instance.Push(this);
    }

    public override void Reset(Vector3 Position, Quaternion Rotation)
    {
        transform.position = Position;
        transform.rotation = Rotation;
    }
}
