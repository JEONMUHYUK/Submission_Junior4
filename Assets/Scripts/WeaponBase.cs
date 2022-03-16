using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Main = 0, Sub, Melee, Throw}

// UnityEvent 클래스의 일반화 정의에 따라 호출할 수 있는 이벤트 메소드들의 매게변수가 결정된다.
[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int>{ }
[System.Serializable]
public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }

public abstract class WeaponBase : MonoBehaviour
{
    [Header("WeaponBase")]
    // WeaponBase를 상속받는 무기 클래스에서 사용할 수 있도록 접근자를 protected로 설정
    [SerializeField]
    protected   WeaponType          weaponType;         // 무기종류
    [SerializeField]
    protected   WeaponSetting       weaponSetting;      // 무기 설정

    protected float           lastAttackTime = 0;         // 마지막 발사시간 체크용
    protected bool            isReload = false;           // 재장전 중인지 체크
    protected bool            isAttack = false;           // 공격 여부 체크용
    protected AudioSource             audioSource;        // 사운드 재생 컴포넌트
    protected PlayerAnimatorController animator;          // 애니메이션 재생 제어

    // 외부에서 이벤트 함수 등록을 할 수 있도록 public 선언
    [HideInInspector]                                   // 인스펙터 창에서 변수를 숨긴다.
    public AmmoEvent        onAmmoEvent     = new AmmoEvent();
    [HideInInspector]
    public MagazineEvent    onMagazineEvent = new MagazineEvent();



     // 외부에서 필요한 정보를 열람하기 위해 정의한 Get Property's
    public PlayerAnimatorController         Animator => animator;
    public WeaponName                       WeaponName => weaponSetting.weaponName;
    public int                              CurrentMagazine => weaponSetting.currentMagazine;
    public int                              MaxMagazine => weaponSetting.maxMagazine;

    public abstract void StartWeaponAction(int type=0);
    public abstract void StopWeaponAction(int type=0);
    public abstract void StartReload();

    protected void PlaySound(AudioClip clip)
    {
        audioSource.Stop();         // 기존 재생중인 사운드를 정지하고,
        audioSource.clip = clip;    // 새로운 사운드 clip으로 교체후
        audioSource.Play();         // 사운드 재생
    }

    protected void Setup()
    {
        audioSource = GetComponent<AudioSource>();
        animator    = GetComponent<PlayerAnimatorController>();
    }

    public virtual void IncreaseMagazine(int magazine)
    {
        // ? abstract와 달리 virtual을 override가 선택이다.
        // 각 무기 클래스에서 IncreaseMagazine()을 재정의(override) 하지 않으면 weaponBase클래스에 있는
        // IncreaseMagazine()이 호출된다.
        weaponSetting.currentMagazine = CurrentMagazine + magazine > MaxMagazine ? MaxMagazine : CurrentMagazine + magazine;

        onMagazineEvent.Invoke(CurrentMagazine);
    }
}
