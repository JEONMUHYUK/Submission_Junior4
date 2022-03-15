using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBarrel : InteractionObject
{
    public override void TakeDamage(int damage)
    {
        // 기본드럼통은 설정상 체력이 무한해서 부서지지 않기 떄문에 체력이 닳지 않도록 TakeDamage 내부가 비어있다.
    }
}
