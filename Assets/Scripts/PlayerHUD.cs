using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    private WeaponBase                  weapon;                 // 현재 정보가 출력되는 무기

    [Header("Components")]
    [SerializeField]
    private Status                      status;                 // 플레이어의 상태 ( 이동속도, 체력)

    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI             textWeaponName;         // 무기 이름
    [SerializeField]
    private Image                       imageWeaponIcon;        // 무기 아이콘
    [SerializeField]
    private Sprite[]                    spriteWeaponIcons;      // 무기 아이콘에 사용되는 sprite 배열
    [SerializeField]
    private Vector2[]                   sizeWeaponIcons;        // 무기 아이콘 UI 크기 배열

    [Header("Ammo")]
    [SerializeField]
    private TextMeshProUGUI             textAmmo;               // 현재/최대 탄 수 출력 Text   

    [Header("Magazine")]
    [SerializeField]
    private GameObject                  magazieUIPrefab;        // 탄창 UI 프리펩
    [SerializeField]
    private Transform                   magazineParent;         // 탄창 UI가 배치되는 Pannel
    [SerializeField]
    private int                         maxMagazineCount;       // 처음 생성하는 최대 탄창 수

    private List<GameObject>            magazineList;           // 탄창 UI 리스트

    [Header("HP & BloodScreen UI")]
    [SerializeField]
    private TextMeshProUGUI             textHP;                 // 플레이어 체력을 출력하는 Text
    [SerializeField]
    private Image                       imageBloodScreen;       // 플레이어가 공격받았을 때 화면에 표시되는 Image
    [SerializeField]
    private AnimationCurve              curveBloodScreen;       


    private void Awake() 
    {
        // 메소드가 등록되어 있는 이벤트 클래스(weapon.xx)의
        // Invoke() 메소드가 호출될 때 등록된 메소드(매게변수)가 실행된다.
        status.onHPEvent.AddListener(UpdateHPHUD);
    }

    public void SetupAllWeapons(WeaponBase[] weapons)
    {
        // 최대 탄창수 만큼 탄창  UI를 만든다.
        SetupMagazie();

        // 사용 가능한 모든 무기의 이벤트 등록
        for (int i = 0; i < weapons.Length; ++ i)
        {
            // 소지중인 모든 무기의 이벤트에 메소드를 등록해 현재 활성화되어 있는 무기의 Ammo, Magazine 정보를 업데이트한다.
            weapons[i].onAmmoEvent.AddListener(UpdateAmmoHUD);
            weapons[i].onMagazineEvent.AddListener(UpdateMagazieHUD);
        }
    }

    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        weapon = newWeapon;
        // 바뀐 무기의 정보를 화면에 출력
        SetupWeapon();
    }

    private void SetupWeapon()
    {
        textWeaponName.text = weapon.WeaponName.ToString();

        // 최대 탄창수 만큼 탄창  UI를 만든다.
        imageWeaponIcon.sprite = spriteWeaponIcons[(int)weapon.WeaponName];
        // 이미지가 바뀔 때 찌그려져 보이지 않도록 이미지 크기를 재설정한다.
        imageWeaponIcon.rectTransform.sizeDelta = sizeWeaponIcons[(int)weapon.WeaponName];
    }

    private void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
    {
        textAmmo.text = $"<size=40>{currentAmmo}/</size>{maxAmmo}";
    }

    private void SetupMagazie()
    {
        // weapon에 등록되어 있는 최대 탄창 개수만큼 Image Icon을 생성
        // magazineParent 오브젝트의 자식으로 등록 후 모두 비활성화/리스트에 저장
        magazineList = new List<GameObject>();
        for (int i = 0; i < maxMagazineCount; ++ i)
        {
            GameObject clone = Instantiate(magazieUIPrefab);
            clone.transform.SetParent(magazineParent);
            clone.SetActive(false);

            magazineList.Add(clone);
        }

    }

    private void UpdateMagazieHUD(int currentMagazine)
    {
        // 전부 비활성화 하고, currentMagazine 개수만큼 활성화
        for (int i = 0; i < magazineList.Count; ++ i)
        {
            magazineList[i].SetActive(false);
        }
        for (int i = 0; i < currentMagazine; ++ i)
        {
            magazineList[i].SetActive(true);
        }
    }

    private void UpdateHPHUD(int previous, int current)
    {
        textHP.text = "HP "+current;

        // 체력이 증가했을 때는 화면에 빨간색 이미지를 출력하지 않도록 return
        if (previous <= current) return;

        // 체력이 감소 하였으면 OnBloodScreen 호출
        if ( previous - current > 0 )
        {
            StopCoroutine("OnBloodScreen");
            StartCoroutine("OnBloodScreen");
        }
    }

    private IEnumerator OnBloodScreen()
    {
        float percent = 0;

        while ( percent < 1)
        {
            percent += Time.deltaTime;

            Color color             = imageBloodScreen.color;
            // BloodScreen alpha 값을 1에서 0까지 1초동안 감소시켜 플레이어가 공격받았음을 화면에 표시.
            color.a                 = Mathf.Lerp(1, 0, curveBloodScreen.Evaluate(percent));
            imageBloodScreen.color = color;

            yield return null;
        }
    }
}
