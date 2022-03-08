using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    [SerializeField]
    private float rotCamXAxisSpeed = 5; // 카메라 x축 회전속도
    [SerializeField]
    private float rotCamYAxisSpeed = 3; // 카메라 y축 회전속도

    private float limitMinX = -80; // 카메라 x축 회전 범위 (최소)
    private float limitMaxX = 50; // 카메라 x축 회전 범위 (최대)
    private float eulerAngleX;
    private float eulerAngleY;

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed; // 마우스 좌/우 이동으로 카메라 y축 회전
        eulerAngleX -= mouseY * rotCamXAxisSpeed; // 마우스 상/하 이동으로 카메라 x축 회전
        // 마우스를 아래(y축)로 내리면 -로 음수인데 오브젝트(eulerAngleX)의 x축이 +방향으로 회전해야 아래를 보기 때문이다.

        //카메라 x축 회전의 경우 회전 범위를 설정
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0); // 마우스를 실제 회전시킬 수 있는 함수.
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if ( angle < -360 ) angle += 360;
        if ( angle > 360 ) angle -= 360;

        // Mathf.Clamp를 이용해 angle이 min <= angle <= max 를 유지하도록 한다.
        return Mathf.Clamp(angle, min, max); 
    }
}
