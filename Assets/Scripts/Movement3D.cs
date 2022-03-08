using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [SerializeField] // 인스펙터에서만 수정 가능하게 하기위해 SeroalozeField를 붙이고 변수의 접근제한자는 private로 한다.
    private float moveSpeed = 5.0f;  // 이동속도
    [SerializeField]
    private float jumpForce = 3.0f;
    private float gravity = -9.81f; //중력 계수
    private Vector3 moveDirection; // 이동방향

    GameObject nearObject;

    [SerializeField]
    private Transform cameraTransform; // 카메라 transform 컴포넌트
    private CharacterController characterController; // 캐릭터 컨트롤러 변수.
    // Start is called before the first frame update
    private void Awake()
    {
        characterController = GetComponent<CharacterController>(); //캐릭터컨트롤러 컴포넌트에 접근하기위해 필드명을 설정.
    }

    // Update is called once per frame
    void Update()
    {
        if ( characterController.isGrounded == false ) // 발위치를 체크하여 발이 충돌되어있지 않을 때를 확인.
        {
            moveDirection.y += gravity * Time.deltaTime; // movedirection의 y축 이동방향에 중력을 넣어준다.
        }   
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        //매게변수로 (이동방향, 속도, 시간)등의 세부적인 이동방법을 설정하면 이동을 수행한다.
    }

    public void MoveTo(Vector3 direction) //이동을 담당하는 함수. ---- 외부에서 호출하여 파라미터를 전달 받는다.
    {
        //moveDirection = direction; //외부에서 받은 파라미터를 moveDirection에 할당한다. -> y축이 파라미터 direction값의 영향을 받지 않도록 한다.
        //moveDirection = new Vector3(direction.x, moveDirection.y, direction.z); // x와 z값은 파라미터direction의 값을 받도록 하고 y축은 파라미터값을 받지 않도록 한다. 

        // 카메라가 보고있는 전방 방향으로 이동할 수 있도록 한다.
        Vector3 moveThis = cameraTransform.rotation * direction; // 카메라컴포넌트의 rotation(쿼터니온 회전값) 값 * PlayerController.cs 에서 받아온 파라미터값
        moveDirection = new Vector3(moveThis.x, moveDirection.y, moveThis.z); // 다시 move값을 받아온 값으로 재 정의 해준다.
    }

    public void JumpTo() // 점프를 담당하는 함수
    {
        if (characterController.isGrounded == true)
        {   
            moveDirection.y = jumpForce; // 무브다이렉션의 y축값에 점프값을 대입한다.
        }
    }

    public void OnTriggerStay(Collider other) {
        if(other.tag == "Weapon")
        {
            nearObject = other.gameObject;

            Debug.Log(nearObject.name);
        }
    }

    public void OnTriggerExit(Collider other) {
        if(other.tag == "Weapon")
        {
            nearObject = null;
        }
    }
}
