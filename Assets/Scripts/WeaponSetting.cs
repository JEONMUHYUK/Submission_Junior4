using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기의 종류가 여러 종류일 때 공용으로 사용하는 변수들은 구조체로 묶어서 정의하면
// 변수가 추가/삭제될 때 구조체에 선언하기 때문에 추가/삭제에 대한 관리과 용이하다.

// Tip. 구조체는 스택영역, 클래스는 힙 영역에 메모리가 할당된다.
// Tip. [System.Serializable]을 이용해 직렬화를 하지 않으면 다른 클래스의 변수로 생성되었을 때 Inspector 창에 멤버 변수들의 목록이 뜨지않는다.

// WeapomName 열거형 정의
public enum WeaponName { AssaultRifle = 0 }

[System.Serializable]
public struct WeaponSetting 
{
    public WeaponName       weaponName;             // 무기이름
    public int              currentMagazine;        // 현재 탄창 수
    public int              maxMagazine;            // 최대 탄창 수
    public int              currentAmmo;            // 현재 탄약수
    public int              maxAmmo;                // 최대 탄약수
    public float            attackRate;             // 공격속도
    public float            attackDistance;         // 공격 사거리
    public bool             isAutomaticAttack;      // 연속 공격 여부
}