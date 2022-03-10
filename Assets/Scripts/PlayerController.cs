using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCodes")]
    [SerializeField]
    private KeyCode     KeyCodeRun = KeyCode.LeftShift; // 달리기 키
    [SerializeField]
    private KeyCode     KeyCodeJump = KeyCode.Space;    // 점프 키
    [SerializeField]    
    private KeyCode     KeyCodeReload = KeyCode.R;      // 탄 재장전 키

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipWalk;                    // 걷기 사운드
    [SerializeField]
    private AudioClip audioClipRun;                     // 달리기 사운드

    private RotateToMouse                rotateToMouse; // 마우스 이동으로 카메라 회전
    private MoveCharacterController      movement;      // 키보드 입력으로 플레이어 이동, 점프
    private Status                       status;        // 이동속도 등의 플레이어 정보
    private PlayerAnimatorController     animator;      // 애니메이션 재생 제어 
    private AudioSource                  audioSource;   // 사운드 재생 제어
    private WeaponAssaultRifle           weapon;        // 무기를 이용한 공격 제어

    private void Awake() {
        // 마우스 커서를 보이지 않게 설정하고, 현재 위치에 고정시킨다.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotateToMouse = GetComponent<RotateToMouse>();
        movement = GetComponent<MoveCharacterController>();
        status = GetComponent<Status>();
        animator = GetComponentInChildren<PlayerAnimatorController>();
        audioSource = GetComponent<AudioSource>();
        weapon = GetComponentInChildren<WeaponAssaultRifle>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
        UpdateWeaponAction();
        
    }

    private void UpdateRotate() // 마우스 회전값 받아오기
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY); // 매게변수로 마우스의 값을 전달한다.
    }

    private void UpdateMove() // 이동값 받아오기
    {
        // GetAisxRaw 는 -1,0,1 만 반환하기 때문에 즉시 반응해야 한다면 Raw를 사용한다.
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 이동중 일 때 (걷기 or 뛰기)
        if ( x != 0 || z != 0) // x가 0이 아니고 z가 0이 아닐때
        {
            bool isRun = false; // 뛰고 있는지 확인하는 변수

            // 옆이나 뒤로 이동할 때는 달릴 수 없다.
            if (z > 0) isRun = Input.GetKey(KeyCodeRun); // 앞(z)으로 이동하고 있을 때 키를 입력하면 isRun은 true가 된다.

            // 삼항연산자
            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
            // isRun이 true 이면 status.RunSpeed 설정하고, false이면 status.WalkSpeed 가 설정된다.
            animator.MoveSpeed = isRun == true ? 1 : 0.5f;
            // 1 = 뛰기 애니메이션 0.5 = 걷기 애니메이션 animator.MoveSpeed 값을 전달한다
            audioSource.clip = isRun == true ? audioClipRun : audioClipWalk;

            // 방향키 입력여부는 매 프레임 확인하기 때문에 재생중일  떄는 다시 재생하지 않도록 isPlaying으로 체크해서 재생
            if(audioSource.isPlaying == false) // 오디오소스가 플레이 되지 않는 다면.
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        // 제자리에 멈춰 있을 때
        else
        {       
            movement.MoveSpeed = 0;
            animator.MoveSpeed = 0;
            //멈췄을 때 사운드가 재생중이면 정지
            if (audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }

        movement.MoveTo(new Vector3(x, 0, z));
    }

    private void UpdateJump()
    {
        if (Input.GetKeyDown(KeyCodeJump))
        {
            movement.Jump();
        }
    }

    private void UpdateWeaponAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weapon.StartWeaponAction();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            weapon.StopWeaponAction();
        }
        if ( Input.GetKeyDown(KeyCodeReload) )
        {
            weapon.StartReload();
        }
    }
}
