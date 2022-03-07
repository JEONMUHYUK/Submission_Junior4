using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private KeyCode jumpKeyCode = KeyCode.Space;
    [SerializeField]
    private CameraController cameraController; //RotateTo() 함수를 호출하기 위해 변수 생성
    private Movement3D movement3D; // 외부스크립트를 사용하기위한 필드설정
    
    private void Awake()
    {
        movement3D = GetComponent<Movement3D>(); // 컴포넌트에 접근하기위한 메서드 할당
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");  // 방향키 좌/우 움직임
        float z = Input.GetAxisRaw("Vertical");    // 방향키 상/하 움직임

        movement3D.MoveTo(new Vector3(x, 0, z)); // movement3D 스크립트의 MoveTo함수안의 direction 파라미터의 값을 여기서 전달해줄수 있다.

        // 점프키를 눌러 y축 방향으로 뛰어오르기
        if ( Input.GetKeyDown(jumpKeyCode) )
        {
            movement3D.JumpTo();
        }

        float mouseX = Input.GetAxis("Mouse X"); //마우스 좌/우 움직임
        float mouseY = Input.GetAxis("Mouse Y"); //마우스 상/하 움직임

        cameraController.RotateTo(mouseX, mouseY);
    }
}
