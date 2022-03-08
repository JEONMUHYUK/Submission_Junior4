using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Walk, Run Speed")] // 걷는 속도 뛰는 속도 변수를 선언
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

//--------------------------------------------------------------------------------------------------
    // => : 람다 연산자 (간이 함수로 생각) + Property
    // WalkSpeed => walkSpeed; ==  WalkSpeed { get { return walkSpeed; } }
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

    // 외부에서 값을 정의하는 용도로 get Property를 정의한다.
//---------------------------------------------------------------------------------------------------
}
