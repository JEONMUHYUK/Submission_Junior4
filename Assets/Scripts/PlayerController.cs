using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Movement3D movement3D;
    // Start is called before the first frame update
    private void Awake()
    {
        movement3D = GetComponent<Movement3D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");  // 방향키 좌/우 움직임
        float z = Input.GetAxisRaw("Vertical");    // 방향키 상/하 움직임

        movement3D.MoveTo(new Vector3(x, 0, z));
    }
}
