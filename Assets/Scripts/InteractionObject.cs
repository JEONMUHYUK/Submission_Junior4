using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 추상클래스는 인스턴스(객체)를 만들 수 없는 특별한 클래스
// 추상메서드를 만들고 이것을 상속을 통해서 파생클래스에서 구현하도록 하는것.
public abstract class InteractionObject : MonoBehaviour
{
    [Header("Interaction Object")]
    [SerializeField]
    protected   int     maxHP = 100;
    protected   int     currentHP;

    private void Awake() {
        currentHP = maxHP;
    }

    // 추상메서드에서 상속시 각각 따로 정의할 부분은 아래와 같이 추상메서드로 하여
    // 메서드를 정의할 곳에서 
    // public override voud TakeDamage(int damage){ 데미지 정의 } 로 구현하면 된다.
    public abstract void TakeDamage(int damage);
}
