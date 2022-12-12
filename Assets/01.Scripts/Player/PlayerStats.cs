using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float hp; 
    [SerializeField] private float maxHp = 100;
    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float recoveryTime;
    [SerializeField] private float recoveryValue;
    [SerializeField] private bool canActivity;
    private bool isRecovry = false;
    public float recoveryAmount = 0f;

    public float Hp { get => hp; set => hp = value; }
    public float MaxHp { get => maxHp; }
    public float Stamina { get => stamina; set => stamina = value; }
    public float MaxStamina { get => maxStamina; }
    public bool CanActivity => canActivity;

    PlayerController pc;

    private void Awake()
    {
        pc = GetComponent<PlayerController>();  

        hp = maxHp;
        stamina = maxStamina;
    }

    private void Update()
    {

        stamina = Mathf.Max(stamina, 0);

        if(!pc.IsActivity) 
        {
            if(!isRecovry)
            {
                isRecovry = true;
                StartCoroutine("Recovery");
            }            
        }
        else 
        {
            isRecovry = false;
            StopCoroutine("Recovery");
            Debug.Log("stop recovery");
        }

        if(stamina <= 0) { canActivity = false; }
        else { canActivity = true; }
    }

    private IEnumerator Recovery()
    {
        Debug.Log("start recovery");

        yield return new WaitForSeconds(recoveryTime);
        
        while(true)
        {
            stamina += recoveryAmount * Time.deltaTime;

            yield return null;
        }
    }
}
    