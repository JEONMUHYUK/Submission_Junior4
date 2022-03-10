using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasingMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject casingPrefab;            // 탄피 오브젝트
    private MemoryPool memoryPool;              // 탄피 메모리풀
    
    private void Awake() {
        memoryPool = new MemoryPool(casingPrefab);
    }

    public void SapwnCasing(Vector3 position, Vector3 direction)        // 위치와 방향을 매게변수로 받는다.
    {
        GameObject item = memoryPool.ActivatePoolItem();                // 오브젝트를 하나 선택해 활성화한다.
        item.transform.position = position;                             // 위치와 회전값 설정
        item.transform.rotation = Random.rotation;
        item.GetComponent<Casing>().Setup(memoryPool, direction);       // casing 매서드의 setup호출
    }
}
