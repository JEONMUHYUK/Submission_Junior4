using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitchSystem : MonoBehaviour
{
    [SerializeField]
    private PlayerController    playerController;
    [SerializeField]
    private PlayerHUD           playerHUD;

    [SerializeField]
    private WeaponBase[]        weapons;            // 소지중인 무기 4종류

    private WeaponBase          currentWeapon;      // 현재 사용중인 무기
    private WeaponBase          previousWeapon;     // 직전에 사용했던 무기

    private void Awake() {
        // 무기 정보 출력을 위한 현재 소지중인 모든 무기 이벤트 등록
        playerHUD.SetupAllWeapons(weapons);

        // 현재 소지중인 모든 무기를 보이지 않게 설정
        for (int i = 0; i < weapons.Length; ++ i)
        {
            if ( weapons[i].gameObject != null )
            {
                weapons[i].gameObject.SetActive(false);
            }
        }

        // Main 무기를 현재 사용 무기로 설정
        SwitchingWeapon(WeaponType.Main);
    }

    private void Update() {
        UpdateSwitch();
    }

    private void UpdateSwitch()
    {
        if ( !Input.anyKeyDown ) return;
        
        // 1~4 숫자키를 누르면 무기 교체
        int inputIndex = 0;

        // ? -- built-in method --
        // ? bool result = int.TryParse(string key, out data);
        // ? key 문자열을 숫자로 형변환 해서 data에 저장 성공하면 "result : true", 실패하면 "result : false"
        // int.TryParse를 호출해 키가 입력되었을 때 input된 키가 숫자이면 inputIndex 변수에 저장하고,
        // 1에서 4사이의 값이면 SwitchingWeapon 메소드를 호출한다.
        if ( int.TryParse(Input.inputString, out inputIndex) && (inputIndex > 0 && inputIndex < 5) )
        {
            SwitchingWeapon((WeaponType)(inputIndex - 1));
        }
    }

    private void SwitchingWeapon(WeaponType weaponType)
    {
        // 교체 가능한 무기가 없으면 종료
        if ( weapons[(int)weaponType] == null)
        {
            return;
        }

        // 현재 사용중인 무기가 있으면 이전 무기정보에 저장
        if ( currentWeapon != null)
        {
            previousWeapon = currentWeapon;
        }
        
        // 무기 교체
        currentWeapon = weapons[(int)weaponType];

        // 현재 사용중인 무기로 교체하려고 할 때 종료
        if ( currentWeapon == previousWeapon )
        {
            return;
        }

        // 무기를 사용하는 PlayerController, PlayerHUD에 현재 무기 정보 전달
        playerController.SwitchingWeapon(currentWeapon);
        playerHUD.SwitchingWeapon(currentWeapon);

        // 이전에 사용하던 무기 비활성화
        if ( previousWeapon != null )
        {
            previousWeapon.gameObject.SetActive(false);
        }
        // 현재 사용하는 무기 활성화
        currentWeapon.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// 첫 번째 매개변수에 설정된 하나의 무기 탄창 수 증가
    /// </summary>
    public void IncreaseMagazine(WeaponType weaponType, int magazine)
    {
        // ? WeaponType weaponType : 증가시킬 무기 정보 , int magazine : 증가하는 탄창 수.
        // 해당 무기가 있는지 검사
        if ( weapons[(int)weaponType] != null )
        {
            // 해당 무기의 탄창수를 magazine 만큼 증가
            weapons[(int)weaponType].IncreaseMagazine(magazine);
        }
    }
        
    

    /// <summary>
    /// 소지중인 모든 무기의 탄창 수 증가
    /// </summary>
    public void IncreaseMagazine(int magazine)
    {
        for (int i = 0; i < weapons.Length; ++ i)
        {
            if ( weapons[i] != null )
            {
                weapons[i].IncreaseMagazine(magazine);
            }
        }
    }
}
