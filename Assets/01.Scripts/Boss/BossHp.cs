using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
    [SerializeField] private float maxHp = 100; 
    [SerializeField] private float hp;

    //private Slider hpBar;

    public float Hp => hp;
    public float MaxHp => maxHp;

    private void Awake()
    {
        //hpBar = GameObject.Find("BossHpSlider").GetComponent<Slider>();
        hp = maxHp;
    }

    private void Update()
    {
        //hpBar.value = hp / maxHp;
    }

    public void Damaged(float damage)
    {
        hp -= damage;
    }
}
