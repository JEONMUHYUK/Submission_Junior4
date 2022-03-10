using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAssaultRifle : MonoBehaviour
{
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject      muzzleFlashEffect;          //총구 이펙트(On/Off)

    [Header("Audio Clips")] // 굵은 글씨체로 인스펙터 창에 보이게 할 수 있다.
    [SerializeField]
    private AudioClip       audioClipTakeOutWeapon;     // 무기 장착 사운드
    [SerializeField]
    private AudioClip       audioClipFire;              // 공격 사운드

    [Header("Spawn Points")]
    [SerializeField]
    private Transform       casingSpawnPoint;           // 탄피 생성 위치

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting   weaponSetting;              //무기 설정

    private float           lastAttackTime = 0;         //마지막 발사시간 체크용

    private AudioSource             audioSource;        //사운드 재생 컴포넌트
    private PlayerAnimatorController animator;          // 애니메이션 재생 제어
    private CasingMemoryPool casingMemoryPool;          // 탄피 생성 후 활성/비활성 관리

    private void Awake()
    {
        audioSource         = GetComponent<AudioSource>(); //오디오 소스 컴포넌트를 불러와 변수에 저장.
        animator            = GetComponentInParent<PlayerAnimatorController>();
        casingMemoryPool    = GetComponent<CasingMemoryPool>();
    }

    private void OnEnable() //무기 오브젝트가 활성화 될때 호출되는 함수.
    {
        // 무기 장착 사운드 재생
        PlaySound(audioClipTakeOutWeapon);
        // 총구 이펙트 비활성화
        muzzleFlashEffect.SetActive(false);
    }

    public void StartWeaponAction(int type=0) //마우스 타입 Mouse(0)
    {
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

    private void PlaySound(AudioClip clip) //무기장착 사운드를 파라미터로 받아온다.
    {
        audioSource.Stop(); // 기존에 재생중인 사운드를 정지하고,
        audioSource.clip = clip; // 새로운 사운드 clip으로 교체후
        audioSource.Play(); //사운드를 재생한다.
    }
    
}
