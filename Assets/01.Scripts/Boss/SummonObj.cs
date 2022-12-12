using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonObj : PoolableMono
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Transform playerTrm;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTrm = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void OnEnable()
    {
        StartCoroutine(Move());
    }

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PoolManager.Instance.Push(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PoolManager.Instance.Push(this);
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(3); 

        Vector2 dir = playerTrm.position - transform.position;

        rb.velocity = dir * speed;
    }

    public override void Reset(Vector3 Position, Quaternion Rotation)
    {
        transform.position = Position;
    }
}
