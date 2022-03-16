using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMedicBag : ItemBase
{
    
    [SerializeField]
    private GameObject  hpEffectPrefab;
    [SerializeField]
    private int         increaseHP = 50;
    [SerializeField]
    private float       moveDistance = 0.2f;
    [SerializeField]
    private float       pingpongSpeed = 0.5f;
    [SerializeField]
    private float       rotateSpeed = 50f;

    
    private IEnumerator Start()
    {
        // 현재 y의 위치를 변수 y에 저장
        float y = transform.position.y;
        

        while (true)
        {
            // y축을 기준으로 회전
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

            // 처음 배치된 위치를 기준으로 y 위치를 위, 아래로 이동
            Vector3 position    = transform.position;
            // y부터 y + moveDistance 까지 무한 왕복 이동한다.
            position.y          = Mathf.Lerp(y, y+moveDistance, Mathf.PingPong(Time.time * pingpongSpeed, 1));
            transform.position  = position;

            yield return null;
        } 
    }

    // Item을 획득할 시 호출되는 메서드
    public override void Use(GameObject entity)
    {
        entity.GetComponent<Status>().IncreaseHP(increaseHP);

        Instantiate(hpEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
