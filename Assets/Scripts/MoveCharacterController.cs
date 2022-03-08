using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // 이 명령이 포함된 스크립트를 게임 오브젝트에 컴포넌트로 적용하면 해당 컴포넌트도 같이 추가된다.
public class MoveCharacterController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed; //이동 속도
    private Vector3 moveForce; //이동 힘(x, z와 y축을 별도로 계산해 실제 이동에 적용)

    [SerializeField]
    private float jumpForce;        //점프힘
    [SerializeField]
    private float gravity;        // 중력 계수

//------------------------------------------------------------------------------------------------------------------
    public float MoveSpeed 
    {
        // 외부에서 이동속도를 정의할 수 있도록 프로퍼티를 정의
        set => moveSpeed = Mathf.Max(0, value); // 0과 value중 더 큰값을 반환하게 하여 음수가 되지 않게 한다.
        get => moveSpeed;
    }
// -----------------------------------------------------------------------------------------------------------
    private CharacterController characterController; // 플레이어 이동 제어를 위한 컴포넌트

    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    private void Update() {       
        //허공에 떠있으면 중력만큼 y축 이동속도 감소
        if (!characterController.isGrounded)
        {
            moveForce.y += gravity * Time.deltaTime;
        }

        // 1초당 moveForce 속력으로 이동
        characterController.Move(moveForce * Time.deltaTime); //캐릭터 컨트롤러로 실제 이동
    }

    public void MoveTo(Vector3 direction) //이동값을 PlayerController에서 받아온다
    {
        // 이동방향 = 캐릭터 회전 값 * 방향 값
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);

        // 이동 힘 = 이동방향 * 속도
        moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);
        // - y값을 moveForce로 하는 이유는 direction을 그대로 사용하면 위나 아래를 바라보고 이동할 경우
        // 캐릭터가 공중으로 뜨거나 아래로 가라앉으려 하기 때문이다.
    }

    public void Jump()
    {
        // 플레이어가 바닥에 있을 때만 점프 가능
        if (characterController.isGrounded)
        {
            moveForce.y = jumpForce;
        }
    }
}
