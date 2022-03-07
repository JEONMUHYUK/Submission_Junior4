using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float rotateSpeedX = 3;
    private float rotateSpeedY = 5;

    private float limitMinX = -80;
    private float limitMaxX = 50;

    private float eulerAngleX;
    private float eulerAngleY;

    public void RotateTo(float mouseX, float mouseY)
    {
        // 마우스를 좌/우로 움직이는 mouseX 값을 y축에 대입하는 이유는
        // 마우스를 좌/우로 움직일 때 카메라도 좌/우를 보려면 카메라 오브젝트의 y축이 회전되어야 하기 때문이다.

        eulerAngleY += mouseX * rotateSpeedX; //카메라 오브젝트를 좌/우로 움직일려면 카메라 오브젝트의 y축으로 움직여야하기 때문이다
        eulerAngleX -= mouseY * rotateSpeedY; //즉, 오일러앵글 x가 상/하, 오일러앵글 y는 좌/우를 뜻한다. 
        // 단, 카메라가 아래를 보는것은 양수이고 마우스가 아래로 이동하는 것은 음수이기 때문에 eulerAngleX -= mouseY 식이 나온다.

        // x축 회전 값의 경우 아래, 위를 볼 수 있는 제한각도가 설정되어 있다.
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        //실제 오브젝트의 쿼터니온 회전에 적용.
        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
        
    }

    private float ClampAngle(float angle, float min, float max) // 위의 오일러앵글 x 값과 최소 최대 값을 받아온다.
    {
        if( angle < -360 ) angle += 360;
        if( angle > 360 ) angle -= 360;

        // Mathf.Clamp를 이용해 angle이 man <= angle <= max 를 유지하도록 한다.
        return Mathf.Clamp(angle, min, max); // 최소 / 최대값을 설정하여 float 값이 범위 이외의 값을 넘지 않도록 한다.
    }
}
