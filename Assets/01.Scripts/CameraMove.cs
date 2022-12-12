using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Transform playerTrm;

    private void Awake()
    {
        playerTrm = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(playerTrm.position.x, transform.position.y, transform.position.z);
    }
}
