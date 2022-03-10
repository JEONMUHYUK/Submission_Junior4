using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;

    private void Awake() {
        // "Player" 오브젝트 기준으로 자식 오브젝트인
        // "arms_assault_rifle_01" 오브젝트에 Animator 컴포넌트가 있다.
        animator = GetComponentInChildren<Animator>();
    }

    public float MoveSpeed
    {
        set => animator.SetFloat("movementSpeed", value);
        get => animator.GetFloat("movementSpeed");
        // animator.SetFloat("ParaName", value);
        // -> Animator View에 있는 float 타입 변수 "ParaName"의 값을 value로 설정한다.
        // float f = animator.GetFloat("ParaName");
        // -> Animator View에 있는 float 타입 변수 "ParaName"의 값을 반환한다.
    }

    public void OnReload()
    {
        animator.SetTrigger("onReload");                                    // animator의 onReload 파라미터를 활성화(on) 시킨다.
    }

    public void Play(string statName, int layer, float normalizedTime) 
    {
        //외부에서 호출할 수 있도록 Play메서드 정의
        animator.Play(statName, layer, normalizedTime);
    }
    
    public bool CurrentAnimationIs(string name)
    {   // 매게변수로 받아온 name(animation)이 현재 재생중인지 확인하고 그 결과를 반환한다.
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

}
