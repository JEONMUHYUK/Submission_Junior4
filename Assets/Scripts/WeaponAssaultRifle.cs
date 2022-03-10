using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UnityEvent 클래스의 일반화 정의에 따라 호출할 수 있는 이벤트 메소드들의 매게변수가 결정된다.
[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }

public class WeaponAssaultRifle : MonoBehaviour
{
    [HideInInspector]                                   // 인스펙터 창에서 변수를 숨긴다.
    public AmmoEvent        onAmmoEvent = new AmmoEvent();

    [Header("Fire Effects")]
    [SerializeField]
    private GameObject      muzzleFlashEffect;          // 총구 이펙트(On/Off)

    [Header("Audio Clips")] // 굵은 글씨체로 인스펙터 창에 보이게 할 수 있다.
    [SerializeField]
    private AudioClip       audioClipTakeOutWeapon;     // 무기 장착 사운드
    [SerializeField]
    private AudioClip       audioClipFire;              // 공격 사운드
    [SerializeField]
    private AudioClip       audioClipReload;            // 재장전 사운드

    [Header("Spawn Points")]
    [SerializeField]
    private Transform       casingSpawnPoint;           // 탄피 생성 위치

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting   weaponSetting;              // 무기 설정

    private float           lastAttackTime = 0;         // 마지막 발사시간 체크용
    private bool            isReload = false;           // 재장전 중인지 체크

    private AudioSource             audioSource;        // 사운드 재생 컴포넌트
    private PlayerAnimatorController animator;          // 애니메이션 재생 제어
    private CasingMemoryPool casingMemoryPool;          // 탄피 생성 후 활성/비활성 관리

    // 외부에서 필요한 정보를 열람하기 위해 정의한 Get Property's
    public WeaponName WeaponName => weaponSetting.weaponName;

    private void Awake()
    {
        audioSource         = GetComponent<AudioSource>(); //오디오 소스 컴포넌트를 불러와 변수에 저장.
        animator            = GetComponentInParent<PlayerAnimatorController>();
        casingMemoryPool    = GetComponent<CasingMemoryPool>();

        // 처음 탄 수는 최대로 설정
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }

    private void OnEnable() //무기 오브젝트가 활성화 될때 호출되는 함수.
    {
        // 무기 장착 사운드 재생
        PlaySound(audioClipTakeOutWeapon);
        // 총구 이펙트 비활성화
        muzzleFlashEffect.SetActive(false);

        // 무기가 활성화될 때 해당 무기의 탄 수 정보를 갱신한다.
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }

    public void StartWeaponAction(int type=0) //마우스 타입 Mouse(0)
    {
        // 재장전 중일 때는 무기 액션을 할 수 없다.
        if ( isReload == true ) return;                 // return으로 바로 반환시켜 함수를 종료한다.
        // 마우스 왼쪽 클릭(공격 시작)
        if( type == 0 )
        {
            //연속 공격
            if ( weaponSetting.isAutomaticAttack == true)
            {
                StartCoroutine("OnAttackLoop");
            }
            // 단발 공격
            else
            {
                OnAttack();
            }
        }
    }

    public void StopWeaponAction(int type=0)
    {
        //마우스 왼쪽 클릭 (공격 종료)
        if (type==0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }

    public void StartReload()
    {
        // 현재 재장전 중이면 재장전 불가능
        if ( isReload == true ) return;

        // 무기 액션 도중에 'R' 키를 눌러 재장전을 시도하면 무기 액션 종료 후 재장전
        StopWeaponAction();

        StartCoroutine("OnReload");
    }

    private IEnumerator OnAttackLoop()
    {
        while (true)
        {
            OnAttack();
            yield return null;
        }
    }

    public void OnAttack()
    {
        if(Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            // 뛰고 있을 때는 공격할 수 없다.
            if (animator.MoveSpeed > 0.5f)
            {
                return;
            }

            // 공격 주기가 되어야 공격할 수 있도록 하기 위해 현재 시간 저장
            lastAttackTime = Time.time;

            // 탄 수가 없으면 공격 불가능
            if ( weaponSetting.currentAmmo <= 0)
            {
                return;
            }
            // 공격시 currentAmmo 1 감소, 탄 수 UI 업데이트
            weaponSetting.currentAmmo --;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            //무기 애니메이션 재생
            animator.Play("Fire", -1, 0);

            // 총구 이펙트 재생
            StartCoroutine("OnMuzzleFlashEffect");
            // 공격 사운드 재생
            PlaySound(audioClipFire);
            // 탄피 생성
            casingMemoryPool.SapwnCasing(casingSpawnPoint.position, transform.right);
        }
    }

    private IEnumerator OnMuzzleFlashEffect()
    {   //무기의 공격속도보다 빠르게 muzzleFlashEffect를 잠깐 활성화한 후 비활성화 한다.
        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
        muzzleFlashEffect.SetActive(false);
    }

    private IEnumerator OnReload()
    {
        isReload = true;

        // 재장전 애니메이션, 사운드 재생
        animator.OnReload();
        PlaySound(audioClipReload);

        while ( true )
        {
            // 사운드가 재생중이 아니고, 현재 애니메이션이 Movement이면
            // 재장전 애니메이션(, 사운드) 재생이 종료되었다는 뜻.
            if ( audioSource.isPlaying == false && animator.CurrentAnimationIs("Movement") )
            {
                isReload = false;

                // 현재 탄 수를 최대로 설정하고, 바뀐 탄 수 정보를 Text UI에 업데이트
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

                yield break;
            }
            yield return null;
        }
    }

    private void PlaySound(AudioClip clip) //무기장착 사운드를 파라미터로 받아온다.
    {
        audioSource.Stop(); // 기존에 재생중인 사운드를 정지하고,
        audioSource.clip = clip; // 새로운 사운드 clip으로 교체후
        audioSource.Play(); //사운드를 재생한다.
    }
    
}
