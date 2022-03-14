using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }

public class Status : MonoBehaviour
{
    [HideInInspector]
    public HPEvent onHPEvent = new HPEvent();

    [Header("Walk, Run Speed")] // 걷는 속도 뛰는 속도 변수를 선언
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    [Header("HP")]
    [SerializeField]
    private int         maxHP = 100;
    private int         currentHP;

//--------------------------------------------------------------------------------------------------
    // => : 람다 연산자 (간이 함수로 생각) + Property
    // WalkSpeed => walkSpeed; ==  WalkSpeed { get { return walkSpeed; } }
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;
    // 외부에서 값을 정의하는 용도로 get Property를 정의한다.
//---------------------------------------------------------------------------------------------------

    private void Awake() 
    {
        currentHP = maxHP;
    }

    public bool DecreaseHP(int damgage)
    {   
        // 현재 HP를 previousHP 변수에 저장
        int previousHP = currentHP;

        currentHP = currentHP - damgage > 0 ? currentHP - damgage : 0;

        // cuurentHP 값이 바뀌었기 때문에 Invoke 호출
        onHPEvent.Invoke(previousHP, currentHP);

        if ( currentHP == 0)
        {
            return true;
        }

        return false;
    }
}
