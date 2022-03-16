using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrenade : WeaponBase
{
    [Header("Audio Clip")]
    [SerializeField]
    private AudioClip           audioClipFire;      // 공격 사운드

    [Header("Grenade")]
    [SerializeField]
    private GameObject          grenadePrefab;      // 수류탄 프리펩
    [SerializeField]
    private Transform           grenadeSpawnPoint;  // 수류탄 생성 위치

    private void OnEnable() 
    {
        // 무기가 활성화될 때 해당 무기의 탄창 정보를 갱신한다.
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);
        // 무기가 활성화될 때 해당 무기의 탄 수 정보를 갱신한다.
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }

    private void Awake() {
        base.Setup();

        // 처음 탄창 수는 최대로 설정
        weaponSetting.currentMagazine   = weaponSetting.maxMagazine;
        // 처음 탄 수는 최대로 설정
        weaponSetting.currentAmmo       = weaponSetting.maxAmmo;
    }

    public override void StartWeaponAction(int type = 0)
    {
        if ( type == 0 && isAttack == false && weaponSetting.currentAmmo > 0 )
        {
            StartCoroutine("OnAttack");
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
    }

    public override void StartReload()
    {
    }

    private IEnumerator OnAttack()
    {
        isAttack = true;

        // 공격 애니메이션 재생
        animator.Play("Fire", -1, 0);
        // 공격 사운드 재생
        PlaySound(audioClipFire);

        yield return new WaitForEndOfFrame();

        while (true)
        {
            // 애니메이션이 movement로 바뀌었다면 애니메이션이 종료됨을 뜻한다.
            // 다시 isAttack을 false로 바꾸어주어야한다.
            if ( animator.CurrentAnimationIs("Movement") )
            {
                isAttack = false;

                yield break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// arms_assault_rifle_01.fbx의
    /// knife_attack_1@assault_rifle_01 애니메이션 이벤트 함수
    /// </summary>
    public void SpawnGrenadeProjectile()
    {
        GameObject grenadeClone = Instantiate(grenadePrefab, grenadeSpawnPoint.position, Random.rotation);
        grenadeClone.GetComponent<WeaponGrenadeProjectile>().Setup(weaponSetting.damage, transform.parent.forward);

        weaponSetting.currentAmmo --;
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }

    public override void IncreaseMagazine(int ammo)
    {
        // 수류탄은 탄창이 따로 없고, 탄수(Ammo)를 수류탄 개수로 사용하기 때문에 탄수를 증가시킨다.
        weaponSetting.currentAmmo = weaponSetting.currentAmmo + ammo > weaponSetting.maxAmmo ?

                                    weaponSetting.maxAmmo : weaponSetting.currentAmmo + ammo;
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }
    
}
