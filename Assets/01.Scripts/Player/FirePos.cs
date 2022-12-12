using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePos : MonoBehaviour
{
    private PlayerController pc;

    private void Awake()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(pc.lookLeft) { angle = Mathf.Atan2(dir.y, -dir.x) * Mathf.Rad2Deg; }

        angle = Mathf.Clamp(angle, -80, 80);

        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
